using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text score;
    private int ScoreValue;

    // Gets text component
    private void Start()
    {
        score = GetComponent<TMP_Text>();
    }

    // Increments score value
    public void updateScore(int newScore)
    {
        ScoreValue += newScore;
    }

    // Displays final score
    public void ShowGameOverScreen()
    {
        score.text = "Score: " + ScoreValue;
        this.gameObject.SetActive(true);
    }
}
