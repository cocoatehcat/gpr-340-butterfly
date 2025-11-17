using System.Collections.Generic;
using UnityEngine;

public class ButterflyBehaviour : MonoBehaviour
{

    public enum ButterflyState { SeekingFlower, Escaping }
    public ButterflyState currentState = ButterflyState.SeekingFlower;

    public GridManager gridManager;
    public Pathfinder pathfinder;
    public Transform player;

    [Header("Behavior Settings")]
    public float playerDetectionRadius = 5f;
    public float escapeDistance = 10f;
    public float moveSpeed = 2.5f;
    public float flowerReachDistance = 0.5f;

    private Vector3 targetPos;
    private List<Node> currentPath = null;
    private int currentPathIndex = 0;

    private AgentController agent;



    private void Awake()
    {
        agent = GetComponent<AgentController>();
    }

    private void Start()
    {
        // Get initial flower path
        FindNewFlowerTarget();
    }

    private void Update()
    {
        switch (currentState)
        {
            case ButterflyState.SeekingFlower:
                HandleSeekingFlower();
                break;

            case ButterflyState.Escaping:
                // PLACEHOLDER. ADD FUNCTION FOR RUNNING AWAY HERE
                UpdateEscaping();
                print("Butterfly running");
                break;
        }
    }

    private void HandleSeekingFlower()
    {
        // Check for nearby player
        if (player != null && Vector3.Distance(transform.position, player.position) < playerDetectionRadius)
        {
            ChangeState(ButterflyState.Escaping);
            StartEscape();
            return;
        }

        // If no path, get one
        if (currentPath == null || currentPathIndex >= currentPath.Count)
        {
            FindNewFlowerTarget();
            return;
        }

        // Check if reached flower
        if (Vector3.Distance(transform.position, targetPos) < flowerReachDistance)
        {
            FindNewFlowerTarget();
        }
    }

    private void FindNewFlowerTarget()
    {
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
        //if (flowers.Length == 0)
        //{
        //    Debug.LogWarning("No flowers found!");
        //    return;
        //}

        // Find nearest flower
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        foreach (var f in flowers)
        {
            float dist = Vector3.Distance(transform.position, f.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = f;
            }
        }

        if (nearest == null) return;

        targetPos = nearest.transform.position;
        ComputePathToTarget();


       //currentPath = pathfinder.FindPath(transform.position, targetPos);
       //currentPathIndex = 0; 
    }



    // ESCAPING
    private void StartEscape()
    {
        currentState = ButterflyState.Escaping;

        Vector3 runDir = (transform.position - player.position).normalized;
        Vector3 fleePos = transform.position + runDir * escapeDistance;

        // Clamp to walkable node
        Node node = gridManager.NodeFromWorldPosition(fleePos);
        if (node == null || !node.walkable)
            node = gridManager.GetFarthestNodeFrom(player.position);

        targetPos = node.worldPosition;
        ComputePathToTarget();
        //currentPath = pathfinder.FindPath(transform.position, targetPos);
        //currentPathIndex = 0;
    }

    private void UpdateEscaping()
    {
        if (Vector3.Distance(transform.position, player.position) > playerDetectionRadius * 2f)
        {
            print(targetPos); 
            ComputePathToTarget();
            //currentState = ButterflyState.SeekingFlower;
            //FindNewFlowerTarget();
            //return;
        }

        if (currentPath == null || currentPathIndex >= currentPath.Count)
        {
            StartEscape();
        }
    }


    private void ComputePathToTarget()
    {
        Node start = gridManager.NodeFromWorldPosition(transform.position);
        Node target = gridManager.NodeFromWorldPosition(targetPos);

        if (start == null || target == null)
        {
            Debug.LogWarning("Invalid start or target node for Butterfly.");
            return;
        }

        currentPath = pathfinder.FindPath(start, target);
        //currentPath = pathfinder.FindPath(transform.position, targetPos);
        currentPathIndex = 0;

        if (currentPath == null || currentPath.Count == 0)
        {
            Debug.Log("No path found for Butterfly.");
            return;
        }

        // Give path to AgentController (world-space waypoints)
        List<Vector3> waypoints = new List<Vector3>();
        foreach (Node n in currentPath)
        {
            waypoints.Add(n.worldPosition);
        }
        agent.SetPath(waypoints);
        DrawPath(currentPath);
    }

 
    private void DrawPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            print("No path");
            return;
        }


        GameObject lrgo = GameObject.Find("PathLineRenderer");
        LineRenderer lr;

        if (lrgo == null)
        {
            lrgo = new GameObject("PathLineRenderer");
            lr = lrgo.AddComponent<LineRenderer>();
            lr.widthMultiplier = 0.2f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.yellow;
            lr.endColor = Color.green;
        }
        else
        {
            lr = lrgo.GetComponent<LineRenderer>();
        }

        lr.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            lr.SetPosition(i, path[i].worldPosition + Vector3.up * 0.2f);
        }
    }

    private void ChangeState(ButterflyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log($"Butterfly state changed to: {newState}");

        if (newState == ButterflyState.Escaping)
            StartEscape();
        //else if (newState == ButterflyState.SeekingFlower)
        //    FindNewFlowerTarget();

        if (newState == ButterflyState.SeekingFlower)
            FindNewFlowerTarget();

    }

    private void OnDrawGizmos()
    {
        // Draw start (current position)
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.2f, 0.2f);

        // Draw target (flower)
        if (targetPos != Vector3.zero)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(targetPos + Vector3.up * 0.2f, 0.5f);
            Gizmos.DrawLine(transform.position, targetPos);
        }

        // Optionally draw path nodes
        if (currentPath != null && currentPath.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine(currentPath[i].worldPosition + Vector3.up * 0.2f,
                                currentPath[i + 1].worldPosition + Vector3.up * 0.2f);
            }
        }
    }
}
