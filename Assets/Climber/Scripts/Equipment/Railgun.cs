using UnityEngine;

namespace Equipment
{
    public class Railgun : BaseEquipment
    {
        override public void Use()
        {
            if (Time.time - lastUsed < cooldown) return;

            if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward, out RaycastHit hit))
            {
                Debug.DrawLine(raycastOrigin.transform.position, hit.point, new Color(255, 0, 0), 2);
            }
            else Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.forward);

            lastUsed = Time.time;
        }

    }
}
