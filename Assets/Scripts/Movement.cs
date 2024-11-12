using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Rigidbody rig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (checkForGround())
        {
            Move();
            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            Strafe();
        }
    }

    private void Move()
    {
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        Vector3 direction = Vector3.ClampMagnitude(transform.forward * vertical + transform.right * horizontal, 1) * speed * Time.fixedDeltaTime;

        rig.linearVelocity = new Vector3(direction.x, rig.linearVelocity.y, direction.z);
    }

    private void Strafe()
    {
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        Vector3 direction = Vector3.ClampMagnitude(transform.forward * vertical + transform.right * horizontal, 1) * strafeSpeed * Time.fixedDeltaTime;

        rig.AddForce(direction);
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
