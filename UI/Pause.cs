using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    // Initializes paused state
    private void Start()
    {
        isPaused = true;
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
    }

    // Handles unpause input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
            ResumeGame();
        }
    }

    // Pauses the game
    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    // Resumes the game
    public void ResumeGame()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Restarts the level
    public void RestartGame(string sceneName)
    {
        isPaused = false;
        SceneManager.LoadScene(sceneName);
    }

    // Loads main menu
    public void GoToMainMenu(string sceneName)
    {
        isPaused = false;
        SceneManager.LoadScene(sceneName);
    }
}
