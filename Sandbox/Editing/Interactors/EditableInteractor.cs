using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed internal class EditableInteractor : Interactor
{
    #region Values
    protected override ControllableCamera controllableCamera { get; set; }

    internal enum Mode
    {
        None,
        Selection
    }

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal float interactionDistance = 8f;

        internal bool queueSelection = false;

        internal Mode mode;
    }
    [SerializeField]
    private Data _data; 
    #endregion

    private void Update()
    {
        if (_data.mode == Mode.None)
            return;

        UpdateSelectionMode();
    }

    internal void EnterMode(Mode mode, Module module = null)
    {
        _data.mode = mode;
    }

    internal override void Register(ControllableCamera controllableCamera)
    {
        this.controllableCamera = controllableCamera;
    }

    internal void ActionSelect()
    {
        if (_data.mode == Mode.None)
            return;

        _data.queueSelection = true;
    }

    private void UpdateSelectionMode()
    {
        controllableCamera.user.hUD.SetReticlePassive();
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, _data.interactionDistance);


        if (hit)
        {
            Module module = raycastHit.collider.gameObject.GetComponent<Module>();
            if (module != null && !module.passive)
            {
                controllableCamera.user.hUD.SetReticleHighlight(ModuleHead.instance.GetInspectionDataOfType(module.GetType()).color);

                controllableCamera.SetPreviewing(true);
                controllableCamera.user.editorWUI.wUIEditablePreview.Inspect(module, module.data.name, raycastHit.point);
                if (_data.queueSelection)
                {
                    bool isNestedPublicLink = module.parent != EditorHead.instance.editable && module is ModuleLogicLink && ((ModuleLogicLink)module).isPublic;
                    if (!isNestedPublicLink)
                        controllableCamera.EnterMode(ControllableCamera.Mode.WUI, module, raycastHit.point);
                }
            }
            else
            {
                controllableCamera.SetPreviewing(false);
                controllableCamera.user.editorWUI.wUIEditablePreview.Hide();
            }
        }
        else
        {
            controllableCamera.SetPreviewing(false);
            controllableCamera.user.editorWUI.wUIEditablePreview.Hide();
        }
        _data.queueSelection = false;
    }
}