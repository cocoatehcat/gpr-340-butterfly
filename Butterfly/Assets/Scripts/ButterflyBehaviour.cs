using System.Collections.Generic;
using UnityEngine;

public class ButterflyBehaviour : MonoBehaviour
{
    public enum State { SeekingFlower, Escaping }
    private State state = State.SeekingFlower;

    [Header("Dependencies")]
    public GridManager gridManager;
    public Pathfinder pathfinder;
    public Transform player;

    [Header("Settings")]
    public float hoverHeight = 1.2f;
    public float playerDetectionRadius = 6f;
    public float escapeDistance = 12f;
    public float flowerReachDistance = 0.6f;

    private AgentController agent;
    private LineRenderer lr;

    private Vector3 targetPos;
    private List<Node> currentPath = null;

    // Cache flower list 
    private static GameObject[] cachedFlowers;

    private float escapeRepathTimer = 0f;
    private const float ESCAPE_REPATH_DELAY = 0.5f;

    private void Awake()
    {
        agent = GetComponent<AgentController>();

        // Path visualization
        GameObject lrObj = new GameObject("PathLine_" + name);
        lr = lrObj.AddComponent<LineRenderer>();
        lr.widthMultiplier = 0.08f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.yellow;
        lr.endColor = Color.green;
    }

    public void InitializeAfterSpawn()
    {
        CacheFlowers();
        PickNewFlowerTarget();
    }

    private void Update()
    {
        if (player == null || gridManager == null) return;

        switch (state)
        {
            case State.SeekingFlower: UpdateSeeking(); break;
            case State.Escaping: UpdateEscaping(); break;
        }
    }

    // SEEKING FLOWERS
    private void UpdateSeeking()
    {
        // Player too close  ... escape
        if (Vector3.Distance(transform.position, player.position) < playerDetectionRadius)
            EnterEscapeState();

        // Reached flower .... pick new one
        else if (Vector3.Distance(transform.position, targetPos) < flowerReachDistance)
            PickNewFlowerTarget();

        // No path or finished ... recalc
        else if (agent.HasFinishedPath())
            PickNewFlowerTarget();
    }

    private void CacheFlowers()
    {
        if (cachedFlowers == null)
            cachedFlowers = GameObject.FindGameObjectsWithTag("Flower");
    }

    private void PickNewFlowerTarget()
    {
        CacheFlowers();
        if (cachedFlowers.Length == 0)
        {
            Debug.LogWarning("No flowers found!");
            return;
        }

        float bestDist = Mathf.Infinity;
        GameObject best = null;

        foreach (var f in cachedFlowers)
        {
            float d = Vector3.Distance(transform.position, f.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = f;
            }
        }

        if (best != null)
        {
            targetPos = best.transform.position;
            ComputePath(targetPos);
        }
    }

    // ESCAPING PLAYER
    private void EnterEscapeState()
    {
        if (state == State.Escaping) return; // prevent double trigger

        state = State.Escaping;
        escapeRepathTimer = ESCAPE_REPATH_DELAY;
        ComputeEscapePath();
    }

    private void UpdateEscaping()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Safe again ... resume seeking
        if (dist > playerDetectionRadius * 1.6f)
        {
            state = State.SeekingFlower;
            PickNewFlowerTarget();
            return;
        }

        // Repath if finished or escape continues
        escapeRepathTimer -= Time.deltaTime;
        if (agent.HasFinishedPath() && escapeRepathTimer <= 0f)
        {
            escapeRepathTimer = ESCAPE_REPATH_DELAY;
            ComputeEscapePath();
        }
    }

    private void ComputeEscapePath()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 fleePos = transform.position + direction * escapeDistance;

        fleePos = gridManager.ClampToGrid(fleePos);

        Node farNode = gridManager.NodeFromWorldPosition(fleePos);

        if (farNode == null || !farNode.walkable)
            farNode = gridManager.GetFarthestNodeFrom(player.position);

        targetPos = farNode.worldPosition;
        ComputePath(targetPos);
    }

    // PATHFINDING
    private void ComputePath(Vector3 worldTarget)
    {
        Node start = gridManager.NodeFromWorldPosition(transform.position);
        Node target = gridManager.NodeFromWorldPosition(worldTarget);

        if (start == null || target == null)
        {
            lr.positionCount = 0;
            return;
        }

        currentPath = pathfinder.FindPath(start, target);

        if (currentPath == null || currentPath.Count == 0)
        {
            lr.positionCount = 0;
            return;
        }

        List<Vector3> waypoints = new List<Vector3>();
        foreach (Node n in currentPath)
        {
            Vector3 p = n.worldPosition;
            p.y = hoverHeight;
            waypoints.Add(p);
        }

        agent.SetPath(waypoints);
        DrawPath();
    }

    private void DrawPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            lr.positionCount = 0;
            return;
        }

        lr.positionCount = currentPath.Count;

        for (int i = 0; i < currentPath.Count; i++)
            lr.SetPosition(i, currentPath[i].worldPosition + Vector3.up * 0.25f);
    }
}

//    private void ChangeState(ButterflyState newState)
//    {
//        if (currentState == newState) return;

//        currentState = newState;
//        Debug.Log($"Butterfly state changed to: {newState}");

//        if (newState == ButterflyState.Escaping)
//            StartEscape();

//        if (newState == ButterflyState.SeekingFlower)
//            FindNewFlowerTarget();

//    }

//    private void OnDrawGizmos()
//    {
//        // Draw start (current position)
//        Gizmos.color = Color.green;
//        Gizmos.DrawSphere(transform.position + Vector3.up * 0.2f, 0.2f);

//        // Draw target (flower)
//        if (targetPos != Vector3.zero)
//        {
//            Gizmos.color = Color.magenta;
//            Gizmos.DrawSphere(targetPos + Vector3.up * 0.2f, 0.5f);
//            Gizmos.DrawLine(transform.position, targetPos);
//        }

//        // Optionally draw path nodes
//        if (currentPath != null && currentPath.Count > 1)
//        {
//            Gizmos.color = Color.cyan;
//            for (int i = 0; i < currentPath.Count - 1; i++)
//            {
//                Gizmos.DrawLine(currentPath[i].worldPosition + Vector3.up * 0.2f,
//                                currentPath[i + 1].worldPosition + Vector3.up * 0.2f);
//            }
//        }
//    }
//}