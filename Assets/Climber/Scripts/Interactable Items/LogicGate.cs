using UnityEngine;

namespace Interactables
{
    public class LogicGate : MonoBehaviour
    {
        protected enum LogicOperator { OR, AND, NOR, NAND }

        [SerializeField] protected LogicOperator _operator = LogicOperator.OR;
        [SerializeField] protected Connector[] _connectorsIn;
        [SerializeField] protected Connector _connectorOut;

        protected void OnValidate()
        {
            foreach (var connector in _connectorsIn)
                if (connector == null)
                {
                    Debug.LogWarning("Logic gate contains an unassigned input connector field");
                }
        }

        protected void Awake()
        {
            foreach (var connector in _connectorsIn)
                connector.ToggleEvent.AddListener(ToggleEventHandler);
        }

        protected bool OR()
        {
            foreach (var connector in _connectorsIn)
                if (connector.Toggled) return true;
            return false;
        }

        protected bool AND()
        {
            foreach (var connector in _connectorsIn)
                if (!connector.Toggled) return false;
            return true;
        }

        protected void ToggleEventHandler()
        {
            bool result = false;
            switch (_operator)
            {
                case LogicOperator.OR: result = OR(); break;
                case LogicOperator.AND: result = AND(); break;
                case LogicOperator.NOR: result = !OR(); break;
                case LogicOperator.NAND: result = !AND(); break;
            }
            if (result) _connectorOut.Enable();
            else _connectorOut.Disable();
        }
    }
}