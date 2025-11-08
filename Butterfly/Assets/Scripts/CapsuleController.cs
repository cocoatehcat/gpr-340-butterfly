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
    private Vector3 startPos;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpSpeed;

    private bool jumpCheck;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(movementX * speed, 0.0f, movementY * speed);

        velocity.y = rb.linearVelocity.y;

        if (jumpCheck)
        {
            velocity.y = jumpSpeed;
            jumpCheck = false;
        }

        rb.linearVelocity = velocity;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue jumpValue)
    {
        if (transform.position.y > startPos.y)
        {
            return;
        }
        jumpCheck = true;
    }
}
