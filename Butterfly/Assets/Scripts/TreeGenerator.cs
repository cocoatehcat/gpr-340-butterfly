using Unity.VisualScripting;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{

    public void GenerateTree(Vector3 position, GameObject gameObject)
    {
        float num = Random.value;

        if (num > 0.8) // 0.8
        {
            Instantiate(gameObject);
            gameObject.transform.position = position;
        }
    }
}
