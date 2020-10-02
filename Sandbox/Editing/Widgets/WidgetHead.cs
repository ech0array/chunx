using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum WidgetType
{
    Move,
    Rotate,
    PivotRotate,
    Scale
}

internal sealed class WidgetHead : SingleMonoBehaviour<WidgetHead>
{
    protected override bool isPersistant => true;

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WidgetGroupMovementAxes widgetGroupMovementAxes;
        [SerializeField]
        internal WidgetGroupRotation widgetGroupRotation;
        [SerializeField]
        internal WidgetGroupPivotRotation widgetGroupPivotRotation;
        [SerializeField]
        internal WidgetGroupScale widgetGroupScale;

        [SerializeField]
        internal WidgetForwardDirection widgetForwardDirection;
    }
    [SerializeField]
    private Components _components;

    internal void Edit(Module module, WidgetType widgetType, Vector3 pivot = default(Vector3))
    {
        switch (widgetType)
        {
            case WidgetType.Move:
                _components.widgetGroupMovementAxes.SetModule(module);
                break;
            case WidgetType.Rotate:
                _components.widgetGroupRotation.SetModule(module);
                break;
            case WidgetType.PivotRotate:
                _components.widgetGroupPivotRotation.SetModule(module, pivot);
                break;
            case WidgetType.Scale:
                _components.widgetGroupScale.SetModule(module);
                break;
            default:
                break;
        }
    }

    internal void ShowForwardOf(Module module, bool visible)
    {
        _components.widgetForwardDirection.gameObject.SetActive(visible);
        if (visible)
            _components.widgetForwardDirection.SetModule(module);
        else
            _components.widgetForwardDirection.Release();
    }

    internal void HideAll()
    {
        _components.widgetGroupMovementAxes.Close();
        _components.widgetGroupPivotRotation.Close();
        _components.widgetGroupRotation.Close();
        _components.widgetGroupScale.Close();
    }
}