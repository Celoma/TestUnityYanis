using UnityEngine;

public class DynamicRaceCamera : MonoBehaviour
{
    public Transform target; // L'objet à suivre (votre véhicule)
    public float minSpeed = 0f; // Vitesse minimale
    public float maxSpeed = 20f; // Vitesse maximale
    public float minFOV = 60f; // FOV minimal
    public float maxFOV = 110f; // FOV maximal
    public float minZOffset = 5f; // Offset Z minimal
    public float maxZOffset = 3f; // Offset Z maximal
    public Vector3 offset = new Vector3(0, 2, -5); // Offset de base de la caméra
    public float smoothSpeed = 5f; // Vitesse de lissage de la position
    public bool stabilizeYAxis = true; // Stabiliser la hauteur de la caméra (axe Y)

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return; // Vérifiez que la cible est définie

        // Obtenez la vitesse actuelle de votre véhicule
        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody == null) return; // Assurez-vous que la cible a un Rigidbody
        float currentSpeed = targetRigidbody.linearVelocity.magnitude; // Vitesse en m/s

        // Affichez la vitesse actuelle dans la console (facultatif pour le debug)
        Debug.Log("Current Speed: " + currentSpeed);

        // Calculez le pourcentage de la vitesse maximale
        float speedPercentage = Mathf.Clamp01((currentSpeed - minSpeed) / (maxSpeed - minSpeed));

        // Ajustez le FOV de la caméra en fonction de la vitesse
        cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, speedPercentage);

        // Ajustez la position Z de la caméra (profondeur)
        float currentZOffset = Mathf.Lerp(minZOffset, maxZOffset, speedPercentage);
        float currentYOffset = Mathf.Lerp(5, 3, speedPercentage);

        Vector3 targetOffset = new Vector3(offset.x, currentYOffset, -currentZOffset);

        // Calculez la position souhaitée de la caméra
        Vector3 desiredPosition = target.position + target.TransformDirection(targetOffset);

        // Stabilisation optionnelle sur l'axe Y
        if (stabilizeYAxis)
        {
            desiredPosition.y = Mathf.Lerp(transform.position.y, desiredPosition.y, Time.deltaTime * smoothSpeed);
        }

        // Appliquez une transition lissée pour la position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // Faites en sorte que la caméra regarde toujours la cible
        transform.LookAt(target);
    }
}
