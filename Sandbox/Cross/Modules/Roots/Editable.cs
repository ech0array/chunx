using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
internal abstract class EditableData : ModuleData
{
    public EditableData() { }
    internal string category;
    internal List<ModuleData> modules = new List<ModuleData>();
}
internal abstract class Editable : Module
{
    protected List<Connection> _childConnections = new List<Connection>();
    protected Dictionary<int, Module> _idModuleMap = new Dictionary<int, Module>();

    internal override void OnLoad()
    {
        base.OnLoad();

        int[] keys = _idModuleMap.Keys.ToArray();
        foreach (KeyValuePair<int, Module> idModuleMapEntry in _idModuleMap)
        {
            if(!(idModuleMapEntry.Value is SandboxObject))
                idModuleMapEntry.Value.OnLoad();
        }
    }

    internal void PopulateLinkDatas()
    {
        int[] keys = _idModuleMap.Keys.ToArray();
        foreach (int key in keys)
        {
            Module module = _idModuleMap[key];
            if (module is ModuleLogicLink)
            {
                ModuleLogicLink link = (ModuleLogicLink)module;

            }
            else if (module is SandboxObject)
            {
                SandboxObject sandboxObject = (SandboxObject)module;
                sandboxObject.PopulateLinkDatas();
            }
            else if (module is SandboxObjectReference)
            {
                SandboxObjectReference childSandboxObject = (SandboxObjectReference)module;
                childSandboxObject.PopulateLinkDatas();
            }
        }
    }


    internal List<ModuleData> GetModuleSaveData()
    {
        List<ModuleData> moduleDatas = new List<ModuleData>();
        int[] keys = _idModuleMap.Keys.ToArray();
        foreach (int key in keys)
        {
            Module module = _idModuleMap[key];
            module.OnSave();
            moduleDatas.Add(module.data);
        }
        return moduleDatas;
    }
    internal Module GetModuleByIdRecursive(int id)
    {
        Module module = _idModuleMap.ContainsKey(id) ? _idModuleMap[id] : null;

        if (module == null)
        {
            int[] keys = _idModuleMap.Keys.ToArray();
            foreach (int key in keys)
            {
                Module moduleEntry = _idModuleMap[key];
                if (moduleEntry is SandboxObjectReference)
                    module = ((SandboxObjectReference)moduleEntry).idModuleMap.ContainsKey(id) ? ((SandboxObjectReference)moduleEntry).idModuleMap[id] : null;
                else if (moduleEntry is SandboxObject)
                    module = ((SandboxObject)moduleEntry)._idModuleMap.ContainsKey(id) ? ((SandboxObject)moduleEntry)._idModuleMap[id] : null;
            }
        }
        return module;
    }


    internal Connection Connect(Module moduleA, Module moduleB, ConnectionType connectionType)
    {
        GameObject gameObject = Instantiate(ModuleHead.instance.prefabs.connection.gameObject, this.transform);
        Connection connection = gameObject.GetComponent<Connection>();
        connection.Connect(moduleA, moduleB);
        connection.SetColor(connectionType == ConnectionType.Link ? ModuleHead.instance.data.linkConnectionColor : ModuleHead.instance.data.weldConnectionColor);
        _childConnections.Add(connection);
        connection.parent = this;
        return connection;
    }
    internal void Disconnect(Connection connection)
    {
        _childConnections.Remove(connection);
        Destroy(connection.gameObject);
    }


    internal Module NewModule(Module prefab)
    {
        GameObject gameObject = Instantiate(prefab.gameObject, this.transform);
        Module module = gameObject.GetComponent<Module>();
        ModuleData moduleData = module.data.defaultData;
        moduleData.id = GameHead.instance.universeData.GenerateUniqueId();

        _idModuleMap.Add(moduleData.id, module);
        module.parent = this;

        gameObject.name = moduleData.name;

        module.PopulateData(moduleData);
        module.OnLoad();
        return module;
    }
    internal Module DuplicateModule(Module module)
    {
        Module prefab = ModuleHead.instance.GetModulePrefab(module.data.GetType());
        GameObject gameObject = Instantiate(prefab.gameObject, this.transform);
        gameObject.name = module.data.name;
        Module instance = gameObject.GetComponent<Module>();
        instance.parent = this;

        instance.PopulateData(module.data);
        instance.data.id = GameHead.instance.universeData.GenerateUniqueId();
        _idModuleMap.Add(instance.data.id, instance);
        instance.OnLoad();
        return instance;
    }
    protected void SpawnModules(List<ModuleData> moduleDatas)
    {
        if (moduleDatas == null)
            return;
        for (int i = 0; i < moduleDatas.Count; i++)
            SpawnModule(moduleDatas[i]);
        OnLoad();
        PopulateLinkDatas();
    }
    private Module SpawnModule(ModuleData moduleData)
    {
        if (GameHead.instance.isPreviewOrRuntime)
        {
            if (moduleData is SandboxObjectReferenceData || moduleData is SandboxObjectSpawnData)
            {
                SandboxObjectData sandboxObjectData = new SandboxObjectData(ModuleHead.GetSandboxObjectDataById(GameHead.instance.universeData, moduleData.id));
                sandboxObjectData.position = moduleData.position;
                sandboxObjectData.rotation = moduleData.rotation;
                sandboxObjectData.scale = moduleData.scale;
                moduleData = sandboxObjectData;
            }
        }

        Module prefab = ModuleHead.instance.GetModulePrefab(moduleData.GetType());
        GameObject gameObject = Instantiate(prefab.gameObject, base.transform);
        gameObject.name = moduleData.name;
        Module instance = gameObject.GetComponent<Module>();
        instance.parent = this;

        _idModuleMap.Add(moduleData.id, instance);
        instance.PopulateData(moduleData);
        return instance;
    }
    internal void UnregisterModule(Module module)
    {
        if (GameHead.instance.gameState == GameState.Editor && !(module is SandboxObjectReference))
            GameHead.instance.universeData.uniqueIds.Remove(module.data.id);
        _idModuleMap.Remove(module.data.id);
    }


    internal Module NewSandboxObjectReference(SandboxObjectReferenceData sandboxObjectReferenceData)
    {
        GameObject gameObject = Instantiate(ModuleHead.instance.GetModulePrefab(typeof(SandboxObjectReferenceData)).gameObject, this.transform);
        SandboxObjectReference sandboxObjectReference = gameObject.GetComponent<SandboxObjectReference>();
        _idModuleMap.Add(sandboxObjectReferenceData.id, sandboxObjectReference);

        sandboxObjectReference.parent = this;
        sandboxObjectReference.PopulateData(sandboxObjectReferenceData);
        sandboxObjectReference.OnLoad();
        return sandboxObjectReference;
    }
    internal SandboxObjectSpawn NewSandboxObjectSpawn(SandboxObjectSpawnData sandboxObjectSpawnData)
    {
        GameObject gameObject = Instantiate(ModuleHead.instance.GetModulePrefab(typeof(SandboxObjectSpawnData)).gameObject, this.transform);
        SandboxObjectSpawn sandboxObjectSpawn = gameObject.GetComponent<SandboxObjectSpawn>();
        _idModuleMap.Add(sandboxObjectSpawnData.id, sandboxObjectSpawn);

        sandboxObjectSpawn.parent = this;
        sandboxObjectSpawn.PopulateData(sandboxObjectSpawnData);
        sandboxObjectSpawn.OnLoad();
        return sandboxObjectSpawn;
    }
}