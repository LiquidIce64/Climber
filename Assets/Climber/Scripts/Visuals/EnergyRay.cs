using UnityEngine;

namespace Visuals
{
    public class EnergyRay : MonoBehaviour
    {
        public float duration;
        public float start_width;
        private float start_time;
        private LineRenderer lineRenderer;

        protected void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            start_time = Time.time;
        }

        protected void Update()
        {
            float t = (Time.time - start_time) / duration;
            if (t >= 1f) Destroy(gameObject);
            lineRenderer.widthMultiplier = start_width * (1f - (t * t) * 0.9f);
        }
    }

}
