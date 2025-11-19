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
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(movementX * speed, 0.0f, movementY * speed);

        velocity.y = rb.linearVelocity.y;

        if (jumpCheck && IsGrounded())
        {
            velocity.y = jumpSpeed;
            jumpCheck = false;
        }

        rb.linearVelocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics.CheckBox(groundCheckPosition.position,groundCheckSize * 0.6f,Quaternion.identity,groundMask);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue jumpValue)
    {
        // Arija is bugfixing this
        //if (Physics.SphereCast() 
        //{
        //    jumpCheck = true;
        //}

        jumpCheck = true;
        
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellowGreen;
        Gizmos.DrawCube(groundCheckPosition.position, groundCheckSize);
    }
}
