using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WidgetMovementAxis : Widget
{
    [Serializable]
    private sealed class Data : BaseData
    {
        internal override bool interacting { get; set; }
        internal override Vector3 startingPosition { get; set; }
        internal override Action<object[]> action { get; set; }
        internal Vector3 cursorOffset;
    }
    [SerializeField]
    private Data _data;

    internal override bool Down(Ray ray, Vector3 hitPoint)
    {
        _data.interacting = !_data.interacting;
        if (!_data.interacting)
            goto COMPLETE;
        _data.startingPosition = ClosestProjectionOnLine(base.transform.position, base.transform.forward, ray.origin, ray.direction);
        _data.cursorOffset = _data.startingPosition - (base.transform.position - (base.transform.parent.rotation * base.transform.localPosition));
        COMPLETE:
        return _data.interacting;
    }
    internal override void Release()
    {
        _data.interacting = false;
    }
    internal override void Hover()
    {
        if (_data.interacting)
            return;
    }
    internal override void Exit()
    {
        if (_data.interacting)
            return;
    }

    internal override void TryUpdate(Ray ray)
    {
        if (!_data.interacting)
            return;
        Vector3 intersection = ClosestProjectionOnLine(_data.startingPosition, base.transform.forward, ray.origin, ray.direction);
        intersection -= _data.cursorOffset;
        intersection.x = (float)Math.Round(intersection.x, 1);
        intersection.y = (float)Math.Round(intersection.y, 1);
        intersection.z = (float)Math.Round(intersection.z, 1);
        _data.action.Invoke(new object[] {intersection});
    }
    internal override void SetAction(Action<object[]> value)
    {
        _data.action = value;
    }
}