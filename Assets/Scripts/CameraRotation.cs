using Unity.Mathematics;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float maxAngle;
    [SerializeField] private float vertSens;
    [SerializeField] private float horizSens;
    [SerializeField] private GameObject cam;
    private float rot = 0f;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        rot = Mathf.Clamp(rot + mouseY * vertSens, -maxAngle, maxAngle);
        cam.transform.localRotation = Quaternion.AngleAxis(rot, Vector3.left);
        transform.Rotate(transform.up, mouseX * horizSens);
    }
}
