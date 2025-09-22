using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity; // Tracks vertical speed due to jump and gravity

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Handle horizontal movement (example)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized * moveSpeed;

        if (horizontalInput == 0f)
        {
            moveDirection.x = 0;
        }

        if (verticalInput == 0f)
        {
            moveDirection.z = 0f;
        }

        // Apply gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime; // Small downward force when grounded
            if (Input.GetButtonDown("Jump")) // Check for jump input
            {
                verticalVelocity = jumpSpeed;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Apply gravity when airborne
        }

        // Combine vertical and horizontal movement
        moveDirection.y = verticalVelocity;

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
    }
}
