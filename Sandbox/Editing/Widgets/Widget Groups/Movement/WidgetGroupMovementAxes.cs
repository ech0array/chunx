using System;
using UnityEngine;

internal sealed class WidgetGroupMovementAxes : WidgetGroup
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Widget WidgetMovementAxisX;
        [SerializeField]
        internal Widget WidgetMovementAxisY;
        [SerializeField]
        internal Widget WidgetMovementAxisZ;

        [SerializeField]
        internal Widget WidgetMovementAxisXZ;
        [SerializeField]
        internal Widget WidgetMovementAxisYZ;
        [SerializeField]
        internal Widget WidgetMovementAxisYX;

        [SerializeField]
        internal Widget WidgetMovementAxisFree;

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
        _components.WidgetMovementAxisX.SetAction(OnTransformAction);
        _components.WidgetMovementAxisY.SetAction(OnTransformAction);
        _components.WidgetMovementAxisZ.SetAction(OnTransformAction);
        _components.WidgetMovementAxisXZ.SetAction(OnTransformAction);
        _components.WidgetMovementAxisYZ.SetAction(OnTransformAction);
        _components.WidgetMovementAxisYX.SetAction(OnTransformAction);
        _components.WidgetMovementAxisFree.SetAction(OnTransformAction);
    }

    protected override void OnTransformAction(object[] values)
    {
        Vector3 newPosition = (Vector3)values[0];
        base.transform.position = newPosition;
        _components.module.transform.position = newPosition;
        _components.module.OnTransformed();
    }

    internal override void SetModule(Module module)
    {
        base.gameObject.SetActive(true);
        _components.module = module;
        base.transform.rotation = _components.module.transform.rotation;
        base.transform.position = _components.module.transform.position;
    }

    internal override void Close()
    {
        base.gameObject.SetActive(false);
        _components.WidgetMovementAxisX.Release();
        _components.WidgetMovementAxisY.Release();
        _components.WidgetMovementAxisZ.Release();
        _components.WidgetMovementAxisXZ.Release();
        _components.WidgetMovementAxisYZ.Release();
        _components.WidgetMovementAxisYX.Release();
        _components.WidgetMovementAxisFree.Release();
    }
}