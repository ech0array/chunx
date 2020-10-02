using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TransformData
{
    internal Vector3 position;
    internal Quaternion rotation;
    internal Vector3 scale;

    internal TransformData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }
    internal void ApplyTo(Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
}
