using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject winMenu;
    public GameObject player;
    public Timer timer;

    public void OnStartButton()
    {
        startMenu.SetActive(false);
        timer.timerIsRunning = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnRestartButton()
    {
        //winMenu.SetActive(false);
        //timer.timerText.text = $"{0:00}:{0:00}";
        //player.GetComponent<PlayerScore>().score = 0;
        //player.GetComponent<PlayerScore>().UpdateScoreUI();
        //player.transform.position = new Vector3(1.706f, 25f, 2.423f);

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
