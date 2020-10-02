using System;
using UnityEngine;

internal sealed class WidgetScaleFree : Widget
{
    [Serializable]
    private sealed class Data : BaseData
    {
        internal override bool interacting { get; set; }
        internal override Vector3 startingPosition { get; set; }
        internal override Action<object[]> action { get; set; }
        internal float baseDistance;
        internal Vector3 hitPoint;
    }
    [SerializeField]
    private Data _data;

    internal override bool Down(Ray ray, Vector3 hitPoint)
    {
        _data.interacting = !_data.interacting;
        if (!_data.interacting)
            return false;
        _data.hitPoint = hitPoint;
        _data.baseDistance = Vector3.Distance(ray.origin, hitPoint);

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
        float distance = Vector3.Distance(ray.origin, _data.hitPoint);
        float delta = distance - _data.baseDistance;
        _data.baseDistance = distance;
        Vector3 scale = Vector3.one * delta;

        _data.action.Invoke(new object[] { scale });
    }
    internal override void SetAction(Action<object[]> value)
    {
        _data.action = value;
    }
}