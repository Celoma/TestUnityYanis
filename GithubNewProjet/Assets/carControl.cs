using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 6000;            // Couple moteur élevé pour une accélération puissante
    public float brakeTorque = 8000;            // Couple de freinage puissant pour un arrêt rapide
    public float maxSpeed = 80;                 // Vitesse maximale élevée pour refléter les performances d'une F1
    public float steeringRange = 15;            // Direction précise mais pas trop large
    public float steeringRangeAtMaxSpeed = 5;   // Réduction importante de l'angle de direction à grande vitesse pour plus de stabilité
    public float centreOfGravityOffset = -0.5f;

    public float CurrentSpeed { get; private set; } // Vitesse actuelle en km/h (lecture seule)

    WheelControl[] wheels;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        // Calculate current speed in relation to the forward direction of the car
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);

        // Update CurrentSpeed (convert m/s to km/h)
        CurrentSpeed = rigidBody.linearVelocity.magnitude * 3.6f;

        // Calculate how close the car is to top speed
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }
            
            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}
