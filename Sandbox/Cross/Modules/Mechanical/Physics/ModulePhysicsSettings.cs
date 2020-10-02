using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModulePhysicsSettingsData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModulePhysicsSettingsData
            {
                name = "Physics Settings",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                usesGravity = true
            };
        }
    }

    public ModulePhysicsSettingsData() { }
    public ModulePhysicsSettingsData(ModulePhysicsSettingsData modulePhysicsSettingsData)
    {
        id = modulePhysicsSettingsData.id;
        parentId = modulePhysicsSettingsData.parentId;
        name = modulePhysicsSettingsData.name;
        position = modulePhysicsSettingsData.position;
        scale = modulePhysicsSettingsData.scale;
        rotation = modulePhysicsSettingsData.rotation;
        tags = modulePhysicsSettingsData.tags;

        friction = modulePhysicsSettingsData.friction;
        drag = modulePhysicsSettingsData.drag;
        angularDrag = modulePhysicsSettingsData.angularDrag;
        usesGravity = modulePhysicsSettingsData.usesGravity;

        freezeRotationX = modulePhysicsSettingsData.freezeRotationX;
        freezeRotationY = modulePhysicsSettingsData.freezeRotationY;
        freezeRotationZ = modulePhysicsSettingsData.freezeRotationZ;

        freezePositionX = modulePhysicsSettingsData.freezePositionX;
        freezePositionY = modulePhysicsSettingsData.freezePositionY;
        freezePositionZ = modulePhysicsSettingsData.freezePositionZ;
    }

    internal float friction;
    internal float drag;
    internal float angularDrag;
    internal bool usesGravity;

    internal bool freezeRotationX;
    internal bool freezeRotationY;
    internal bool freezeRotationZ;

    internal bool freezePositionX;
    internal bool freezePositionY;
    internal bool freezePositionZ;

    internal override ModuleData Clone()
    {
        return new ModulePhysicsSettingsData(this);
    }
}

internal sealed class ModulePhysicsSettings : Module
{
    private ModulePhysicsSettingsData _data = new ModulePhysicsSettingsData();
    internal override ModuleData data => _data;


    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModulePhysicsSettingsData((ModulePhysicsSettingsData)moduleData);
        ApplyData();
    }

    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();

        SandboxValue friction = new SandboxValue
        {
            module = this,
            id = "Friction",
            description = "The friction of the object against others.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.friction,
            set = (object obj) =>
            {
                _data.friction = (float)obj;
                ApplyData();
            },
            meta = new object[] {}
        };
        sandboxValuesById.Add(friction.id, friction);



        SandboxValue drag = new SandboxValue
        {
            module = this,
            id = "Drag",
            description = "The drag of the object.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.drag,
            set = (object obj) =>
            {
                _data.drag = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(drag.id, drag);

        SandboxValue angularDrag = new SandboxValue
        {
            module = this,
            id = "Angular Drag",
            description = "The angular drag of the object.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.angularDrag,
            set = (object obj) =>
            {
                _data.angularDrag = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(angularDrag.id, angularDrag);

        SandboxValue usesGravity = new SandboxValue
        {
            module = this,
            id = "Uses Gravity",
            description = "Determines if the object uses gravity or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.usesGravity,
            set = (object obj) =>
            {
                _data.usesGravity = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(usesGravity.id, usesGravity);



        SandboxValue freezeRotationX = new SandboxValue
        {
            module = this,
            id = "Freeze Rotation X",
            description = "Determines if the object is allowed to rotate around the x axis or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.freezeRotationX,
            set = (object obj) =>
            {
                _data.freezeRotationX = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(freezeRotationX.id, freezeRotationX);
        SandboxValue freezeRotationY = new SandboxValue
        {
            module = this,
            id = "Freeze Rotation Y",
            description = "Determines if the object is allowed to rotate around the y axis or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.freezeRotationY,
            set = (object obj) =>
            {
                _data.freezeRotationY = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(freezeRotationY.id, freezeRotationY);
        SandboxValue freezeRotationZ = new SandboxValue
        {
            module = this,
            id = "Freeze Rotation Z",
            description = "Determines if the object is allowed to rotate around the z axis or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.freezeRotationZ,
            set = (object obj) =>
            {
                _data.freezeRotationZ = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(freezeRotationZ.id, freezeRotationZ);


        SandboxValue freezePositionX = new SandboxValue
        {
            module = this,
            id = "Freeze Position X",
            description = "Determines if the object is allowed to move along the x axis or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.freezePositionX,
            set = (object obj) =>
            {
                _data.freezePositionX = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(freezePositionX.id, freezePositionX);
        SandboxValue freezePositionY = new SandboxValue
        {
            module = this,
            id = "Freeze Position Y",
            description = "Determines if the object is allowed to move along the y axis or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.freezePositionY,
            set = (object obj) =>
            {
                _data.freezePositionY = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(freezePositionY.id, freezePositionY);
        SandboxValue freezePositionZ = new SandboxValue
        {
            module = this,
            id = "Freeze Position Z",
            description = "Determines if the object is allowed to move along the z axis or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.freezePositionZ,
            set = (object obj) =>
            {
                _data.freezePositionZ = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(freezePositionZ.id, freezePositionZ);
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
}
