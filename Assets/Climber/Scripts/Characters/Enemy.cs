using Movement;
using Equipment;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Railgun))]
    public class Enemy : BaseCharacter
    {
        private Railgun railgun;

        private new void Start()
        {
            railgun = GetComponent<Railgun>();

            base.Start();
        }

    }
}
