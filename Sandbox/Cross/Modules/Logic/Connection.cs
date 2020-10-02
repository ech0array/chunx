using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum ConnectionType
{
    Weld,
    Link
}

internal sealed class Connection : MonoBehaviour
{
    internal Editable parent;
    internal Module moduleA => _components.moduleA;
    internal Module moduleB => _components.moduleB;
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal LineRenderer lineRenderer;
        internal Module moduleA;
        internal Module moduleB;
    }
    [SerializeField]
    private Components _components;

    internal void Connect(Module moduleA, Module moduleB)
    {
        _components.moduleA = moduleA;
        _components.moduleB = moduleB;

        moduleA.AddConnection(this);
        moduleB.AddConnection(this);
        RefreshLine();
    }

    internal void SetColor(Color color)
    {
        if(GameHead.instance.isPreviewOrRuntime)
        {
            _components.lineRenderer.enabled = false;
            return;
        }
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0) };
        gradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(color.a, 1) };
        _components.lineRenderer.colorGradient = gradient;
    }

    internal void RefreshLine()
    {
        _components.lineRenderer.SetPosition(0, _components.moduleB.gameObject.transform.position);
        _components.lineRenderer.SetPosition(1, _components.moduleA.gameObject.transform.position);
    }

    internal void Break()
    {
        _components.moduleA.BreakConnection(this);
        _components.moduleB.BreakConnection(this);
        parent.Disconnect(this);
    }

    internal bool IsConnectedTo(Module module)
    {
        return _components.moduleA == module || _components.moduleB == module;
    }

    internal void Spoof(Vector3 vector3A, Vector3 vector3B)
    {
        _components.lineRenderer.SetPosition(1, vector3A);
        _components.lineRenderer.SetPosition(0, vector3B);
    }

    internal Module GetOtherTo(Module module)
    {
        if (module == moduleA)
            return moduleB;
        else
            return moduleA;
    }
}
