using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class WUILoomInpsector : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameText;

        [SerializeField]
        internal GameObject linkButton;
        [SerializeField]
        internal GameObject unlinkButton;

        [SerializeField]
        internal WUIEditableTools wUIEditableTools;


        [SerializeField]
        internal WUILoomModules wUILoomModules;

        [SerializeField]
        internal RectTransform nodeContainer;

        internal List<WUILoomNode> wUILoomNodes = new List<WUILoomNode>();

    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUILoomNodeModuleData wUILoomNodeModuleData;
        [SerializeField]
        internal WUILoomNodeModuleEvent wUILoomNodeModuleEvent;
        [SerializeField]
        internal WUILoomNodeModuleCallOut wUILoomNodeModuleCallOut;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal ModuleLogicLink moduleLogicLink;
    internal void Inspect(ModuleLogicLink moduleLogicLink, Vector3 cursorPosition)
    {
        this.moduleLogicLink = moduleLogicLink;

        _components.nameText.text = moduleLogicLink.data.name;

        moduleLogicLink.OnEditorInspect();

        Stack();
        wUI.Position(cursorPosition);
        bool linkIsLocal = moduleLogicLink.parent == EditorHead.instance.editable;
        _components.linkButton.SetActive(linkIsLocal);
        _components.unlinkButton.SetActive(linkIsLocal);

        // moduleLogicLink.loomNodes

    }

    internal override void Unstack()
    {
        base.Unstack();
        Clear();
    }

    private void Update()
    {
        if (!base.isInFocus)
            return;
        UpdateInput();
    }

    private void UpdateInput()
    {
        if(base.wUI.user.input.actionX && !base.wUI.user.deltaInput.actionX)
            _components.wUILoomModules.Inspect(moduleLogicLink.connectedModules);
    }


    public void UE_OpenTools()
    {
        _components.wUIEditableTools.Inspect(moduleLogicLink);
    }

    public void UE_Connect()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Link, moduleLogicLink);
        Unstack();
    }
    public void UE_Disconnect()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Unlink, moduleLogicLink);
        Unstack();
    }


    internal void Create(SandboxMember sandboxMember, Vector3 position)
    {
        if(sandboxMember is SandboxEvent)
        {
            GameObject gameObject = Instantiate( _prefabs.wUILoomNodeModuleEvent.gameObject, _components.nodeContainer);
            WUILoomNodeModuleEvent wUILoomNodeModuleEvent = gameObject.GetComponent<WUILoomNodeModuleEvent>();
            _components.wUILoomNodes.Add(wUILoomNodeModuleEvent);
            wUILoomNodeModuleEvent.Populate(new LoomEvent((SandboxEvent)sandboxMember));
            gameObject.transform.position = position;
        }
        else
        {
            GameObject gameObject = Instantiate(_prefabs.wUILoomNodeModuleCallOut.gameObject, _components.nodeContainer);
            WUILoomNodeModuleCallOut wUILoomNodeModuleCallOut = gameObject.GetComponent<WUILoomNodeModuleCallOut>();
            _components.wUILoomNodes.Add(wUILoomNodeModuleCallOut);
            gameObject.transform.position = position;
        }
    }
    internal void Create(List<SandboxValue> sandboxValues, Vector3 position)
    {
        GameObject gameObject = Instantiate(_prefabs.wUILoomNodeModuleData.gameObject, _components.nodeContainer);
        WUILoomNodeModuleData wUILoomNodeModuleCallOut = gameObject.GetComponent<WUILoomNodeModuleData>();
        _components.wUILoomNodes.Add(wUILoomNodeModuleCallOut);
        gameObject.transform.position = position;
        wUILoomNodeModuleCallOut.Populate(new LoomModuleData(sandboxValues));
    }

    internal void Clear()
    {
        foreach (WUILoomNode wUILoomNode in _components.wUILoomNodes)
        {
            Destroy(wUILoomNode.gameObject);
        }
        _components.wUILoomNodes.Clear();
    }
}
