using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

internal sealed class WUILoomModuleMembersMemberElement : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;

        internal WUILoomModuleMembers wUILoomModuleMembers;
    }
    [SerializeField]
    private Components _components;


    [Serializable]
    private class Data
    {
        internal SandboxMember sandboxMember;
        internal Module module;

    }
    [SerializeField]
    private Data _data;


    internal void Populate(WUILoomModuleMembers wUILoomModuleMembers, SandboxMember sandboxMember)
    {
        _data.sandboxMember = sandboxMember;
        _components.wUILoomModuleMembers = wUILoomModuleMembers;
        _components.nameLabel.text = $"{(sandboxMember is SandboxCall ? "CALL" : "EVENT")} : {sandboxMember.id}";
    }

    internal void Populate(WUILoomModuleMembers wUILoomModuleMembers, Module module)
    {
        _components.nameLabel.text = $"DATA";
        _data.module = module;
        _components.wUILoomModuleMembers = wUILoomModuleMembers;

    }

    public void UE_Click()
    {
        if (_data.sandboxMember != null)
            _components.wUILoomModuleMembers.Select(_data.sandboxMember);
        else
            _components.wUILoomModuleMembers.Select(_data.module.sandboxValuesById.Values.ToList());
    }

}
