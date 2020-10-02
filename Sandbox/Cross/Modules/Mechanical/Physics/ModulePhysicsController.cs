using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum InputStick
{
    Left,
    Right
}
internal enum VelocityDirection
{
    Local,
    Camera,
    CameraGrounded
}
internal enum ApplicationMethod
{
    Set,
    Add
}

[Serializable]
internal sealed class ModulePhysicsControllerData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModulePhysicsControllerData
            {
                name = "Physics Controller",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                inputStick = InputStick.Left,
                forwardMovementSpeed = 100f,
                sidewaysMovementSpeed = 75f,
                velocityDirection = VelocityDirection.CameraGrounded,
                applicationMethod = ApplicationMethod.Add
            };
        }
    }
    public ModulePhysicsControllerData() { }

    public ModulePhysicsControllerData(ModulePhysicsControllerData modulePhysicsControllerData)
    {
        id = modulePhysicsControllerData.id;
        parentId = modulePhysicsControllerData.parentId;
        name = modulePhysicsControllerData.name;
        position = modulePhysicsControllerData.position;
        scale = modulePhysicsControllerData.scale;
        rotation = modulePhysicsControllerData.rotation;
        tags = modulePhysicsControllerData.tags;
        inputStick = modulePhysicsControllerData.inputStick;
        forwardMovementSpeed = modulePhysicsControllerData.forwardMovementSpeed;
        sidewaysMovementSpeed = modulePhysicsControllerData.sidewaysMovementSpeed;
        velocityDirection = modulePhysicsControllerData.velocityDirection;
        applicationMethod = modulePhysicsControllerData.applicationMethod;
    }

    internal InputStick inputStick;
    internal VelocityDirection velocityDirection;
    internal ApplicationMethod applicationMethod;
    internal float forwardMovementSpeed;
    internal float sidewaysMovementSpeed;

    internal override ModuleData Clone()
    {
        return new ModulePhysicsControllerData(this);
    }
}

internal sealed class ModulePhysicsController : Module
{
    private ModulePhysicsControllerData _data = new ModulePhysicsControllerData();
    internal override ModuleData data => _data;

    private void FixedUpdate()
    {
        if (user?.allowRuntimeInput == false)
            return;

        if(user != null)
            UpdateMovement(_data.inputStick == InputStick.Right ? user.input.look : user.input.move);
    }

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModulePhysicsControllerData((ModulePhysicsControllerData)moduleData);
        ApplyData();
    }

    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();

        SandboxValue inputStick = new SandboxValue
        {
            module = this,
            id = "Input Stick",
            description = "The input stick that controls the velocity.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.inputStick,
            set = (object obj) =>
            {
                _data.inputStick = (InputStick)obj;
                ApplyData();
            },
            meta = new object[] { typeof(InputStick) }
        };
        sandboxValuesById.Add(inputStick.id, inputStick);


        SandboxValue velocityDirection = new SandboxValue
        {
            module = this,
            id = "Velocity Direction",
            description = "The direction along the object at-which the velocity is applied.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.velocityDirection,
            set = (object obj) =>
            {
                _data.velocityDirection = (VelocityDirection)obj;
                ApplyData();
            },
            meta = new object[] { typeof(VelocityDirection) }
        };
        sandboxValuesById.Add(velocityDirection.id, velocityDirection);


        SandboxValue applicationMethod = new SandboxValue
        {
            module = this,
            id = "Application Method",
            description = "The method at-which the velcoity is applied.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.applicationMethod,
            set = (object obj) =>
            {
                _data.applicationMethod = (ApplicationMethod)obj;
                ApplyData();
            },
            meta = new object[] { typeof(ApplicationMethod) }
        };
        sandboxValuesById.Add(applicationMethod.id, applicationMethod);


        SandboxValue forwardMovementSpeed = new SandboxValue
        {
            module = this,
            id = "Forward Movement Speed",
            description = "The speed at which the object will move along the z component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.forwardMovementSpeed,
            set = (object obj) =>
            {
                _data.forwardMovementSpeed = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(forwardMovementSpeed.id, forwardMovementSpeed);


        SandboxValue sidewaysMovementSpeed = new SandboxValue
        {
            module = this,
            id = "Sideways Movement Speed",
            description = "The speed at which the object will move along the x component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.sidewaysMovementSpeed,
            set = (object obj) =>
            {
                _data.sidewaysMovementSpeed = (float)obj;
                ApplyData();
            },
            meta = new object[] {}
        };
        sandboxValuesById.Add(sidewaysMovementSpeed.id, sidewaysMovementSpeed);
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }

    private void UpdateMovement(Vector2 value)
    {
        SandboxObject parentSandboxObject = ((SandboxObject)parent);
        Vector3 scaledDirection = Vector3.zero;
        if (_data.velocityDirection == VelocityDirection.Local)
        {
            scaledDirection = base.transform.forward * value.y * _data.forwardMovementSpeed;
            scaledDirection += base.transform.right * value.x * _data.sidewaysMovementSpeed;
        }
        else if (_data.velocityDirection == VelocityDirection.Camera)
        {
            scaledDirection = parentSandboxObject.user.controllableCamera.transform.forward * value.y * _data.forwardMovementSpeed;
            scaledDirection += parentSandboxObject.user.controllableCamera.transform.right * value.x * _data.sidewaysMovementSpeed;
        }
        else
        {
            Vector3 forward = Quaternion.Euler(0, parentSandboxObject.user.controllableCamera.transform.eulerAngles.y, parentSandboxObject.user.controllableCamera.transform.eulerAngles.z) * Vector3.forward;
            scaledDirection = forward * value.y * _data.forwardMovementSpeed;
            scaledDirection += parentSandboxObject.user.controllableCamera.transform.right * value.x * _data.sidewaysMovementSpeed;
        }

        if(_data.applicationMethod == ApplicationMethod.Add)
            parentSandboxObject.rigidbody.velocity += scaledDirection * Time.fixedDeltaTime;
        else
        {
            parentSandboxObject.rigidbody.velocity = scaledDirection * Time.fixedDeltaTime;
        }

    }

    internal override bool OnAttach(User user)
    {
        this.user = user;
        return true;
    }
}
