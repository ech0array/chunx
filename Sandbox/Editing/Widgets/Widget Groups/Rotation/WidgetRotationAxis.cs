using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WidgetRotationAxis : Widget
{
    [Serializable]
    private sealed class Data : BaseData
    {
        internal override bool interacting { get; set; }
        internal override Vector3 startingPosition { get; set; }
        internal override Action<object[]> action { get; set; }

        internal Vector3 startForward;
        internal Vector3 startDirection;
        internal float deltaAngle;
    }
    [SerializeField]
    private Data _data;

    internal override bool Down(Ray ray, Vector3 hitPoint)
    {
        _data.interacting = !_data.interacting;
        if (_data.interacting)
        {
            _data.startForward = base.transform.forward;
            Vector3 start = ProjectPointOnPlane(base.transform.position, base.transform.forward, ray);
            _data.startDirection = (start - base.transform.parent.position).normalized;
            _data.startingPosition = base.transform.position + _data.startDirection;
            _data.deltaAngle = 0;
        }
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
        Vector3 intersection = ProjectPointOnPlane(base.transform.position, _data.startForward, ray);
        Vector3 direction = intersection - base.transform.position;

        float angle = Vector3.SignedAngle(_data.startDirection, direction, _data.startForward);

        _data.action.Invoke(new object[] { _data.startForward, angle, _data.deltaAngle });
        _data.deltaAngle = angle;
    }
    internal override void SetAction(Action<object[]> value)
    {
        _data.action = value;
    }
}