using UnityEngine;

// Reviewed the Unity Page on 3D camera movement
public class MouseLook : MonoBehaviour
{
    public Transform trackedObject;
    public float maxDistance = 10;
    public float moveSpeed = 20;
    public float updateSpeed = 10;
    [Range(0, 10)]
    public float currentDistance = 5;
    private string moveAxis = "Mouse ScrollWheel";
    private GameObject ahead;
    private MeshRenderer _renderer;
    public float hideDistance = 1.5f;
    private float yRotation;

    void Start()
    {
        ahead = new GameObject("ahead");
        _renderer = trackedObject.gameObject.GetComponent<MeshRenderer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Track behind the player
        // She's a little buggy but she works so maybe fine tuning?
        ahead.transform.position = trackedObject.position + trackedObject.forward * (maxDistance * 0.25f);
        currentDistance += Input.GetAxisRaw(moveAxis) * moveSpeed * Time.deltaTime;
        currentDistance = Mathf.Clamp(currentDistance, 0, maxDistance);
        transform.position = Vector3.MoveTowards(transform.position, trackedObject.position + Vector3.up * currentDistance - trackedObject.forward * (currentDistance + maxDistance * 0.5f), updateSpeed * Time.deltaTime);
        transform.LookAt(ahead.transform);
        _renderer.enabled = (currentDistance > hideDistance);
    }

    /*
    public float sensitivity = 200f;
    //float xRotation = 0f;
    //float yRotation = 0f;
    private Vector3 mousePos;

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

        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition.z = transform.position.z; // Keep the current Z position of the camera
        transform.position = Vector3.Lerp(transform.position, worldPosition, sensitivity * Time.deltaTime); // Smoothly move to the mouse position


        /* Redoing as this is buggy.
         * 
        // Vertical rotation (camera up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (rotate player left/right)
        //playerBody.Rotate(Vector3.up * mouseX);
        yRotation -= mouseX;
        yRotation = Mathf.Clamp(yRotation, -75f, 75f);
        transform.localRotation = Quaternion.Euler(-xRotation, yRotation, 0f); 
    } */
}