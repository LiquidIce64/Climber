using Interactables;
using UnityEngine;

public class DebugToggleMaterial : MonoBehaviour, IToggleable
{
    protected bool _toggled = false;
    [SerializeField] protected GameObject connector;
    protected IConnector _connector = null;
    [SerializeField] protected Material onMaterial;
    [SerializeField] protected Material offMaterial;
    protected MeshRenderer meshRenderer;

    public bool IsEnabled { get { return _toggled; } }

    protected void OnValidate()
    {
        if (connector != null && !connector.TryGetComponent<IConnector>(out var _))
            Debug.LogError("Could not get connector component from object");
    }

    protected void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (connector != null)
        {
            connector.TryGetComponent(out _connector);
            _connector?.ToggleEvent.AddListener(ToggleEventHandler);
        }
    }

    protected void ToggleEventHandler()
    {
        if (_connector.IsEnabled) Enable();
        else Disable();
    }

    public void Enable()
    {
        _toggled = true;
        meshRenderer.material = onMaterial;
    }

    public void Disable()
    {
        _toggled = false;
        meshRenderer.material = offMaterial;
    }
}
