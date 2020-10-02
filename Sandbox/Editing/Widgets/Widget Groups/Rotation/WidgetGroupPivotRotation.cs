using System;
using System.Collections.Generic;
using UnityEngine;

sealed internal class WidgetGroupPivotRotation : WidgetGroup
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

    private class Data
    {
        internal Vector3 pivot;
    }
    private Data _data = new Data();

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
        base.transform.RotateAround(_data.pivot, axis, -deltaAngle);

        float angle = (float)values[1];
        angle -= angle % 5;
        base.transform.RotateAround(_data.pivot, axis, angle);

        _components.module.transform.RotateAround(_data.pivot, axis, -deltaAngle);
        _components.module.transform.RotateAround(_data.pivot, axis, angle);

        _components.module.OnTransformed();
    }

    internal override void SetModule(Module value)
    {
    }
    internal void SetModule(Module value, Vector3 pivot)
    {
        base.gameObject.SetActive(true);
        _components.module = value;
        base.transform.rotation = _components.module.transform.rotation;
        base.transform.position = pivot;
        _data.pivot = pivot;
    }

    internal override void Close()
    {
        _components.WidgetRotationAxisX.Release();
        _components.WidgetRotationAxisY.Release();
        _components.WidgetRotationAxisZ.Release();
        base.gameObject.SetActive(false);
    }

}