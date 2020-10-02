using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WidgetForwardDirection : MonoBehaviour
{
    private class Components
    {
        [SerializeField]
        internal Module module;
    }
    private Components _components = new Components();

    private void LateUpdate()
    {
        if (_components.module == null)
            return;

        UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        base.transform.position = _components.module.transform.position;
        base.transform.rotation = _components.module.transform.rotation;
    }

    internal void SetModule(Module module)
    {
        _components.module = module;
    }

    internal void Release()
    {
        _components.module = null;
    }
}
