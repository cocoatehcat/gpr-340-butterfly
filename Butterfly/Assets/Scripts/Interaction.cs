using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/*
 * 
 * Created by: Arija Hartel (@cocoatehcat)
 * Purpose: Interaction between the player and the Butterfly
 * 
 */

public class Interaction : MonoBehaviour
{
    private GameObject[] butterflies;
    private CapsuleCollider self;

    private void Start()
    {
        butterflies = GameObject.FindGameObjectsWithTag("Butterfly");
        self = transform.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
    }

    void OnInteract(InputValue interactValue)
    {
        print("What");
        for (int i = 0; i < butterflies.Length; i++)
        {
            var interactionPosition = butterflies[i].GetComponentInChildren<SphereCollider>();

            // distance between player and butterfly
            float dist = Vector3.Distance(transform.position, interactionPosition.transform.position);

            // check if inside butterfly radius
            if (dist <= interactionPosition.radius * interactionPosition.transform.localScale.x)
            {
                // collect the butterfly
                butterflies[i].SetActive(false);
                return;
            }
        }
    }
}