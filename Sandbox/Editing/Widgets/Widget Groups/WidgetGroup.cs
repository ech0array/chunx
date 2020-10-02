using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class WidgetGroup : MonoBehaviour
{
    protected abstract void BindTransformAction();
    protected abstract void OnTransformAction(object[] values);

    internal abstract void SetModule(Module value);

    internal abstract void Close();
}