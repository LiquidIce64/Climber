using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float groundCheckRadius;
    private bool isGrounded;
    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckForGround();
    }
    
    public void Move(float horizontal, float vertical)
    {
        Vector3 direction = Vector3.ClampMagnitude(transform.forward * vertical + transform.right * horizontal, 1) * speed * Time.fixedDeltaTime;
        if (isGrounded)
        {
            rig.linearVelocity = new Vector3(direction.x, rig.linearVelocity.y, direction.z);
        }
        else
        {
            rig.AddForce(direction);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rig.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
        }
    }

    private bool CheckForGround()
    {
        isGrounded = Physics.OverlapSphere(groundCheck.transform.position, groundCheckRadius, LayerMask.GetMask("Ground")).Length > 0;
        return isGrounded;
    }
}
