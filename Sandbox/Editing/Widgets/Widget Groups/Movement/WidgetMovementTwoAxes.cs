using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WidgetMovementTwoAxes : Widget
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
            return false;

        _data.startingPosition = ProjectPointOnPlane(base.transform.position, base.transform.forward, ray);
        _data.cursorOffset = _data.startingPosition - (base.transform.position - (base.transform.parent.rotation * base.transform.localPosition));

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

        Vector3 newPosition = ProjectPointOnPlane(base.transform.position - _data.cursorOffset, base.transform.forward, ray);
        newPosition -= _data.cursorOffset;
        newPosition.x = (float)Math.Round(newPosition.x, 1);
        newPosition.y = (float)Math.Round(newPosition.y, 1);
        newPosition.z = (float)Math.Round(newPosition.z, 1);
        _data.action.Invoke(new object[] {newPosition});
    }
    internal override void SetAction(Action<object[]> value)
    {
        _data.action = value;
    }
}