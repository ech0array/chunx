using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed internal class WidgetScaleAxis : Widget
{
    [Serializable]
    private sealed class Data : BaseData
    {
        internal override bool interacting { get; set; }
        internal override Vector3 startingPosition { get; set; }
        internal override Action<object[]> action { get; set; }
        internal Vector3 widgetStartLocalPosition;
        [SerializeField]
        internal Vector3 direction;
    }
    [SerializeField]
    private Data _data;

    private void Awake()
    {
        _data.widgetStartLocalPosition = base.transform.localPosition;
    }

    internal override bool Down(Ray ray, Vector3 hitPoint)
    {
        _data.interacting = !_data.interacting;
        if (!_data.interacting)
        {
            base.transform.localPosition = _data.widgetStartLocalPosition;
            goto ABORT;
        }

        _data.startingPosition = ClosestProjectionOnLine(base.transform.position, base.transform.forward, ray.origin, ray.direction);

        ABORT:
        return _data.interacting;
    }
    internal override void Release()
    {
        _data.interacting = false;
    }

    internal override void TryUpdate(Ray ray)
    {
        if (!_data.interacting)
            return;
        
        Vector3 intersection = ClosestProjectionOnLine(base.transform.position, base.transform.forward, ray.origin, ray.direction);
        base.transform.position = intersection;
        float distance = Vector3.Distance(intersection, _data.startingPosition);
        float direction = (base.transform.TransformVector(_data.startingPosition) - base.transform.TransformVector(intersection)).normalized.z;
        _data.startingPosition = intersection;

        Vector3 scale = _data.direction * direction * distance;

        _data.action.Invoke(new object[] { scale * 2f });
    }
    internal override void SetAction(Action<object[]> value)
    {
        _data.action = value;
    }
}