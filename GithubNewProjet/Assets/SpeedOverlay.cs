using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler l'UI texte (si tu utilises Text normal)

public class SpeedOverlay : MonoBehaviour
{
    public Text speedText; // Référence au texte affiché sur le canvas
    public Transform target; // Transform de la voiture (ou de l'objet suivi)
    
    private Rigidbody rb; // Référence au Rigidbody de la voiture

    void Start()
    {
        // Assurez-vous que le Rigidbody est attaché à l'objet de la voiture
        rb = target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calcule la vitesse actuelle du véhicule (en m/s)
        float currentSpeed = rb.linearVelocity.magnitude;

        // Affiche la vitesse en km/h (ou m/s si tu préfères)
        speedText.text = "Speed: " + (currentSpeed * 8.6f).ToString("F0") + " km/h"; // Conversion en km/h

        // Si tu utilises TextMeshPro, remplace Text par TMP_Text et ajoute `using TMPro;` en haut
    }
}
