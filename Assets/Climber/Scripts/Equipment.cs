using UnityEngine;

abstract public class Equipment : MonoBehaviour
{
    [SerializeField] protected GameObject raycastOrigin;

    abstract public void Use();
}
