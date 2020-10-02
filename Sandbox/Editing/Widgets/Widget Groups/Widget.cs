using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Widget : MonoBehaviour
{
    [Serializable]
    protected abstract class BaseData
    {
        internal abstract bool interacting { get; set; }
        internal abstract Vector3 startingPosition { get; set; }
        internal abstract Action<object[]> action { get; set; }
    }

    internal virtual bool Down(Ray ray, Vector3 hitPoint) { return false; }
    internal virtual void Release()
    {
    }
    internal virtual void Hover() { }
    internal virtual void Exit() { }

    internal virtual void TryUpdate(Ray ray) { }
    internal virtual void SetAction(Action<object[]> value)
    {
    }


    protected static Vector3 ProjectPointOnPlane(Vector3 planePosition, Vector3 planeNormal, Ray ray)
    {
        Plane plane = new Plane(planeNormal, planePosition);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    protected static Vector3 ClosestProjectionOnLine(Vector3 linePosition, Vector3 lineDirection, Vector3 projectionPosition, Vector3 projectionDirection)
    {
        Vector3 closestPointLine1 = Vector3.zero;

        float a = Vector3.Dot(lineDirection, lineDirection);
        float b = Vector3.Dot(lineDirection, projectionDirection);
        float e = Vector3.Dot(projectionDirection, projectionDirection);

        float d = a * e - b * b;

        if (d != 0.0f)
        {

            Vector3 r = linePosition - projectionPosition;
            float c = Vector3.Dot(lineDirection, r);
            float f = Vector3.Dot(projectionDirection, r);

            float s = (b * f - c * e) / d;
            float t = (a * f - c * b) / d;
            Vector3 position = closestPointLine1 = linePosition + lineDirection * s;
            return position.magnitude < Mathf.Infinity ? position : Vector3.zero;
        }
        return Vector3.zero;
    }
}
