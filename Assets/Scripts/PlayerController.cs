using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float gravityValue = -3f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float jumpHeight = 2.0f;

    private bool jumping;

    private CharacterController Controller;
    private float xRotation = 0f;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    void Update()
    {
        // Player Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //groundedPlayer = Controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 2F; // Reset vertical velocity when grounded
            jumping = false;
            Debug.Log("DoneJumped");
        }

        // Handle Jump Input (using the new Input System example)
        // If using the legacy Input System, replace with: if (Input.GetButtonDown("Jump") && groundedPlayer)
        if (Input.GetButtonDown("Jump") && groundedPlayer && jumping == false)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            jumping = true;
            groundedPlayer = false;
            Debug.Log("Jumped");
            //Controller.Move(playerVelocity * Time.deltaTime); // Move the character
        }

        

        playerVelocity.y += gravityValue * Time.deltaTime; // Apply gravity
        Controller.Move(playerVelocity * Time.deltaTime); // Move the character


        Vector3 move = transform.right * x + transform.up * gravityValue + transform.forward * z;
        Controller.Move(move * moveSpeed * Time.deltaTime);

        // Camera Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnTriggerStay(Collider other)
    {
       // if (other.gameObject.layer == 10)
            groundedPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.layer == 10)
            groundedPlayer = false;
    }
}
