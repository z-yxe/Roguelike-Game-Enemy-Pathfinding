using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    // ( RAPI )

    private void Start()
    {
        isPaused = true;
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
    }

    private void Update()
    {
        // Bind (ESC) untuk keluar dari pause Screen
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
            ResumeGame();
        }
    }

    // Pasue pake time freeze
    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    // Resume
    public void ResumeGame()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Restart
    public void RestartGame(string sceneName)
    {
        isPaused = false;
        SceneManager.LoadScene(sceneName);
    }

    // Balik ke menu
    public void GoToMainMenu(string sceneName)
    {
        isPaused = false;
        SceneManager.LoadScene(sceneName);
    }
    

}
