using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
internal sealed class SandboxObjectReferenceData : ModuleData
{
    internal override ModuleData defaultData => new SandboxObjectReferenceData();
    public SandboxObjectReferenceData() { }
    public SandboxObjectReferenceData(SandboxObjectData sandboxObjectData)
    {
        id = sandboxObjectData.id;
        name = sandboxObjectData.name;
    }
    public SandboxObjectReferenceData(SandboxObjectReferenceData sandboxObjectReferenceData)
    {
        id = sandboxObjectReferenceData.id;
        name = sandboxObjectReferenceData.name;
    }

    internal override ModuleData Clone()
    {
        return new SandboxObjectReferenceData(this);
    }
}
internal sealed class SandboxObjectReference : Module
{
    #region Values
    private SandboxObjectReferenceData _data = new SandboxObjectReferenceData();
    internal override ModuleData data => _data;
    internal Dictionary<int, Module> idModuleMap = new Dictionary<int, Module>();
    #endregion

    #region Functions
    internal override void PopulateData(ModuleData moduleData)
    {
        _data = (SandboxObjectReferenceData)moduleData;
        SandboxObjectData sandboxObjectData = ModuleHead.GetSandboxObjectDataById(GameHead.instance.universeData, _data.id);
        SpawnNestedModules(sandboxObjectData);
        ApplyData();
    }

    internal override void OnLoad()
    {
        base.OnLoad();
        int[] keys = idModuleMap.Keys.ToArray();
        foreach (int key in keys)
           idModuleMap[key].OnLoad();
    }

    internal void PopulateLinkDatas()
    {

        int[] keys = idModuleMap.Keys.ToArray();
        foreach (int key in keys)
        {
            Module module = idModuleMap[key];
            if (module is ModuleLogicLink)
            {
                ModuleLogicLink link = (ModuleLogicLink)module;

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
        instance.passive = !(moduleData is ModuleLogicLinkData) && !passive;
        instance.parent = null;
        instance.PopulateData(moduleData);
        idModuleMap.Add(moduleData.id, instance);
        return instance;
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
    #endregion
}