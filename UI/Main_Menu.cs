using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    public GameObject GameObject;

    // Loads new scene
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    // Exits the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("KELUAR");
    }
}
