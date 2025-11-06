using UnityEngine;
using UnityEngine.InputSystem;

/*
 * 
 * Created by: Arija Hartel (@cocoatehcat)
 * Purpose: For allowing the player to be able to move!
 * 
 */

public class CapsuleController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed = 5;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
}
