using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public int totalButterflies = 10;  
    public TMP_Text scoreText;
    public GameObject winScreen;
    public Timer timer;

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddPoint()
    {
        score++;
        Debug.Log("Score: " + score);
        UpdateScoreUI();

        if (score >= totalButterflies)
        {
            WinGame();
        }
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Butterflies: {score} / {totalButterflies}";
        }
    }

    private void WinGame()
    {
        Debug.Log("YOU WIN!");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (winScreen != null)
            winScreen.SetActive(true);

        // Display final time
        winScreen.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = timer.timeRemaining.ToString();

        Time.timeScale = 0f; // Freeze game
    }
}
