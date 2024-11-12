using UnityEngine;

public class Player : MonoBehaviour
{
    private Movement movement;
    private CameraRotation cameraRotation;
    private PlayerInput playerInput;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        cameraRotation = GetComponent<CameraRotation>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        playerInput.UpdateValues();

        cameraRotation.RotateCamera(playerInput.MouseX, playerInput.MouseY);
    }

    private void FixedUpdate()
    {
        movement.Move(playerInput.Horizontal, playerInput.Vertical);
        if (playerInput.Jump)
        {
            movement.Jump();
        }
    }
}
