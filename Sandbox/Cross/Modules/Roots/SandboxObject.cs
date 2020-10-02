using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal sealed class SandboxObjectData : EditableData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new SandboxObjectData
            {
                name = "sandbox_object",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
            };
        }
    }

    public SandboxObjectData() { }
    public SandboxObjectData(SandboxObjectData sandboxObjectData)
    {
        id = sandboxObjectData.id;
        parentId = sandboxObjectData.parentId;

        name = sandboxObjectData.name;

        category = sandboxObjectData.category;
        position = sandboxObjectData.position;
        rotation = sandboxObjectData.rotation;
        scale = sandboxObjectData.scale;
        tags = sandboxObjectData.tags;

        modules = new List<ModuleData>();
        foreach (ModuleData moduleData in sandboxObjectData.modules)
            modules.Add(moduleData.Clone());
    }

    internal override ModuleData Clone()
    {
        SandboxObjectData sandboxObjectData = new SandboxObjectData(this);
        sandboxObjectData.id = GameHead.instance.universeData.GenerateUniqueId();
        return sandboxObjectData;
    }
}

internal sealed class SandboxObject : Editable
{
    #region Values
    private SandboxObjectData _data = new SandboxObjectData();
    internal override ModuleData data => _data;

    internal Rigidbody rigidbody;

    private class Components
    {
        internal ModulePhysicsImpactSensor modulePhysicsImpactSensor;
    }
    private Components _components = new Components();


    #endregion


    protected override void OnCollisionEnter(Collision collision)
    {
        if (_components.modulePhysicsImpactSensor != null)
            _components.modulePhysicsImpactSensor.OnCollisionEnter(collision);
    }
    protected override void OnCollisionStay(Collision collision)
    {
        if (_components.modulePhysicsImpactSensor != null)
            _components.modulePhysicsImpactSensor.OnCollisionStay(collision);
    }
    protected override void OnCollisionExit(Collision collision)
    {
        if (_components.modulePhysicsImpactSensor != null)
            _components.modulePhysicsImpactSensor.OnCollisionExit(collision);
    }

    #region Population
    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();
    }
    protected override void RegisterSandboxEvents()
    {
        // restrict events
    } 
    #endregion

    #region Construction
    internal override void PopulateData(ModuleData moduleData)
    {
        _data = (SandboxObjectData)moduleData;
        SpawnModules(_data.modules);
        ApplyData();
        PostProcessModules();
    }

    private void PostProcessModules()
    {
        if (!GameHead.instance.isPreviewOrRuntime)
            return;

        ModulePhysicsSettings modulePhysicsSettings = null;

        foreach (KeyValuePair<int, Module> entry in _idModuleMap)
        {
            Module module = entry.Value;

            {

                if (module is ModulePhysicsSettings)
                    modulePhysicsSettings = (ModulePhysicsSettings)module;

                if (rigidbody == null)
                {
                    if (module is ModulePhysicsController || module is ModulePhysicsVelocityApplier || module is ModulePhysicsSettings)
                    {
                        rigidbody = base.gameObject.AddComponent<Rigidbody>();
                        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    }
                }

                if (module is ModulePhysicsImpactSensor)
                    _components.modulePhysicsImpactSensor = (ModulePhysicsImpactSensor)module;

                if (module is ModuleArt)
                {
                    MeshCollider meshCollider = module.GetComponent<MeshCollider>();
                    if (meshCollider != null)
                        meshCollider.convex = true;
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

            if (modulePhysicsSettings != null)
            {
                ModulePhysicsSettingsData modulePhysicsSettingsData = (ModulePhysicsSettingsData)modulePhysicsSettings.data;

                rigidbody.drag = modulePhysicsSettingsData.drag;
                rigidbody.drag = modulePhysicsSettingsData.angularDrag;
                rigidbody.useGravity = modulePhysicsSettingsData.usesGravity;

                if (modulePhysicsSettingsData.freezeRotationX)
                    rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints.FreezeRotationX;
                if (modulePhysicsSettingsData.freezeRotationY)
                    rigidbody.constraints = rigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
                if (modulePhysicsSettingsData.freezeRotationZ)
                    rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints.FreezeRotationZ;

                if (modulePhysicsSettingsData.freezePositionX)
                    rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints.FreezePositionX;
                if (modulePhysicsSettingsData.freezePositionY)
                    rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints.FreezePositionY;
                if (modulePhysicsSettingsData.freezePositionZ)
                    rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints.FreezePositionZ;


                PhysicMaterial physicMaterial = new PhysicMaterial();
                physicMaterial.dynamicFriction = modulePhysicsSettingsData.friction;

                Collider[] colliders = base.gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                    collider.sharedMaterial = physicMaterial;
            }
        }
    }

    internal override bool OnAttach(User user)
    {
        bool hasAttached = false;
        int[] keys = _idModuleMap.Keys.ToArray();
        foreach (int key in keys)
        {
            Module module = _idModuleMap[key];
            bool attached = module.OnAttach(user);
            hasAttached = hasAttached || attached;
        }

        if (hasAttached)
        {
            this.user = user;
            this.user.attachement = this;
        }
        return hasAttached;
    }
    #endregion
}
