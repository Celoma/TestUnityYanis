using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    Button buttonStart;
    Button buttonExit;

    void Start()
    {
        // Récupère les boutons depuis la scène
        buttonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        buttonExit = GameObject.Find("ButtonExit").GetComponent<Button>();

        // Ajoute les listeners pour chaque bouton
        buttonStart.onClick.AddListener(OnStartButtonClick);
        buttonExit.onClick.AddListener(OnExitButtonClick);
    }

    void OnStartButtonClick()
    {
        SceneManager.LoadScene("TestScene");
    }

    void OnExitButtonClick()
    {
        // Quitte l'application
        Application.Quit();

        // Message de debug (utile uniquement dans l'éditeur Unity, car `Application.Quit` ne fonctionne pas dans l'éditeur)
        Debug.Log("Application Quit");
    }
}
