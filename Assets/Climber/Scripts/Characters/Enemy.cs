using Movement;
using Equipment;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : BaseCharacter
    {
        [SerializeField] private GameObject path;
        [SerializeField] private float patrolSpeed;
        [SerializeField] private float maxChaseDistance;
        [SerializeField] private float stoppingDistance;
        private float chaseSpeed;
        private float chaseTimeout;
        private bool chasing = false;
        private Vector3[] pathPoints;
        private int pathPointInd = -1;
        private GameObject player;
        private NavMeshAgent nav;
        [SerializeField] private Railgun railgun;

        protected new void Start()
        {
            chaseSpeed = movementConfig.walkSpeed;
            movementConfig.walkSpeed = patrolSpeed;

            // Get path point positions
            pathPoints = new Vector3[path.transform.childCount];
            for (int i = 0; i < path.transform.childCount; i++)
                pathPoints[i] = path.transform.GetChild(i).position;

            player = GameObject.FindGameObjectWithTag("Player");
            nav = GetComponent<NavMeshAgent>();
            nav.updateUpAxis = false;

            base.Start();
        }

        protected new void Update()
        {
            float distanceToPlayer = (player.transform.position - viewObject.transform.position).magnitude;
            Vector3 directionToPlayer = (player.transform.position - viewObject.transform.position).normalized;
            if (distanceToPlayer <= maxChaseDistance &&
                Physics.Raycast(
                    viewObject.transform.position,
                    directionToPlayer,
                    out var hit,
                    maxChaseDistance,
                    LayerMask.GetMask("Default", "Characters")
                ) && hit.collider.gameObject == player)
            { // Has direct line of sight of player
                chasing = true;
                movementConfig.walkSpeed = chaseSpeed;
                nav.destination = player.transform.position;
                chaseTimeout = Time.time + 5f;
                Vector3 horizDir = directionToPlayer;
                horizDir.y = 0;
                horizDir.Normalize();
                transform.rotation = Quaternion.LookRotation(horizDir);
                viewAngle = Vector3.SignedAngle(directionToPlayer, transform.forward, transform.right);
                railgun.Use();
            }
            else if (chasing)
            { // Lost sight but still chasing
                if (Time.time >= chaseTimeout)
                {
                    chasing = false;
                    movementConfig.walkSpeed = patrolSpeed;
                    nav.ResetPath();
                    viewAngle = 0f;
                }
                else nav.destination = player.transform.position;
            }

            if (!nav.hasPath && !nav.pathPending)
            {
                movementConfig.walkSpeed = patrolSpeed;
                if (++pathPointInd == pathPoints.Length) { pathPointInd = 0; }
                nav.destination = pathPoints[pathPointInd];
            }

            if (chasing && distanceToPlayer <= stoppingDistance)
            {
                _moveData.horizontalAxis = 0f;
                _moveData.verticalAxis = 0f;
            }
            else
            {
                Vector3 desiredDirection = Vector3.ClampMagnitude(nav.desiredVelocity, 1);
                _moveData.horizontalAxis = Vector3.Dot(transform.right, desiredDirection);
                _moveData.verticalAxis = Vector3.Dot(transform.forward, desiredDirection);
            }

            base.Update();

            nav.velocity = _moveData.velocity;
            nav.nextPosition = _moveData.origin;
        }

    }
}
