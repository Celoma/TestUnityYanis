using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Car Settings")]
    public float motorForce = 3000f;
    public float brakeForce = 8000f;
    public float maxSteerAngle = 15f;

    [Header("Stability Control")]
    public float stabilityForce = 10f;
    public float centerOfMassOffset = -0.7f;
    public float tractionControl = 0.95f;

    private Rigidbody carRigidbody;
    private float horizontalInput;
    private float verticalInput;
    private bool isBraking;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = new Vector3(0, centerOfMassOffset, 0);
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        ApplyStabilityControl();
    }

    private void GetInput()
    {
        // Separate controls for forward, brake, and reverse
        verticalInput = Input.GetKey(KeyCode.W) ? 1 : 
                        Input.GetKey(KeyCode.S) ? -1 : 0;
        horizontalInput = Input.GetKey(KeyCode.A) ? -1 : 
                          Input.GetKey(KeyCode.D) ? 1 : 0;
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        
        if (isBraking)
        {
            // Apply full brake force when spacebar is pressed
            ApplyBraking(brakeForce);
            verticalInput = 0; // Prevent motor input during braking
        }
        else if (verticalInput > 0)
        {
            // Forward motion
            float adjustedMotorForce = motorForce * Mathf.Lerp(1f, 0.5f, 
                Mathf.Abs(horizontalInput) * currentSpeed / 50f);

            rearLeftWheel.motorTorque = adjustedMotorForce * tractionControl;
            rearRightWheel.motorTorque = adjustedMotorForce * tractionControl;
            ApplyBraking(0); // Release brakes
        }
        else if (verticalInput < 0)
        {
            // Reverse motion
            rearLeftWheel.motorTorque = motorForce * tractionControl * -1;
            rearRightWheel.motorTorque = motorForce * tractionControl * -1;
            ApplyBraking(0); // Release brakes
        }
        else
        {
            // No input
            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
            ApplyBraking(brakeForce * 0.5f); // Soft brake
        }
    }

    private void ApplyBraking(float brakeAmount)
    {
        frontLeftWheel.brakeTorque = brakeAmount;
        frontRightWheel.brakeTorque = brakeAmount;
        rearLeftWheel.brakeTorque = brakeAmount;
        rearRightWheel.brakeTorque = brakeAmount;
    }

    private void HandleSteering()
    {
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float dynamicSteerAngle = Mathf.Lerp(maxSteerAngle, 5f, currentSpeed / 30f);

        frontLeftWheel.steerAngle = horizontalInput * dynamicSteerAngle;
        frontRightWheel.steerAngle = horizontalInput * dynamicSteerAngle;
    }

    private void ApplyStabilityControl()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        
        float lateralSlip = Mathf.Abs(localVelocity.x);
        if (lateralSlip > 5f)
        {
            Vector3 stabilityTorque = -carRigidbody.angularVelocity * stabilityForce;
            carRigidbody.AddTorque(stabilityTorque);
        }

        carRigidbody.AddForce(-transform.up * carRigidbody.linearVelocity.magnitude * stabilityForce);
    }
}