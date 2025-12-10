//using UnityEngine;

//public class PlayerScore : MonoBehaviour
//{
//    public int score = 0;

//    public void AddPoint()
//    {
//        score++;
//        Debug.Log("Score: " + score);
//    }
//}
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public int totalButterflies = 10;   // Set this from the  spawner
    public TMP_Text scoreText;
    public GameObject winScreen;

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
        if (winScreen != null)
            winScreen.SetActive(true);

        Time.timeScale = 0f; // Freeze game
    }
}
