using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 12f;
    public float jumpSpeed = 12f;
    public float gravity = 40f;
    public float fallMultiplier = 2.5f;

    [Header("Wall Running")]
    public float wallRunSpeed = 15f;
    public float wallRunGravity = 5f;
    public float wallJumpForce = 10f;
    public float wallCheckDistance = 1f;
    public float maxWallRunTime = 2f;
    public LayerMask wallMask;

    [Header("Wall Run Conditions")]
    public float minJumpHeight = 1.5f;
    public float minWallRunSpeed = 5f;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float cameraTilt = 15f;

    // Private state
    private CharacterController controller;
    private Camera playerCamera;

    private Vector3 moveDirection;
    private float verticalVelocity;

    private float xRotation = 0f;
    private float currentTilt = 0f;

    private bool isWallRunning = false;
    private bool isWallRight = false;
    private bool isWallLeft = false;
    private float wallRunTimer = 0f;

    // --------------------------------------------------

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Wall detection
        CheckForWall();

        // Try wall run first
        HandleWallRun();

        // If not wall running, handle normal movement
        if (!isWallRunning)
        {
            HandleMovement();
        }
        else
        {
            controller.Move(moveDirection * Time.deltaTime);
        }

        HandleMouseLook();
        HandleCameraTilt();
    }

    // --------------------------------------------------
    // MOVEMENT
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        moveDirection = movement * moveSpeed;

        // Ground check & jump
        if (controller.isGrounded)
        {
            verticalVelocity = -2f; // small downward force to keep grounded
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }
        else
        {
            // Apply gravity
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
        controller.Move(moveDirection * Time.deltaTime);
    }

    // --------------------------------------------------
    // WALL RUN
    private void HandleWallRun()
    {
        if (CanWallRun() && (isWallLeft || isWallRight))
        {
            if (!isWallRunning)
            {
                isWallRunning = true;
                wallRunTimer = maxWallRunTime;
            }

            wallRunTimer -= Time.deltaTime;
            if (wallRunTimer <= 0f)
            {
                isWallRunning = false;
                return;
            }

            // Apply reduced gravity
            verticalVelocity = -wallRunGravity;

            // Forward momentum
            Vector3 forwardMove = transform.forward * wallRunSpeed;
            moveDirection = forwardMove;
            moveDirection.y = verticalVelocity;

            // Wall jump
            if (Input.GetButtonDown("Jump"))
            {
                Vector3 wallJumpDir = transform.up + (isWallRight ? -transform.right : transform.right);
                moveDirection = wallJumpDir * wallJumpForce + transform.forward * wallRunSpeed;
                verticalVelocity = wallJumpForce;
                isWallRunning = false;
            }
        }
        else
        {
            isWallRunning = false;
        }
    }

    private bool CanWallRun()
    {
        // Not grounded + moving upward enough
        if (controller.isGrounded) return false;
        if (Input.GetAxisRaw("Vertical") <= 0) return false;

        // Ensure player is at least above a "jump height" above ground
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
    }

    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, transform.right, wallCheckDistance, wallMask);
        isWallLeft = Physics.Raycast(transform.position, -transform.right, wallCheckDistance, wallMask);
    }

    // --------------------------------------------------
    // CAMERA
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Pitch
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply camera pitch + tilt
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);

        // Yaw
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleCameraTilt()
    {
        if (isWallRunning)
        {
            if (isWallRight)
                currentTilt = Mathf.Lerp(currentTilt, cameraTilt, Time.deltaTime * 5f);
            else if (isWallLeft)
                currentTilt = Mathf.Lerp(currentTilt, -cameraTilt, Time.deltaTime * 5f);
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * 5f);
        }

        // Final tilt applied in HandleMouseLook
    }
}