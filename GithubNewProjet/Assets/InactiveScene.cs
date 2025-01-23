using UnityEngine;
using UnityEngine.SceneManagement;

public class InactiveScene : MonoBehaviour
{
    // Start est appelé avant la première exécution d'Update
    void Start()
    {
        // Désactiver tous les GameObjects de la scène actuelle
        Scene currentScene = SceneManager.GetActiveScene();

        foreach (GameObject obj in currentScene.GetRootGameObjects())
        {
            obj.SetActive(false);
        }
    }
}
