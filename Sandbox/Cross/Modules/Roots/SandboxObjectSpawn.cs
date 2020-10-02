using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class SandboxObjectSpawnData : ModuleData
{
    internal override ModuleData defaultData => new SandboxObjectSpawnData();

    internal bool respawns = true;
    internal float respawnDuration = 30f;
    internal float initialSpawnDelay = 0f;

    internal SandboxObjectSpawnData() { }
    internal SandboxObjectSpawnData(SandboxObjectData sandboxObjectData)
    {
        id = sandboxObjectData.id;
        name = sandboxObjectData.name;
    }
    internal SandboxObjectSpawnData(SandboxObjectSpawnData sandboxObjectSpawnData)
    {
        id = sandboxObjectSpawnData.id;
        name = sandboxObjectSpawnData.name;
        respawns = sandboxObjectSpawnData.respawns;
        respawnDuration = sandboxObjectSpawnData.respawnDuration;
    }

    internal override ModuleData Clone()
    {
        return new SandboxObjectSpawnData(this);
    }
}

internal sealed class SandboxObjectSpawn : Module
{
    #region Values
    private SandboxObjectSpawnData _data = new SandboxObjectSpawnData();
    internal override ModuleData data => _data;
    internal List<Module> modules = new List<Module>();
    #endregion

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new SandboxObjectSpawnData((SandboxObjectSpawnData)moduleData);
        SandboxObjectData sandboxObjectData = ModuleHead.GetSandboxObjectDataById(GameHead.instance.universeData, _data.id);
        SpawnNestedModules(sandboxObjectData);
        ApplyData();
    }

    #region Functions
    internal override void OnLoad()
    {
        base.OnLoad();
        for (int i = 0; i < modules.Count; i++)
            modules[i].OnLoad();
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();
        SandboxValue respawns = new SandboxValue
        {
            module = this,
            id = "Respawns",
            description = "Determines if the object is to respawn.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.respawns,
            set = (object obj) =>
            {
                _data.respawns = (bool)obj;
                ApplyData();
            },
            meta = null
        };
        sandboxValuesById.Add(respawns.id, respawns);

        SandboxValue respawnDuration = new SandboxValue
        {
            module = this,
            id = "Respawn Duration",
            description = "The duration along-which it takes for the object to spawn.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.respawnDuration,
            set = (object obj) =>
            {
                _data.respawnDuration = (float)obj;
                ApplyData();
            },
            meta = null
        };
        sandboxValuesById.Add(respawnDuration.id, respawnDuration);

        SandboxValue initialSpawnDelay = new SandboxValue
        {
            module = this,
            id = "Initial Spawn Delay",
            description = "The duration along-which the object will wait to first spawn.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.initialSpawnDelay,
            set = (object obj) =>
            {
                _data.initialSpawnDelay = (float)obj;
                ApplyData();
            },
            meta = null
        };
        sandboxValuesById.Add(initialSpawnDelay.id, initialSpawnDelay);
    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();
    }

    internal void PopulateLinkScriptDatas()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i] is ModuleLogicLink)
            {
                ModuleLogicLink link = (ModuleLogicLink)modules[i];

            }
        }
    }

    private void SpawnNestedModules(SandboxObjectData sandboxObjectData)
    {
        foreach (ModuleData moduleData in sandboxObjectData.modules)
        {
            bool isPublicLink = moduleData is ModuleLogicLinkData && ((ModuleLogicLinkData)moduleData).isPublic;
            if (moduleData is ModuleArtData || moduleData is SandboxObjectReferenceData || isPublicLink)
                SpawnNestedModule(moduleData);
        }
    }
    private Module SpawnNestedModule(ModuleData moduleData)
    {
        Module prefab = ModuleHead.instance.GetModulePrefab(moduleData.GetType());
        GameObject gameObject = Instantiate(prefab.gameObject, base.gameObject.transform);
        Module instance = gameObject.GetComponent<Module>();
        instance.passive = true;
        instance.parent = null;
        instance.PopulateData(moduleData);
        modules.Add(instance);
        return instance;
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
    #endregion
}