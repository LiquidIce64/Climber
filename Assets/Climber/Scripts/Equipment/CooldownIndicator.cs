using UnityEngine;

namespace Equipment
{
    public class CooldownIndicator : MonoBehaviour
    {
        [SerializeField] private BaseEquipment equipment;

        void Update()
        {
            transform.localScale = new Vector3(1f, 1f, 1f - equipment.CooldownFraction);
        }
    }
}
