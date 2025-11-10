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

    public void OnInteract(InputValue interactValue)
    {
        for (int i = 0;  i < butterflies.Length; i++)
        {
            var interactionPosition = butterflies[i].GetComponentInChildren<SphereCollider>();

            
        }
    }
}

/*
 * Press e
 * check if bug is in radius
 * if there is bug in radius
 * collect bug
 * else return nothing
 */