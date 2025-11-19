using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    public GridManager gridManager;
    public Camera playerCamera;
    public float baseCost = 1f;
    public float heightPenaltyFactor = 2f;
    public float diagonalCost = 1.4142f;
    private float nextVisUpdate;
    public float visablityCost = 3f;
    public bool allowDiagonal = true;

    void Start()
    {
        if (gridManager == null) gridManager = GridManager.Instance;
    }

    private void Update()
    {
        if (Time.time >= nextVisUpdate)
        {
            UpdateVisibilityCost();
            // Updating about 5 times per second
            nextVisUpdate = Time.time + 0.2f;
        }
    }

    public List<Node> FindPath(Vector3 startWorld, Vector3 targetWorld)
    {
        Node start = gridManager.NodeFromWorldPosition(startWorld);
        Node target = gridManager.NodeFromWorldPosition(targetWorld);
        return FindPath(start, target);
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        if (startNode == null || targetNode == null) return null;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        startNode.gCost = 0;
        startNode.hCost = Heuristic(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            Node current = openSet.OrderBy(n => n.fCost).First();
            if (current == targetNode)
                return RetracePath(startNode: startNode, endNode: targetNode);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node neighbor in gridManager.GetNeighbors(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                float tentativeG = current.gCost + MovementCost(current, neighbor);
                if (!openSet.Contains(neighbor) || tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = Heuristic(neighbor, targetNode);
                    neighbor.parent = current;
                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        // no path
        return null;
    }

    private bool IsVisibleToCamera(Vector3 worldPos)
    {
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(worldPos);

        // Check if inside camera frustum
        bool inView = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0;

        if (!inView) return false;

        if (Physics.Raycast(playerCamera.transform.position, (worldPos - playerCamera.transform.position).normalized, out RaycastHit hit))
        {
            return hit.collider != null && hit.collider.transform.position == worldPos;
        }

        return false;
    }

    public void UpdateVisibilityCost()
    {
        foreach (Node node in gridManager.grid)
        {
            if (IsVisibleToCamera(node.worldPosition))
                node.visablityCost = visablityCost;
            else
                node.visablityCost = 1f;
        }
    }

    private float MovementCost(Node a, Node b)
    {
        float planarDist = (a.gridX != b.gridX && a.gridY != b.gridY) ? diagonalCost : baseCost;
        float heightDiff = Mathf.Abs(a.worldPosition.y - b.worldPosition.y);
        float heightPenalty = 1f + heightPenaltyFactor * heightDiff;
        float visibility = Mathf.Max(a.visablityCost, b.visablityCost);
        float weight = (a.weight + b.weight) * 0.5f;
        return planarDist * baseCost * weight * heightPenalty * visibility;
    }

    private float Heuristic(Node a, Node b)
    {
        float dx = Mathf.Abs(a.gridX - b.gridX);
        float dy = Mathf.Abs(a.gridY - b.gridY);
        float h = (dx + dy) + (diagonalCost - 2f) * Mathf.Min(dx, dy);

        float dv = Mathf.Abs(a.worldPosition.y - b.worldPosition.y);
        return h + dv * heightPenaltyFactor;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;
        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
            if (current == null) break;
        }
        path.Reverse();
        return path;
    }
}