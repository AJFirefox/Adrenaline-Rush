using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float jumpSpeed = 12f;
    public float gravity = 40f;
    public float mouseSensitivity = 100f;
    public float fallMultiplier = 2.5f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity;

    private float xRotation = 0f;
    private Camera playerCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        moveDirection = movement * moveSpeed;

        // Apply gravity (account for grounded state)
        if (controller.isGrounded)
        {
            verticalVelocity = -2f; // small constant downward force to stay grounded
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // gravity applied once per frame
        }

        // Apply vertical velocity (for jumping/falling)
        moveDirection.y = verticalVelocity;

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

       // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Handle vertical camera look
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Handle horizontal player rotation
        transform.Rotate(Vector3.up * mouseX);
    }
}