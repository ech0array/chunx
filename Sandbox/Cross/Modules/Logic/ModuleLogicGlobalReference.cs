using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleLogicGlobalReferenceData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleLogicGlobalReferenceData
            {
                name = "Global References",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero
            };
        }
    }
    public ModuleLogicGlobalReferenceData() { }
    public ModuleLogicGlobalReferenceData(ModuleLogicGlobalReferenceData globalLinkData)
    {
        name = globalLinkData.name;
        id = globalLinkData.id;
        parentId = globalLinkData.parentId;
        position = globalLinkData.position;
        scale = globalLinkData.scale;
        rotation = globalLinkData.rotation;
        tags = globalLinkData.tags;
    }
    internal override ModuleData Clone()
    {
        return new ModuleLogicGlobalReferenceData(this);
    }
}

internal sealed class ModuleLogicGlobalReference : Module
{
    private ModuleLogicGlobalReferenceData _data = new ModuleLogicGlobalReferenceData();
    internal override ModuleData data => _data;


    private void OnDestroy()
    {
        UnbindEvents();
    }

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModuleLogicGlobalReferenceData((ModuleLogicGlobalReferenceData)moduleData);
        ApplyData();
    }
    internal override bool OnAttach(User user)
    {
        this.user = user;
        return true;
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();
        SandboxValue teamAScore = new SandboxValue
        {
            module = this,
            id = "Team A Score",
            description = "The score of team A.",
            wuiValueType = typeof(WUIIntergerValue),
            get = () => LinkHead.instance.values.teamAScore,
            set = (object obj) =>
            {
                LinkHead.instance.values.teamAScore = (int)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(teamAScore.id, teamAScore);
    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();

        BindEvents();

    }
    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();

        SandboxCall sandboxAction = new SandboxCall
        {
            module = this,
            id = "test_action_0",
            action = (obj) => { LinkHead.instance.TestGlobalCall(); }
        };
        sandboxCallsById.Add(sandboxAction.id, sandboxAction);

    }

    private void BindEvents()
    {
        LinkHead.instance.onTest += TestBinding;
    }
    private void UnbindEvents()
    {
        LinkHead.instance.onTest -= TestBinding;
    }

    private void TestBinding()
    {
    }
}
