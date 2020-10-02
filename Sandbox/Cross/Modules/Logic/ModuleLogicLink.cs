using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


[Serializable]
internal class ModuleLogicLinkData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleLogicLinkData
            {
                name = "Link",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero
            };
        }
    }

    [SerializeField]
    internal bool isPublic;

    [SerializeField]
    internal List<LoomNodeData> loomNodeDatas = new List<LoomNodeData>();

    [SerializeField]
    internal List<int> connections = new List<int>();



    public ModuleLogicLinkData() { }
    public ModuleLogicLinkData(ModuleLogicLinkData linkData)
    {
        isPublic = linkData.isPublic;
        id = linkData.id;
        parentId = linkData.parentId;

        name = linkData.name;
        position = linkData.position;
        rotation = linkData.rotation;
        scale = linkData.scale;

        connections = new List<int>(linkData.connections);
    }

    internal override ModuleData Clone()
    {
        return new ModuleLogicLinkData(this);
    }
}

internal class ModuleLogicLink : Module
{
    #region Values
    internal bool isPublic => _data.isPublic;
    protected ModuleLogicLinkData _data = new ModuleLogicLinkData();
    internal override ModuleData data
    {
        get
        {
            return _data;
        }
    }

    internal List<Module> connectedModules = new List<Module>();
    internal List<LoomNode> loomNodes = new List<LoomNode>();

    #endregion

    #region Population
    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModuleLogicLinkData((ModuleLogicLinkData)moduleData);
        ApplyData();
    }

    protected override void OnEditorLoad()
    {
        DisplayConnections();
    }
    protected override void OnRuntimeLoad()
    {
        DisplayConnections();
    }
    internal override void OnSave()
    {
        base.OnSave();
    }



    protected override void RegisterSandboxCalls()
    {
        if(!_data.isPublic)
            base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();
    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();
    }

    #endregion

    #region Connections
    internal override void AddConnection(Connection connection)
    {
        base.AddConnection(connection);

        if (connection.moduleA == this && !_data.connections.Contains(connection.moduleB.data.id))
            _data.connections.Add(connection.moduleB.data.id);
    }
    internal override void BreakConnection(Connection connection)
    {
        base.BreakConnection(connection);
        Module module = connection.moduleA == this ? connection.moduleB : connection.moduleA;
        _data.connections.Remove(module.data.id);

    }
    internal override void BreakConnectionTo(Module module)
    {
        base.BreakConnectionTo(module);
        _data.connections.Remove(module.data.id);


        if (connectedModules.Contains(module))
            connectedModules.Remove(module);
    }

    private void DisplayConnections()
    {
        foreach (int connectionId in _data.connections)
        {
            Module module = parent.GetModuleByIdRecursive(connectionId);
            parent.Connect(this, module, ConnectionType.Link);

            if (!connectedModules.Contains(module))
                connectedModules.Add(module);
        }
    }
    #endregion

    #region Logic
    internal void OnEditorInspect()
    {
    }



    internal void SerializeData(List<LoomNode> loomNodes)
    {
        List<LoomNodeData> loomNodeDatas = new List<LoomNodeData>();
        foreach (LoomNode loomNode in loomNodes)
            loomNodeDatas.Add(loomNode.Serialize());
    }


    private object GetEnumFromStrings(string enumType, string enumValue)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(enumType);
            if (type == null || !type.IsEnum)
                continue;
            return Enum.Parse(type, enumValue);
        }
        return null;
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
    #endregion
}