using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public Timer timer;

    public void OnStartButton()
    {
        startMenu.SetActive(false);
        timer.timerIsRunning = true;
    }
}
