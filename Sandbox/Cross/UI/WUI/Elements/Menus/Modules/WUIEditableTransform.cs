using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class WUIEditableTransform : WUIMenu
{

    [Serializable]
    private class Components
    {
        internal Module module;
    }
    [SerializeField]
    private Components _components;

    internal void Inspect(Module module)
    {
        _components.module = module;
        Stack();
    }

    public void UE_Move()
    {
        WidgetHead.instance.Edit(_components.module, WidgetType.Move);
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Widget, _components.module);
        wUI.UnstackAll();
    }
    public void UE_Rotate()
    {
        WidgetHead.instance.Edit(_components.module, WidgetType.Rotate);
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Widget, _components.module);
        wUI.UnstackAll();
    }
    public void UE_PivotRotate()
    {
        WidgetHead.instance.Edit(_components.module, WidgetType.PivotRotate, wUI.origin);
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Widget, _components.module);
        wUI.UnstackAll();
    }
    public void UE_Scale()
    {
        WidgetHead.instance.Edit(_components.module, WidgetType.Scale);
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Widget, _components.module);
        wUI.UnstackAll();
    }

    public void UE_QuickMove()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.QuickMove, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
    public void UE_QuickRotate()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.QuickRotate, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
    public void UE_QuickScale()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.QuickScale, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
    public void UE_MimicPosition()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.MimicPosition, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
    public void UE_MimicRotation()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.MimicRotation, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
    public void UE_MimicScale()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.MimicScale, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
    public void UE_MimicAll()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.MimicAll, _components.module, wUI.origin);
        wUI.UnstackAll();
    }
}
