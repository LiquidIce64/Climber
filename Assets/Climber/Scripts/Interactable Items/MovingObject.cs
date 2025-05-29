using UnityEngine;
using Utils;

namespace Interactables
{
    public class MovingObject : BaseToggleable
    {
        [SerializeField] protected Connector _reverseConnector;
        [SerializeField] protected float _speed = 1.0f;
        [SerializeField] protected bool _loop = false;
        [SerializeField][Tooltip("Reverse direction when bumping into ends of the path")]
            protected bool _pingPong = false;
        [SerializeField] protected bool _reversed = false;
        [SerializeField] protected Vector3[] _pathPoints = new Vector3[0];
        protected int _pathIndex = 0;
        protected Vector3 _target;
        protected Vector3 _velocity = Vector3.zero;
        protected AudioLoopController moveSound;

        public Vector3 Velocity => _velocity;

        protected void OnDrawGizmosSelected()
        {
            // Path uses local coords in editor, set drawing matrix to local
            if (!Application.isPlaying)
                Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLineStrip(_pathPoints, _loop);
        }

        [ContextMenu("Reverse")]
        protected void Reverse()
        {
            _reversed = !_reversed;
            NextTarget();
        }

        protected new void Awake()
        {
            base.Awake();
            if (_reverseConnector != null)
                _reverseConnector.ToggleEvent.AddListener(Reverse);
            moveSound = GetComponent<AudioLoopController>();

            Matrix4x4 mat = transform.localToWorldMatrix;
            for (int i = 0; i < _pathPoints.Length; i++)
                _pathPoints[i] = mat.MultiplyPoint(_pathPoints[i]);
            _target = _pathPoints[0];
        }

        protected void Update()
        {
            if (!_connector.Toggled) return;

            Vector3 vecToTarget = _target - transform.position;
            Vector3 step = _speed * Time.deltaTime * vecToTarget.normalized;

            // Clamp step to not overshoot target
            if (step.sqrMagnitude > vecToTarget.sqrMagnitude)
                step = vecToTarget;

            transform.position += step;
            _velocity = step / Time.deltaTime;

            if (transform.position == _target) UpdateTarget();

            if (step.sqrMagnitude > 0f) moveSound.Play();
            else moveSound.Stop();
        }

        protected void UpdateTarget()
        {
            if (_pingPong && !_loop && (_pathIndex == 0 || _pathIndex == _pathPoints.Length - 1))
                _reversed = !_reversed;
            NextTarget();
        }

        protected void NextTarget()
        {
            if (_reversed) _pathIndex--;
            else _pathIndex++;

            if (_loop) _pathIndex = (_pathIndex + _pathPoints.Length) % _pathPoints.Length;
            else _pathIndex = Mathf.Clamp(_pathIndex, 0, _pathPoints.Length - 1);

            _target = _pathPoints[_pathIndex];
        }

        protected override void Disabled()
        {
            _velocity = Vector3.zero;
            moveSound.Stop();
        }
    }
}
