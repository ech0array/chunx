using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class WUILoomModuleMembers : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform memberContainer;

        internal List<WUILoomModuleMembersMemberElement> wUILoomModuleMembersMemberElements = new List<WUILoomModuleMembersMemberElement>();

        internal WUILoomModules wUILoomModules;
    }
    [SerializeField]
    private Components _components;


    [Serializable]
    private class Data
    {
    }
    [SerializeField]
    private Data _data;


    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUILoomModuleMembersMemberElement wUILoomModuleMembersMemberElement;
    }
    [SerializeField]
    private Prefabs _prefabs;



    internal void Populate(WUILoomModules wUILoomModules, Module module)
    {
        Clear();


        _components.wUILoomModules = wUILoomModules;



        // add data set
        GameObject gameObject = Instantiate(_prefabs.wUILoomModuleMembersMemberElement.gameObject, _components.memberContainer, false);
        WUILoomModuleMembersMemberElement wUILoomModuleMembersMemberElement = gameObject.GetComponent<WUILoomModuleMembersMemberElement>();
        wUILoomModuleMembersMemberElement.Populate(this, module);


        foreach (KeyValuePair<string, SandboxEvent> entry in module.sandboxEventsById)
            CreateMember(entry.Value);

        foreach (KeyValuePair<string, SandboxCall> entry in module.sandboxCallsById)
            CreateMember(entry.Value);
        Stack();
    }

    private void CreateMember(SandboxMember sandboxMember)
    {
        GameObject gameObject = Instantiate(_prefabs.wUILoomModuleMembersMemberElement.gameObject, _components.memberContainer, false);
        WUILoomModuleMembersMemberElement wUILoomModuleMembersMemberElement = gameObject.GetComponent<WUILoomModuleMembersMemberElement>();
        wUILoomModuleMembersMemberElement.Populate(this, sandboxMember);
    }


    private void Clear()
    {
        foreach (WUILoomModuleMembersMemberElement wUILoomModuleMembersMemberElement in _components.wUILoomModuleMembersMemberElements)
            Destroy(wUILoomModuleMembersMemberElement.gameObject);

        _components.wUILoomModuleMembersMemberElements.Clear();
    }

    internal void Select(SandboxMember sandboxMember)
    {
        base.Unstack();
        _components.wUILoomModules.Unstack();
        wUI.user.editorWUI.wUILoomInpsector.Create(sandboxMember, base.transform.position);
    }

    internal void Select(List<SandboxValue> sandboxValues)
    {
        base.Unstack();
        _components.wUILoomModules.Unstack();
        wUI.user.editorWUI.wUILoomInpsector.Create(sandboxValues, base.transform.position);
    }

}
