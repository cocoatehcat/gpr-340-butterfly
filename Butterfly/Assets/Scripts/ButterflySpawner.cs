using System.Collections;
using UnityEngine;

public class ButterflySpawner : MonoBehaviour
{
    public GameObject butterflyPrefab;
    public int butterflyCount = 10;

    public Transform player;
    public GridManager gridManager;
    public Pathfinder pathfinder;

    IEnumerator Start()
    {
        while (gridManager == null || gridManager.grid == null)
            yield return null;

        for (int i = 0; i < butterflyCount; i++)
        {
            Node n = GetRandomWalkableNode();
            Vector3 pos = n.worldPosition;
            pos.y = 1.2f;

            GameObject b = Instantiate(butterflyPrefab, pos, Quaternion.identity);

            var bb = b.GetComponent<ButterflyBehaviour>();
            bb.gridManager = gridManager;
            bb.pathfinder = pathfinder;
            bb.player = player;

            bb.InitializeAfterSpawn();
        }
    }

    private Node GetRandomWalkableNode()
    {
        Node n = null;
        while (n == null || !n.walkable)
        {
            int x = Random.Range(0, gridManager.gridWidth);
            int y = Random.Range(0, gridManager.gridHeight);
            n = gridManager.grid[x, y];
        }
        return n;
    }
}
