using UnityEngine;

public class ClimbTool : Equipment
{
    public float rayDist = 2f;
    [Range(60f, 90f)] public float minWallAngle = 75f;

    private Movement.Player player;

    private void Awake()
    {
        player = GetComponent<Movement.Player>();
    }
    
    override public void Use()
    {
        // If already moving upwards fast, no reason to climb
        if (player.moveData.velocity.y > player.moveConfig.climbVelocity) return;

        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward, out RaycastHit hit, rayDist))
        {
            // Check if ray hit a wall
            float a = Vector3.Angle(hit.normal, Vector3.up);
            if (minWallAngle <= a && a <= 180f - minWallAngle)
                player.moveData.desiredClimb = true;
        }
    }
}
