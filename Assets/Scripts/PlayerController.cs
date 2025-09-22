using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float gravity = -3f;
    private Vector3 playerVelocity;
    private Vector3 moveDirection;
    private bool groundedPlayer;
    public float jumpSpeed = 2.0f;

    private bool jumping;
    private float verticalVelocity;

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

        // Apply gravity
        if (Controller.isGrounded)
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
        Controller.Move(moveDirection * Time.deltaTime);


        // Handle Jump Input (using the new Input System example)
        // If using the legacy Input System, replace with: if (Input.GetButtonDown("Jump") && groundedPlayer)
        if (Input.GetButtonDown("Jump") && groundedPlayer && jumping == false)
        {
            playerVelocity.y += Mathf.Sqrt(jumpSpeed * -2.0f * gravity);
            jumping = true;
            groundedPlayer = false;
            Debug.Log("Jumped");
            //Controller.Move(playerVelocity * Time.deltaTime); // Move the character
        }


        Vector3 move = transform.right * x + transform.up * gravity + transform.forward * z;
        Controller.Move(move * moveSpeed * Time.deltaTime);

        // Camera Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

}
