using System;
using System.Collections.Generic;
using UnityEngine;

internal enum EditorType
{
    Map,
    Object
}

internal sealed class EditorHead : SingleMonoBehaviour<EditorHead>
{
    #region Values
    protected override bool isPersistant => true;
    internal bool isMap => editableData is SandboxData;

    internal EditableData editableData;
    internal Editable editable;


    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Transform environment;
    }
    [SerializeField]
    private Components _components;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _components.environment.gameObject.SetActive(false);
    }

    internal void OnStateEnter()
    {
        _components.environment.gameObject.SetActive(true);

        ControllableCamera.SetAllMode(ControllableCamera.Mode.Module);
        ControllableCamera.PositionAllAt(new Vector3(10, 5, 10));
        ControllableCamera.LookAllAt(new Vector3(0, 0, 0));
    }
    internal void OnStateExit()
    {
        WidgetHead.instance.HideAll();
        _components.environment.gameObject.SetActive(false);
        ModuleHead.instance.Cleanup();
    }

    internal SandboxData NewSandbox(int sandboxID)
    {
        SandboxData sandboxData = new SandboxData
        {
            id = sandboxID,
            name = $"SANDBOX : {sandboxID}",
            position = Vector3.zero,
            rotation = Vector3.zero,
            scale = Vector3.one,
            modules = new List<ModuleData>()
        };
        return sandboxData;
    }
    internal SandboxObjectData NewSandboxObject(int sandboxObjectID, int publicLinkID)
    {
        SandboxObjectData sandboxObjectData = new SandboxObjectData
        {
            id = sandboxObjectID,
            name = $"SANDBOX OBJECT : {sandboxObjectID}",
            position = Vector3.zero,
            rotation = Vector3.zero,
            scale = Vector3.one,
        };
        sandboxObjectData.modules.Add(new ModuleLogicLinkData { id = publicLinkID, name = $"PUBLIC LINK : {sandboxObjectID}", isPublic = true, position = Vector3.up * 2f, scale = Vector3.one * 0.25f });
        return sandboxObjectData;
    }
    internal void Edit(UniverseData universeData, EditableData editableData)
    {
        GameHead.instance.universeData = universeData;
        this.editableData = editableData;


        GameHead.instance.EnterGameState(GameState.Editor);

        editable = ModuleHead.instance.Spawn(editableData);
        editable.transform.position = Vector3.zero;
    }

    internal void ReturnToEdit()
    {
        Edit(GameHead.instance.universeData, editableData);
    }


    internal Module NewModule(Module prefab, Vector3 position)
    {
        Module module = editable.NewModule(prefab);
        module.gameObject.transform.position = position;
        return module;
    }
    internal SandboxObjectReference NewChildSandboxObject(SandboxObjectReferenceData childSandboxObjectData, Vector3 position)
    {
        SandboxObjectReference childSandboxObject = (SandboxObjectReference)editable.NewSandboxObjectReference(childSandboxObjectData);
        childSandboxObject.gameObject.transform.position = position;
        return childSandboxObject;
    }
    internal SandboxObjectSpawn NewSandboxObjectSpawn(SandboxObjectSpawnData sandboxObjectSpawnData, Vector3 position)
    {
        SandboxObjectSpawn sandboxObjectSpawn = (SandboxObjectSpawn)editable.NewSandboxObjectSpawn(sandboxObjectSpawnData);
        sandboxObjectSpawn.gameObject.transform.position = position;
        return sandboxObjectSpawn;
    }

    internal void Save()
    {
        editableData.modules = editable.GetModuleSaveData();
        UserHead.SaveAllUsersData();
    }

    internal List<SandboxObjectData> GetNestableObjects()
    {
        List<SandboxObjectData> nonOverflowingSandboxObjects = new List<SandboxObjectData>();
        for (int i = 0; i < GameHead.instance.universeData.objects.Count; i++)
        {
            SandboxObjectData sandboxObjectData = (SandboxObjectData)GameHead.instance.universeData.objects[i];
            if (sandboxObjectData.id == editableData.id)
                continue;
            if (sandboxObjectData.modules.Count > 0)
            {
                bool overflowable = HasChildRecursive(sandboxObjectData, editableData.id);
                if (overflowable)
                    continue;
            }

            nonOverflowingSandboxObjects.Add(sandboxObjectData);
        }
        return nonOverflowingSandboxObjects;
    }
    private bool HasChildRecursive(SandboxObjectData sandboxObjectData, int id)
    {
        for (int i = 0; i < sandboxObjectData.modules.Count; i++)
        {
            if (!(sandboxObjectData.modules[i] is SandboxObjectReferenceData))
                continue;
            SandboxObjectReferenceData childSandboxObjectData = (SandboxObjectReferenceData)sandboxObjectData.modules[i];
            int childId = childSandboxObjectData.id;
            if (childId == id)
                return true;
            bool overflow = HasChildRecursive(ModuleHead.GetSandboxObjectDataById(GameHead.instance.universeData, childId), id);
            if (overflow)
                return true;
        }
        return false;
    }
}