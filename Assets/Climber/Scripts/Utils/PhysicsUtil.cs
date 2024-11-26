using UnityEngine;


public static class PhysicsUtil
{
    public struct Trace
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public float fraction;
        public bool startSolid;
        public Collider hitCollider;
        public Vector3 hitPoint;
        public Vector3 planeNormal;
        public float distance;
    }

    public static void GetCapsulePoints(CapsuleCollider capc, Vector3 origin, out Vector3 p1, out Vector3 p2)
    {
        var distanceToPoints = capc.height / 2f;
        p1 = origin + capc.center + Vector3.up * distanceToPoints;
        p2 = origin + capc.center - Vector3.up * distanceToPoints;
    }

    public static Trace TraceCollider(
        Collider collider,
        Vector3 origin,
        Vector3 end,
        int layerMask,
        float colliderScale = 1f)
    {

        if (collider is BoxCollider)
        {
            return TraceBox(origin, end, collider.bounds.extents,
                collider.contactOffset, layerMask, colliderScale);
        }
        if (collider is CapsuleCollider)
        {
            var capc = (CapsuleCollider)collider;
            GetCapsulePoints(capc, origin, out Vector3 point1, out Vector3 point2);

            return TraceCapsule(point1, point2, capc.radius, origin,
                end, capc.contactOffset, layerMask, colliderScale);
        }

        throw new System.NotImplementedException("Trace missing for collider: " + collider.GetType());

    }

    public static Trace TraceCapsule(
        Vector3 point1,
        Vector3 point2,
        float radius,
        Vector3 start,
        Vector3 destination,
        float contactOffset,
        int layerMask,
        float colliderScale = 1f)
    {
        var result = new Trace()
        {
            startPos = start,
            endPos = destination
        };

        var longSide = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
        radius *= (1f - contactOffset);
        var direction = (destination - start).normalized;
        var maxDistance = Vector3.Distance(start, destination) + longSide;

        if (Physics.CapsuleCast(
            point1: point1 - 0.5f * colliderScale * Vector3.up,
            point2: point2 + 0.5f * colliderScale * Vector3.up,
            radius: radius * colliderScale,
            direction: direction,
            hitInfo: out RaycastHit hit,
            maxDistance: maxDistance,
            layerMask: layerMask,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore))
        {
            result.fraction = hit.distance / maxDistance;
            result.hitCollider = hit.collider;
            result.hitPoint = hit.point;
            result.planeNormal = hit.normal;
            result.distance = hit.distance;

            if (Physics.Raycast(result.hitPoint - direction * 0.01f, direction, out hit, 0.02f, layerMask))
                result.planeNormal = hit.normal;
        }
        else result.fraction = 1;

        return result;

    }

    public static Trace TraceBox(
        Vector3 start,
        Vector3 destination,
        Vector3 extents,
        float contactOffset,
        int layerMask,
        float colliderScale = 1f)
    {
        var result = new Trace()
        {
            startPos = start,
            endPos = destination
        };

        var longSide = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
        var direction = (destination - start).normalized;
        var maxDistance = Vector3.Distance(start, destination) + longSide;
        extents *= (1f - contactOffset);

        if (Physics.BoxCast(center: start,
            halfExtents: extents * colliderScale,
            direction: direction,
            orientation: Quaternion.identity,
            maxDistance: maxDistance,
            hitInfo: out RaycastHit hit,
            layerMask: layerMask,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore))
        {
            result.fraction = hit.distance / maxDistance;
            result.hitCollider = hit.collider;
            result.hitPoint = hit.point;
            result.planeNormal = hit.normal;
            result.distance = hit.distance;
        }
        else result.fraction = 1;

        return result;

    }

}
