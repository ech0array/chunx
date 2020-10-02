using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleTransformRotationControllerData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleTransformRotationControllerData
            {
                name = "Transform Rotation Controller",

                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,

                yawRate = 100f,
                yawMin = -360f,
                yawMax = 360f,

                pitchRate = 100f,
                pitchMin = -89f,
                pitchMax = 89f
            };
        }
    }

    public ModuleTransformRotationControllerData() { }
    public ModuleTransformRotationControllerData(ModuleTransformRotationControllerData moduleTransformRotationControllerData)
    {
        id = moduleTransformRotationControllerData.id;
        parentId = moduleTransformRotationControllerData.parentId;
        name = moduleTransformRotationControllerData.name;
        position = moduleTransformRotationControllerData.position;
        scale = moduleTransformRotationControllerData.scale;
        rotation = moduleTransformRotationControllerData.rotation;
        tags = moduleTransformRotationControllerData.tags;


        yawRate = moduleTransformRotationControllerData.yawRate;
        yawMin = moduleTransformRotationControllerData.yawMin;
        yawMax = moduleTransformRotationControllerData.yawMax;

        pitchRate = moduleTransformRotationControllerData.pitchRate;
        pitchMin = moduleTransformRotationControllerData.pitchMin;
        pitchMax = moduleTransformRotationControllerData.pitchMax;
    }

    internal float yawRate;
    internal float yawMin;
    internal float yawMax;

    internal float pitchRate;
    internal float pitchMin;
    internal float pitchMax;

    internal override ModuleData Clone()
    {
        return new ModuleTransformRotationControllerData(this);
    }
}


internal sealed class ModuleTransformRotationController : Module
{
    private ModuleTransformRotationControllerData _data = new ModuleTransformRotationControllerData();
    internal override ModuleData data => _data;


    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModuleTransformRotationControllerData((ModuleTransformRotationControllerData)moduleData);
        ApplyData();
    }

    protected override void UpdateRuntime()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (user == null)
            return;
        if (!user.allowRuntimeInput)
            return;

        Vector3 rotation = parent.transform.localEulerAngles;

        rotation.y += _data.yawRate * user.input.look.x * Time.deltaTime;
        rotation.y = ClampAngle(rotation.y, new Vector2(_data.yawMin, _data.yawMax));
        if (_data.pitchRate != 0 && (_data.pitchMin != 0 && _data.pitchMax != 0))
        {
            rotation.x += _data.pitchRate * user.input.look.y * Time.deltaTime;
            rotation.x = ClampAngle(rotation.x, new Vector2(_data.pitchMin, _data.pitchMax));
        }
        parent.transform.localEulerAngles = rotation;
    }

    private float ClampAngle(float angle, Vector2 minMax)
    {
        if (angle > 180)
            angle = -(360 - angle);

        return Mathf.Clamp(angle, minMax.x, minMax.y);
    }


    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();

        SandboxValue yawRate = new SandboxValue
        {
            module = this,
            id = "Yaw Rate",
            description = "The rate at which the module rotates along the yaw component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.yawRate,
            set = (object obj) =>
            {
                _data.yawRate = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(yawRate.id, yawRate);

        SandboxValue yawMin = new SandboxValue
        {
            module = this,
            id = "Yaw Min",
            description = "The minimum angle long the yaw component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.yawMin,
            set = (object obj) =>
            {
                _data.yawMin = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(yawMin.id, yawMin);
        SandboxValue yawMax = new SandboxValue
        {
            module = this,
            id = "Yaw Max",
            description = "The maximum angle long the yaw component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.yawMax,
            set = (object obj) =>
            {
                _data.yawMax = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(yawMax.id, yawMax);

        SandboxValue pitchRate = new SandboxValue
        {
            module = this,
            id = "Pitch Rate",
            description = "The rate at which the module rotates along the pitch component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.pitchRate,
            set = (object obj) =>
            {
                _data.pitchRate = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(pitchRate.id, pitchRate);
        SandboxValue pitchMin = new SandboxValue
        {
            module = this,
            id = "Pitch Min",
            description = "The minimum angle long the pitch component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.pitchMin,
            set = (object obj) =>
            {
                _data.pitchMin = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(pitchMin.id, pitchMin);
        SandboxValue pitchMax = new SandboxValue
        {
            module = this,
            id = "Pitch Max",
            description = "The maximum angle long the pitch component.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.pitchMax,
            set = (object obj) =>
            {
                _data.pitchMax = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(pitchMax.id, pitchMax);
    }

    internal override bool OnAttach(User user)
    {
        this.user = user;
        return false;
    }
}
