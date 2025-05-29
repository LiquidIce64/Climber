using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class KeyPressController : MonoBehaviour
    {
        [Serializable]
        public struct KeyPressAction
        {
            public KeyCode key;
            public UnityEvent action;
        }

        [SerializeField] private KeyPressAction[] actions;

        void Update()
        {
            foreach (var action in actions)
                if (Input.GetKey(action.key)) action.action.Invoke();
        }
    }
}
