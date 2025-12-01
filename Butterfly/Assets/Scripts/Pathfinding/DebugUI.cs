using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public GridManager gridManager;
    public Pathfinder pathfinder;
    public HeightmapGenerator heightmapGenerator;

    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject agentPrefab;

    private Vector3 sourcePos;
    private Vector3 targetPos;
    private bool placingSource = false;
    private bool placingTarget = false;
    private GameObject debugSourceGO;
    private GameObject debugTargetGO;
    private GameObject spawnedAgent;

    void Start()
    {

        // default
        gridManager.InitializeGrid();
    }

    public void PlaceObstacle(Vector3 pos)
    {
        GameObject go;

        if (obstaclePrefab == null)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "ObstacleAuto";
        }
        else
        {
            go = Instantiate(obstaclePrefab, pos, Quaternion.identity);
        }

        go.transform.position = pos + Vector3.up * 0.5f;
        go.tag = "Obstacle";
        go.layer = LayerMask.NameToLayer("Obstacle");

        if (go.GetComponent<Collider>() == null)
            go.AddComponent<BoxCollider>();

        gridManager.InitializeGrid();
    }

    public void OnReset()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Obstacle"))
            Destroy(go);

        GameObject lrgo = GameObject.Find("PathLineRenderer");
        if (lrgo != null)
            Destroy(lrgo);

        heightmapGenerator.GenerateHeightmap();
        heightmapGenerator.RebuildMesh();

        if (debugSourceGO) Destroy(debugSourceGO);
        if (debugTargetGO) Destroy(debugTargetGO);
        if (spawnedAgent) Destroy(spawnedAgent);

        gridManager.InitializeGrid();
    }

    public void OnRunPath()
    {
        Node start = gridManager.NodeFromWorldPosition(sourcePos);
        Node target = gridManager.NodeFromWorldPosition(targetPos);

        if (start == null || target == null) { Debug.LogWarning("Place source and target first."); return; }

        List<Node> path = pathfinder.FindPath(start, target);
        if (path == null)
        {
            Debug.Log("No path found.");
            return;
        }

        DrawPath(path);

    }

    private void DrawPath(List<Node> path)
    {
        // a simple line renderer
        GameObject lrgo = GameObject.Find("PathLineRenderer");
        if (lrgo == null)
        {
            lrgo = new GameObject("PathLineRenderer");
            var lr = lrgo.AddComponent<LineRenderer>();
            lr.widthMultiplier = 0.4f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
                lr.SetPosition(i, path[i].worldPosition + Vector3.up * 0.2f);
        }
        else
        {
            var lr = lrgo.GetComponent<LineRenderer>();
            lr.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
                lr.SetPosition(i, path[i].worldPosition + Vector3.up * 0.2f);
        }
    }

    public void OnSpawnAgent()
    {
        if (agentPrefab == null)
        {
            spawnedAgent = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            spawnedAgent.transform.position = sourcePos + Vector3.up * 1f;
        }
        else
        {
            spawnedAgent = Instantiate(agentPrefab, sourcePos + Vector3.up * 1f, Quaternion.identity);
        }

        // get the last drawn path positions
        GameObject lrgo = GameObject.Find("PathLineRenderer");
        if (lrgo == null)
        {
            Debug.LogWarning("No path to follow. Run pathfinding first.");
            return;
        }
        var lr = lrgo.GetComponent<LineRenderer>();
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < lr.positionCount; i++) points.Add(lr.GetPosition(i));
        var ac = spawnedAgent.GetComponent<AgentController>();
        if (ac == null) ac = spawnedAgent.AddComponent<AgentController>();
        ac.SetPath(points);
    }
}
