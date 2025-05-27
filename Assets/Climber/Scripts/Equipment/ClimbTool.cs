using Character;
using Interactables;
using Visuals;
using UnityEngine;
using Utils;
using System;

namespace Equipment
{
    public class ClimbTool : BaseEquipment
    {
        [Serializable]
        protected struct RayOrigin {
            public Transform transform;
            public bool useColor;
        }

        public float rayDist = 2f;
        [Range(60f, 90f)] public float minWallAngle = 75f;
        [SerializeField] protected EnergyColor _color = EnergyColor.Blue;
        [SerializeField] protected EnergyMaterial[] energyMaterials;
        [SerializeField] protected RayOrigin[] rayOrigins;
        [SerializeField] protected GameObject energyRay;
        [SerializeField] protected GameObject sparkParticles;
        [SerializeField] protected MaterialArray particleMaterials;
        protected AudioSource reloadSound;
        [SerializeField] protected float reloadSoundOffset = 1f;

        protected void OnValidate()
        {
            foreach (var mat in energyMaterials)
                mat.UpdateMaterialInEditor(_color);
        }

        protected new void Awake()
        {
            base.Awake();
            player = (Player)character;
            reloadSound = GetComponent<AudioSource>();
        }

        public void SetColor(EnergyColor color)
        {
            _color = color;
            foreach (var mat in energyMaterials)
                mat.UpdateMaterial(_color);
        }

        protected void OnHit(RaycastHit hit, bool useColor = false)
        {
            var material = EnergyMaterial.GetMaterial(particleMaterials, _color);

            // Create rays
            foreach (RayOrigin origin in rayOrigins)
            {
                Vector3 rayVec = hit.point - origin.transform.position;

                var ray = Instantiate(energyRay);
                ray.layer = gameObject.layer;
                ray.transform.SetPositionAndRotation(origin.transform.position, Quaternion.LookRotation(rayVec.normalized));
                ray.transform.localScale = new Vector3(1f, 1f, rayVec.magnitude);

                var rayComp = ray.GetComponent<EnergyRay>();
                rayComp.start_width *= origin.transform.localScale.x;

                if (useColor && origin.useColor)
                    ray.GetComponent<LineRenderer>().material = material;
            }

            var particles = Instantiate(sparkParticles, hit.point, Quaternion.LookRotation(hit.normal));
            particles.layer = gameObject.layer;

            if (useColor)
                particles.GetComponent<ParticleSystemRenderer>().material = material;

            audioSource.Play();

            if (cooldown > 0f)
                reloadSound.PlayDelayed(cooldown - reloadSoundOffset);

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
                OnHit(hit, true);
                return;
            }
        }

    }
}
