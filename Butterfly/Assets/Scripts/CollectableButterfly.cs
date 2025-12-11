using UnityEngine;
using TMPro;

public class CollectableButterfly : MonoBehaviour
{
    public KeyCode collectKey = KeyCode.E;

    private bool playerInRange = false;
    private PlayerScore playerScore;
    private Transform player;

    [Header("UI Prompt")]
    public GameObject collectPrompt;  

    void Start()
    {
        playerScore = FindObjectOfType<PlayerScore>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (collectPrompt != null)
            collectPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(collectKey))
        {
            Collect();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (collectPrompt != null)
                collectPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (collectPrompt != null)
                collectPrompt.SetActive(false);
        }
    }

    private void Collect()
    {
        if (collectPrompt != null)
            collectPrompt.SetActive(false);

        playerScore.AddPoint();
        Destroy(gameObject);
    }
}
