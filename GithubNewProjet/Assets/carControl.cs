using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Car Settings")]
    public float motorForce = 1000f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 15f; // Reduced steering angle

    [Header("Stability Control")]
    public float stabilityForce = 10f;
    public float centerOfMassOffset = -0.7f;
    public float tractionControl = 0.95f; // Traction control multiplier

    private Rigidbody carRigidbody;
    private float horizontalInput;
    private float verticalInput;

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
        verticalInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        horizontalInput = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
    }

    private void HandleMotor()
    {
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        
        // Reduce motor force at high speeds and during turning
        float adjustedMotorForce = motorForce * Mathf.Lerp(1f, 0.5f, 
            Mathf.Abs(horizontalInput) * currentSpeed / 50f);

        // Apply traction control
        rearLeftWheel.motorTorque = verticalInput * adjustedMotorForce * tractionControl;
        rearRightWheel.motorTorque = verticalInput * adjustedMotorForce * tractionControl;
    }

    private void HandleSteering()
    {
        // Progressive steering reduction at high speeds
        float currentSpeed = carRigidbody.linearVelocity.magnitude;
        float dynamicSteerAngle = Mathf.Lerp(maxSteerAngle, 5f, currentSpeed / 30f);

        frontLeftWheel.steerAngle = horizontalInput * dynamicSteerAngle;
        frontRightWheel.steerAngle = horizontalInput * dynamicSteerAngle;
    }

    private void ApplyStabilityControl()
    {
        // Advanced stability control
        Vector3 localVelocity = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        
        // Limit lateral slip
        float lateralSlip = Mathf.Abs(localVelocity.x);
        if (lateralSlip > 5f)
        {
            Vector3 stabilityTorque = -carRigidbody.angularVelocity * stabilityForce;
            carRigidbody.AddTorque(stabilityTorque);
        }

        // Additional downforce
        carRigidbody.AddForce(-transform.up * carRigidbody.linearVelocity.magnitude * stabilityForce);
    }
}