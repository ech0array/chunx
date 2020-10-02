using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModulePhysicsVelocityApplierData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModulePhysicsVelocityApplierData
            {
                name = "Physics Velocity Applier",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero
            };
        }
    }

    public ModulePhysicsVelocityApplierData() { }
    public ModulePhysicsVelocityApplierData(ModulePhysicsVelocityApplierData modulePhysicsVelocityApplierData)
    {
        id = modulePhysicsVelocityApplierData.id;
        parentId = modulePhysicsVelocityApplierData.parentId;
        name = modulePhysicsVelocityApplierData.name;
        position = modulePhysicsVelocityApplierData.position;
        scale = modulePhysicsVelocityApplierData.scale;
        rotation = modulePhysicsVelocityApplierData.rotation;
        tags = modulePhysicsVelocityApplierData.tags;

        applicationType = modulePhysicsVelocityApplierData.applicationType;
        setType = modulePhysicsVelocityApplierData.setType;
        velocityX = modulePhysicsVelocityApplierData.velocityX;
        velocityY = modulePhysicsVelocityApplierData.velocityY;
        velocityZ = modulePhysicsVelocityApplierData.velocityZ;
    }

    internal ApplicationType applicationType;
    internal SetType setType;
    internal float velocityX;
    internal float velocityY;
    internal float velocityZ;

    internal override ModuleData Clone()
    {
        return new ModulePhysicsVelocityApplierData(this);
    }
}

internal enum ApplicationType
{
    Constant,
    Initial,
    Called
}
internal enum SetType
{
    Set,
    Add
}

internal sealed class ModulePhysicsVelocityApplier : Module
{
    private ModulePhysicsVelocityApplierData _data = new ModulePhysicsVelocityApplierData();
    internal override ModuleData data => _data;

    protected override void StartRuntime()
    {
        if (_data.applicationType == ApplicationType.Initial)
            ApplyVelocity();
    }

    protected override void FixedUpdateRuntime()
    {
        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        if (_data.applicationType != ApplicationType.Constant)
            return;
        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        if (!GameHead.instance.isPreviewOrRuntime)
            return;
        SandboxObject sandboxObject = (SandboxObject)parent;
        Vector3 velocity = new Vector3(_data.velocityX, _data.velocityY, _data.velocityZ) * Time.fixedDeltaTime;
        if (_data.setType == SetType.Add)
            sandboxObject.rigidbody.AddForce(velocity, ForceMode.Acceleration);
        if (_data.setType == SetType.Set)
            sandboxObject.rigidbody.velocity = velocity;
    }

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModulePhysicsVelocityApplierData((ModulePhysicsVelocityApplierData)moduleData);
        ApplyData();
    }

    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();

        SandboxValue applicationType = new SandboxValue
        {
            module = this,
            id = "Application Type",
            description = "Determines when the velocity will be applied.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.applicationType,
            set = (object obj) =>
            {
                _data.applicationType = (ApplicationType)obj;
                ApplyData();
            },
            meta = new object[] {typeof(ApplicationType) }
        };
        sandboxValuesById.Add(applicationType.id, applicationType);



        SandboxValue setType = new SandboxValue
        {
            module = this,
            id = "Set Type",
            description = "Determines how the velocity will be applied.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.setType,
            set = (object obj) =>
            {
                _data.setType = (SetType)obj;
                ApplyData();
            },
            meta = new object[] { typeof(SetType) }
        };
        sandboxValuesById.Add(setType.id, setType);

        SandboxValue velocityX = new SandboxValue
        {
            module = this,
            id = "Velocity X",
            description = "The velocity applied to the x axis of the object.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.velocityX,
            set = (object obj) =>
            {
                _data.velocityX = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(velocityX.id, velocityX);

        SandboxValue velocityY = new SandboxValue
        {
            module = this,
            id = "Velocity Y",
            description = "The velocity applied to the y axis of the object.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.velocityY,
            set = (object obj) =>
            {
                _data.velocityY = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(velocityY.id, velocityY);
        SandboxValue velocityZ = new SandboxValue
        {
            module = this,
            id = "Velocity Z",
            description = "The velocity applied to the z axis of the object.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.velocityZ,
            set = (object obj) =>
            {
                _data.velocityZ = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(velocityZ.id, velocityZ);
    }
    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();

        SandboxCall applyVelocity = new SandboxCall
        {
            module = this,
            id = "Apply Velocity",
            description = "Applies velocity to the object.",
            action = (obj) => { ApplyVelocity(); }
        };
        sandboxCallsById.Add(applyVelocity.id, applyVelocity);
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
}
