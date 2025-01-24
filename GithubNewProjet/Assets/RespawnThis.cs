using UnityEngine;

public class RespawnThis : MonoBehaviour
{
    private Vector3 respawnPosition = new Vector3(0, 1, 0); // Position de respawn (0, 1, 0)
    private Rigidbody rb;

    void Start()
    {
        // Récupérer le composant Rigidbody s'il existe
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Vérifier si la touche Tab est pressée
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Déplacer l'objet à la position de respawn
        transform.position = respawnPosition;

        // Réinitialiser la rotation de l'objet
        transform.rotation = Quaternion.identity;

        // Si l'objet a un Rigidbody, réinitialiser sa vitesse
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;   
            rb.angularVelocity = Vector3.zero;
        }
    }
}
