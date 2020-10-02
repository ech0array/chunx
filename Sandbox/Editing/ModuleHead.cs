using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

internal sealed class ModuleHead : SingleMonoBehaviour<ModuleHead>
{
    protected override bool isPersistant => true;

    #region Values
    [Serializable]
    internal class Components
    {
        [SerializeField]
        internal Transform objectsContainer;

        internal Connection spoofConnection;

        internal Dictionary<int, Editable> idEditableMap = new Dictionary<int, Editable>();
    }
    [SerializeField]
    internal Components _components = new Components();

    [Serializable]
    internal class Prefabs
    {
        [SerializeField]
        internal Connection connection;

        [SerializeField]
        internal List<Module> modules = new List<Module>();

        internal Dictionary<Type, Module> moduleDataTypeMap = new Dictionary<Type, Module>();
        internal Dictionary<ModuleCategory, List<Module>> moduleCategoryMap = new Dictionary<ModuleCategory, List<Module>>();
    }
    [SerializeField]
    internal Prefabs prefabs;

    [Serializable]
    internal class Data
    {
        [SerializeField]
        internal Color weldConnectionColor;
        [SerializeField]
        internal Color linkConnectionColor;
    }
    [SerializeField]
    internal Data data = new Data();
    #endregion

    #region Unity Framework Entry Functions
    protected override void Awake()
    {
        base.Awake();
        DefineSpoofConnection();
        DefineModuleDataTypeMap();
        DefineModuleCategoryMap();
    }
    #endregion

    internal void Cleanup()
    {
        int[] keys = _components.idEditableMap.Keys.ToArray();
        foreach (int key in keys)
            Destroy(_components.idEditableMap[key].gameObject);

        _components.idEditableMap.Clear();
    }

    private void DefineModuleDataTypeMap()
    {
        foreach (Module module in prefabs.modules)
        {
            if (module.data == null)
                Debug.LogError($"{module.gameObject.name} Does not have set data... Fix that shit...");
            else
                prefabs.moduleDataTypeMap.Add(module.data.GetType(), module);
        }
    }
    private void DefineModuleCategoryMap()
    {
        foreach (Module module in prefabs.modules)
        {
            if(!prefabs.moduleCategoryMap.ContainsKey(module.inspectionData.category))
                prefabs.moduleCategoryMap.Add(module.inspectionData.category, new List<Module>());

            prefabs.moduleCategoryMap[module.inspectionData.category].Add(module);
        }
    }

    internal InspectionData GetInspectionDataOfType(Type type)
    {
        foreach (Module module in prefabs.modules)
            if (module.GetType() == type)
                return module.inspectionData;

        return null;
    }


    internal Module GetModulePrefab(Type type)
    {
        return prefabs.moduleDataTypeMap[type];
    }
    internal List<Module> GetModulesOfCategory(ModuleCategory moduleCategory)
    {
        return prefabs.moduleCategoryMap[moduleCategory];
    }


    private void DefineSpoofConnection()
    {
        _components.spoofConnection = Instantiate(prefabs.connection, base.transform);
        _components.spoofConnection.gameObject.SetActive(false);
        _components.spoofConnection.gameObject.name = "spoof_connection";
    }
    internal void ShowSpoofConnection(Module module, Vector3 vector3B)
    {
        _components.spoofConnection.gameObject.SetActive(true);
        _components.spoofConnection.Spoof(module.gameObject.transform.position, vector3B);
        _components.spoofConnection.SetColor(data.linkConnectionColor);
    }
    internal void HideSpoofConnection()
    {
        _components.spoofConnection.gameObject.SetActive(false);
    }


    internal Editable Spawn(EditableData editableData)
    {
        GameObject gameObject = Instantiate(GetModulePrefab(editableData.GetType()).gameObject, _components.objectsContainer);
        Editable editable = gameObject.GetComponent<Editable>();
        gameObject.name = editableData.name;
        editable.PopulateData(editableData);
        _components.idEditableMap.Add(editableData.id, editable);
        return editable;
    }
    internal static SandboxObjectData GetSandboxObjectDataById(UniverseData universeData, int id)
    {
        return (SandboxObjectData)universeData.objects.Find(sbo => sbo.id == id);
    }




    // should update to view id... not sure what this was for ( could be for scripting? )

    internal void InvokeModuleCall(int editableID, int moduleID, string callID)
    {
        _components.idEditableMap[editableID].GetModuleByIdRecursive(moduleID).InvokeCall(callID);
    }
    internal void InvokeModuleEvent(int editableID, int moduleID, string eventID)
    {
        _components.idEditableMap[editableID].GetModuleByIdRecursive(moduleID).InvokeEvent(eventID);
    }
}