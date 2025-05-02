using Character;
using Interactables;
using Visuals;
using UnityEngine;
using Utils;

namespace Equipment
{
    public class ClimbTool : BaseEquipment
    {
        public float rayDist = 2f;
        [Range(60f, 90f)] public float minWallAngle = 75f;
        [SerializeField] protected EnergyColor _color = EnergyColor.Blue;
        [SerializeField] protected EnergyMaterial[] energyMaterials;
        [SerializeField] protected Transform[] rayOrigins;
        [SerializeField] protected GameObject energyRay;
        [SerializeField] protected GameObject sparkParticles;
        [SerializeField] protected MaterialArray particleMaterials;

        protected void OnValidate()
        {
            foreach (var mat in energyMaterials)
                mat.UpdateMaterialInEditor(_color);
        }

        protected new void Awake()
        {
            base.Awake();
            player = (Player)character;
        }

        public void SetColor(EnergyColor color)
        {
            _color = color;
            foreach (var mat in energyMaterials)
                mat.UpdateMaterial(_color);
        }

        protected void OnHit(RaycastHit hit)
        {
            var material = EnergyMaterial.GetMaterial(particleMaterials, _color);

            // Create rays
            foreach (Transform origin in rayOrigins)
            {
                Vector3 rayVec = hit.point - origin.position;
                var ray = Instantiate(energyRay);
                ray.layer = gameObject.layer;
                ray.transform.SetPositionAndRotation(origin.position, Quaternion.LookRotation(rayVec.normalized));
                ray.transform.localScale = new Vector3(1f, 1f, rayVec.magnitude);
                var rayComp = ray.GetComponent<EnergyRay>();
                rayComp.start_width *= origin.localScale.x;
                ray.GetComponent<LineRenderer>().material = material;
            }

            var particles = Instantiate(sparkParticles, hit.point, Quaternion.LookRotation(hit.normal));
            particles.layer = gameObject.layer;
            particles.GetComponent<ParticleSystemRenderer>().material = material;

            audioSource.Play();

            lastUsed = Time.time;
        }

        override public void Use()
        {
            if (Time.time - lastUsed < cooldown) return;

            // Skip if nothing was hit
            if (!Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hit, rayDist)) return;

            GameObject obj = hit.collider.gameObject;
            // If hit a linked collider, use its parent object instead
            if (obj.TryGetComponent(out LinkedCollider collider))
                obj = collider.ParentObject;

            // Hit interactable item
            if (obj.TryGetComponent(out IInteractable hitInteractable))
            {
                if (!hitInteractable.CanInteract) return;
                if (hitInteractable.EnergyCost > 0f && player.TakeEnergy(hitInteractable.EnergyCost) == 0f) return;
                
                hitInteractable.OnInteract();
                OnHit(hit);
                return;
            }
            
            // Hit climbable wall
            if (obj.TryGetComponent(out ClimbableWall hitWall))
            {
                if (!hitWall.Toggled) return;
                if (hitWall.Color != _color) return;

                // If already moving upwards fast, no reason to climb
                if (character.moveData.velocity.y >= character.moveConfig.climbVelocity) return;

                if (player.TakeEnergy(energyConsumption) == 0f) return;

                character.moveData.desiredClimb = true;
                OnHit(hit);
                return;
            }
        }

    }
}
