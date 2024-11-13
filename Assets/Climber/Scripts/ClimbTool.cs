using UnityEngine;

public class ClimbTool : Equipment
{
    [SerializeField] private float rayDist;
    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    override public void Use()
    {
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward, out RaycastHit hit, rayDist))
        {
            //check if ray hit a wall
            float a = Vector3.Angle(hit.normal, Vector3.up);
            if (45 < a && a < 135)
            {
                movement.Climb();
            }
        }
    }
}
