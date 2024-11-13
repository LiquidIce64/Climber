using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float climbVelocity;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float groundCheckDistance;
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

    public void Climb()
    {
        rig.linearVelocity = new Vector3(rig.linearVelocity.x, climbVelocity, rig.linearVelocity.z);
    }

    private bool CheckForGround()
    {
        if (Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            isGrounded = Vector3.Angle(hit.normal, Vector3.up) < 50;
        }
        else { isGrounded = false; }
        return isGrounded;
    }
}
