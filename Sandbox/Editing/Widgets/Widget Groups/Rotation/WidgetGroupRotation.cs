using System;
using System.Collections.Generic;
using UnityEngine;

sealed internal class WidgetGroupRotation : WidgetGroup
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Widget WidgetRotationAxisX;
        [SerializeField]
        internal Widget WidgetRotationAxisY;
        [SerializeField]
        internal Widget WidgetRotationAxisZ;

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
        _components.WidgetRotationAxisX.SetAction(OnTransformAction);
        _components.WidgetRotationAxisY.SetAction(OnTransformAction);
        _components.WidgetRotationAxisZ.SetAction(OnTransformAction);
    }
    protected override void OnTransformAction(object[] values)
    {
        Vector3 axis = (Vector3)values[0];

        float deltaAngle = (float)values[2];
        deltaAngle -= deltaAngle % 5;
        base.transform.Rotate(axis, -deltaAngle, Space.World);

        float angle = (float)values[1];
        angle -= angle % 5;
        base.transform.Rotate(axis, angle, Space.World);

        _components.module.transform.rotation = base.transform.rotation;
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
        _components.WidgetRotationAxisX.Release();
        _components.WidgetRotationAxisY.Release();
        _components.WidgetRotationAxisZ.Release();
        base.gameObject.SetActive(false);
    }
}