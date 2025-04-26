using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text score;
    private int ScoreValue;

    // ( RAPI )

    private void Start()
    {
        score = GetComponent<TMP_Text>();
    }

    // Update score sesuai score player
    public void updateScore(int newScore)
    {
        ScoreValue += newScore;
    }

    // Pop-up game over screen
    public void ShowGameOverScreen()
    {
        score.text = "Score: " + ScoreValue;
        this.gameObject.SetActive(true);
    }
}
