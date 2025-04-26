using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    public GameObject GameObject;

    // ( RAPI )

    // Play Button untuk load level
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    // Keluar Game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("KELUAR");
    }
}
