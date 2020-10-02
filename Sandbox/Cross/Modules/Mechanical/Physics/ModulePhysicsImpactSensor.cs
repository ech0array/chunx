using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModulePhysicsImpactSensorData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModulePhysicsImpactSensorData
            {
                name = "Collision Sensor",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero
            };
        }
    }

    public ModulePhysicsImpactSensorData() { }
    public ModulePhysicsImpactSensorData(ModulePhysicsImpactSensorData modulePhysicsImpactSensorData)
    {
        id = modulePhysicsImpactSensorData.id;
        parentId = modulePhysicsImpactSensorData.parentId;
        name = modulePhysicsImpactSensorData.name;
        position = modulePhysicsImpactSensorData.position;
        scale = modulePhysicsImpactSensorData.scale;
        rotation = modulePhysicsImpactSensorData.rotation;
        tags = modulePhysicsImpactSensorData.tags;
    }

    internal override ModuleData Clone()
    {
        return new ModulePhysicsImpactSensorData(this);
    }
}

internal sealed class ModulePhysicsImpactSensor : Module
{
    private ModulePhysicsImpactSensorData _data = new ModulePhysicsImpactSensorData();
    internal override ModuleData data => _data;

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModulePhysicsImpactSensorData((ModulePhysicsImpactSensorData)moduleData);
        ApplyData();
    }

    new internal void OnCollisionEnter(Collision collision)
    {
        sandboxEventsById["On Collision Enter"].Invoke();
    }
    new internal void OnCollisionStay(Collision collision)
    {
        sandboxEventsById["On Collision Stay"].Invoke();
    }
    new internal void OnCollisionExit(Collision collision)
    {
        sandboxEventsById["On Collision Exit"].Invoke();
    }

    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();

        SandboxEvent onCollisionEnter = new SandboxEvent
        {
            module = this,
            id = "On Collision Enter"
        };
        sandboxEventsById.Add(onCollisionEnter.id, onCollisionEnter);

        SandboxEvent onCollisionStay = new SandboxEvent
        {
            module = this,
            id = "On Collision Stay"
        };
        sandboxEventsById.Add(onCollisionStay.id, onCollisionStay);


        SandboxEvent onCollisionExit = new SandboxEvent
        {
            module = this,
            id = "On Collision Exit"
        };
        sandboxEventsById.Add(onCollisionExit.id, onCollisionExit);
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
}
