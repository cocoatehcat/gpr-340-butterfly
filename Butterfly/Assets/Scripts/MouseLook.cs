using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 200f;
    float xRotation = 0f;

    public Transform playerBody;  

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Vertical rotation (camera up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (rotate player left/right)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}