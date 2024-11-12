using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool jump;
    private float mouseX;
    private float mouseY;

    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }

    public float Vertical
    {
        get
        {
            return vertical;
        }
    }

    public bool Jump
    {
        get
        {
            return jump;
        }
    }

    public float MouseX
    {
        get
        {
            return mouseX;
        }
    }

    public float MouseY
    {
        get
        {
            return mouseY;
        }
    }

    public void UpdateValues()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetAxis("Jump") > .1f;
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
    }
}
