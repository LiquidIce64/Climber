using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool jump;
    private float mouseX;
    private float mouseY;
    private bool mouse1;
    private float mouseWheel;

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

    public bool Mouse1
    {
        get
        {
            return mouse1;
        }
    }

    public float MouseWheel
    {
        get
        {
            return mouseWheel;
        }
    }
    

    public void UpdateValues()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        jump = Input.GetAxisRaw("Jump") > .1f;
        mouse1 = Input.GetMouseButtonDown(0);
    }

    public void FixedUpdateValues()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    
}
