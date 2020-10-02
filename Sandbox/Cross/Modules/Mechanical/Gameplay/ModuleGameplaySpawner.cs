using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleGameplaySpawnerData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleGameplaySpawnerData
            {
                name = "Spawner",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                spawnedObject = -1
            };
        }
    }
    public ModuleGameplaySpawnerData() { }
    public ModuleGameplaySpawnerData(ModuleGameplaySpawnerData moduleSpawnerData)
    {
        id = moduleSpawnerData.id;
        parentId = moduleSpawnerData.parentId;
        name = moduleSpawnerData.name;
        position = moduleSpawnerData.position;
        scale = moduleSpawnerData.scale;
        rotation = moduleSpawnerData.rotation;
        tags = moduleSpawnerData.tags;
        spawnedObject = moduleSpawnerData.spawnedObject;
    }

    internal int spawnedObject;

    internal override ModuleData Clone()
    {
        return new ModuleGameplaySpawnerData(this);
    }
}
internal sealed class ModuleGameplaySpawner : Module
{
    private ModuleGameplaySpawnerData _data = new ModuleGameplaySpawnerData();
    internal override ModuleData data => _data;

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModuleGameplaySpawnerData((ModuleGameplaySpawnerData)moduleData);
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
        SandboxValue spawnedObject = new SandboxValue
        {
            module = this,
            id = "Spawned Object",
            description = "The object that the spawner spawns.",
            wuiValueType = typeof(WUIObjectReferenceValue),
            get = () => _data.spawnedObject,
            set = (object obj) =>
            {
                _data.spawnedObject = (int)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(spawnedObject.id, spawnedObject);
    }
}
