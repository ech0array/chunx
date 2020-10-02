using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
internal sealed class SandboxData : EditableData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new SandboxData();
        }
    }
    public SandboxData(){}
    public SandboxData(SandboxData sandboxData)
    {
        id = sandboxData.id;
        name = sandboxData.name;
        category = sandboxData.category;

        modules = new List<ModuleData>();
        foreach (ModuleData moduleData in sandboxData.modules)
            modules.Add(moduleData.Clone());
    }

    internal override ModuleData Clone()
    {
        SandboxData sandboxData = new SandboxData(this);
        sandboxData.id = GameHead.instance.universeData.GenerateUniqueId();
        return sandboxData;
    }
}



// Spawn, regulate spawn, cleanup, resets, pools
internal sealed class Sandbox : Editable
{
    #region Values
    private SandboxData _data = new SandboxData();
    internal override ModuleData data => _data;
    #endregion

    #region Functions
    internal override void PopulateData(ModuleData moduleData)
    {
        _data = (SandboxData)moduleData;
        SpawnModules(_data.modules);
        ApplyData();
        PostProcessModules();
    }
    private void PostProcessModules()
    {
        if (!GameHead.instance.isPreviewOrRuntime)
            return;

        ModulePhysicsSettings modulePhysicsSettings = null;
        int[] keys = _idModuleMap.Keys.ToArray();
        foreach (int key in keys)
        {
            Module module = _idModuleMap[key];
            if (module is ModuleArt)
            {
                MeshCollider meshCollider = module.GetComponent<MeshCollider>();
                if (meshCollider != null)
                    meshCollider.convex = false;
            }
            if (module.inspectionData.forceHideAtRuntime)
            {
                Renderer[] renderers = module.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    if (renderer is ParticleSystemRenderer)
                        continue;
                    renderer.enabled = false;
                }
            }
            if (module.inspectionData.forceNoCollisionAtRuntime)
            {
                Collider[] colliders = module.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                    collider.enabled = false;
            }
            if (module.inspectionData.category == ModuleCategory.Mechanical || module.inspectionData.category == ModuleCategory.Scripting || module.inspectionData.category == ModuleCategory.Environment)
            {
                Renderer[] renderers = module.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                    renderer.enabled = false;
                Collider[] colliders = module.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                    collider.enabled = false;
            }
        }
    }

    internal override bool OnAttach(User user)
    {
        int[] keys = _idModuleMap.Keys.ToArray();
        foreach (int key in keys)
        {
            Module module = _idModuleMap[key];
            if (module is SandboxObject)
            {
                bool attached = module.OnAttach(user);
                if (attached)
                    return true;
            }
        }
        return false;
    }
    #endregion
}
