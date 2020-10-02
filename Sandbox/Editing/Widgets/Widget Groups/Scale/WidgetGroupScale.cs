using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WidgetGroupScale : WidgetGroup
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Widget WidgetScaleAxisX;
        [SerializeField]
        internal Widget WidgetScaleAxisY;
        [SerializeField]
        internal Widget WidgetScaleAxisZ;
        [SerializeField]
        internal Widget WidgetScaleFree;

        internal Module module;
    }
    [SerializeField]
    private Components _components;

    private void Awake()
    {
        BindTransformAction();
    }

    protected override void BindTransformAction()
    {
        _components.WidgetScaleAxisX.SetAction(OnTransformAction);
        _components.WidgetScaleAxisY.SetAction(OnTransformAction);
        _components.WidgetScaleAxisZ.SetAction(OnTransformAction);
        _components.WidgetScaleFree.SetAction(OnTransformAction);
    }

    protected override void OnTransformAction(object[] values)
    {
        Vector3 scale = (Vector3)values[0];
        _components.module.transform.localScale += scale;
        _components.module.OnTransformed();
    }

    internal override void SetModule(Module value)
    {
        base.gameObject.SetActive(true);
        _components.module = value;
        base.transform.rotation = _components.module.transform.rotation;
        base.transform.position = _components.module.transform.position;
    }

    internal override void Close()
    {
        base.gameObject.SetActive(false);
        _components.WidgetScaleAxisX.Release();
        _components.WidgetScaleAxisY.Release();
        _components.WidgetScaleAxisZ.Release();
        _components.WidgetScaleFree.Release();
    }
}