using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float maxAngle;
    [SerializeField] private float vertSens;
    [SerializeField] private float horizSens;
    [SerializeField] private GameObject cam;
    private float rot = 0f;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RotateCamera(float mouseX, float mouseY)
    {
        rot = Mathf.Clamp(rot + mouseY * vertSens, -maxAngle, maxAngle);
        cam.transform.localRotation = Quaternion.AngleAxis(rot, Vector3.left);
        transform.Rotate(transform.up, mouseX * horizSens);
    }
}
