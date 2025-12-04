using UnityEngine;

public class CollectableButterfly : MonoBehaviour
{
    public KeyCode collectKey = KeyCode.E;

    private bool playerNearby = false;
    private PlayerScore playerScore;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(collectKey))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (playerScore != null)
        {
            playerScore.AddPoint();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            playerScore = other.GetComponent<PlayerScore>();
            Debug.Log("Player entered butterfly trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player left butterfly trigger");
        }
    }
}
