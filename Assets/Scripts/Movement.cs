using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Rigidbody rig;

    private void FixedUpdate()
    {
        checkForGround();
        Move();
        if (isGrounded && Input.GetAxis("Jump") > .1f)
        {
            Jump();
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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

    private void Jump()
    {
        rig.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
    }

    private bool checkForGround()
    {
        isGrounded = Physics.OverlapSphere(groundCheck.transform.position, groundCheckRadius, LayerMask.GetMask("Ground")).Length > 0;
        return isGrounded;
    }
}
