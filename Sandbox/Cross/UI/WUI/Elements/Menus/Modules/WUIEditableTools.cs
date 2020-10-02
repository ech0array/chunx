using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class WUIEditableTools : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIEditableTransform wUIEditableTransform;

        [SerializeField]
        internal Button delete;
        [SerializeField]
        internal Button duplicate;
        [SerializeField]
        internal Button transform;
        [SerializeField]
        internal Button rename;

        internal Module module;
    }
    [SerializeField]
    private Components _components;

    internal void Inspect(Module module)
    {
        _components.module = module;

        bool isPublicLink = module is ModuleLogicLink && ((ModuleLogicLink)module).isPublic;
        bool isSandboxReference = module is SandboxObjectReference || module is SandboxObjectSpawn;
        _components.delete.gameObject.SetActive(!isPublicLink);
        _components.duplicate.gameObject.SetActive(!isPublicLink);
        _components.rename.gameObject.SetActive(!isPublicLink && !isSandboxReference);
        _components.transform.gameObject.SetActive(true);

        base.gameObject.SetActive(true);
        Stack();
    }

    #region Unity Event Functions
    public void UE_Delete()
    {
        wUI.Confirm("WARNING! YOU ARE ABOUT TO DELETE A MODULE. YOU CANNOT UNDO THIS. PLEASE RECONSIDER BEFORE CONTINUING.", () =>
        {
            _components.module.Delete();
            wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Module, _components.module);
            wUI.UnstackAll();
        });
    }
    public void UE_Duplicate()
    {
        _components.module = EditorHead.instance.editable.DuplicateModule(_components.module);
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Widget, _components.module);
        WidgetHead.instance.Edit(_components.module, WidgetType.Move);
        wUI.UnstackAll();
    }

    public void UE_Transform()
    {
        _components.wUIEditableTransform.Inspect(_components.module);
    }

    public void UE_Rename()
    {
        wUI.Edit(_components.module.data.name, (str) => { _components.module.data.name = str; });
    }

    #endregion
}
