using UnityEngine;

public class Player : MonoBehaviour
{
    private Movement movement;
    private CameraRotation cameraRotation;
    private PlayerInput playerInput;
    [SerializeField] private GameObject cam;
    private ClimbTool climbTool;
    private Railgun railgun;
    private Equipment equipped;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        cameraRotation = GetComponent<CameraRotation>();
        playerInput = GetComponent<PlayerInput>();
        climbTool = GetComponent<ClimbTool>();
        railgun = GetComponent<Railgun>();
        equipped = climbTool;
    }

    private void Update()
    {
        playerInput.UpdateValues();

        cameraRotation.RotateCamera(playerInput.MouseX, playerInput.MouseY);

        if (playerInput.MouseWheel > 0)
        {
            equipped = climbTool;
            Debug.Log("equipped climbtool");
        }
        else if (playerInput.MouseWheel < 0)
        {
            equipped = railgun;
            Debug.Log("equipped railgun");
        }

        if (playerInput.Mouse1)
        {
            equipped.Use();
        }
    }

    private void FixedUpdate()
    {
        playerInput.FixedUpdateValues();
        movement.Move(playerInput.Horizontal, playerInput.Vertical);
        if (playerInput.Jump)
        {
            movement.Jump();
        }
    }
}
