using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Le personnage à suivre
    public Vector3 offset = new Vector3(0, 5, -10); // Décalage de la caméra par rapport au personnage
    public float smoothSpeed = 0.125f; // Vitesse de suivi fluide

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
