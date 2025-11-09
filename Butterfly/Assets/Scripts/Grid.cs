using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private bool debugMenu;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private GameObject tree;

    private Vector3[] vertices;
    private Mesh mesh;
    private MeshCollider mCollider;

    private TreeGenerator treeGen;

    private void Awake()
    {
        mCollider = GetComponent<MeshCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawGrid();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.greenYellow;

  

    }

    public void DrawGrid()
    {
        // Reset for regen
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Grid";

        vertices = new Vector3[(width + 1) * (height + 1)];

        // placement points
        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);

                
            }
        }

        // Generating actual mesh
        mesh.vertices = vertices;
        int[] triangles = new int[width * height * 6];
        for (int ti = 0, vi = 0, z = 0; z < height; z++, vi++)
        {
            for (int x = 0; x < width; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                triangles[ti + 5] = vi + width + 2;
                mesh.triangles = triangles;

                // Tree
                float modifier = Random.Range(10, 20);

                Instantiate(tree);
                tree.transform.position = new Vector3(x * modifier, tree.transform.position.y, z * modifier);
                
            }

        }

       
    }
}

// Generate new grid
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var grid = (Grid)target;

        if (GUILayout.Button("Reload Grid", GUILayout.Width(200)))
        {
            grid.DrawGrid();
        }
    }
}