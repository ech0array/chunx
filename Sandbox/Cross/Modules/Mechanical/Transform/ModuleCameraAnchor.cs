using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleCameraAnchorData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleCameraAnchorData
            {
                name = "Camera Anchor",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                offset = 0f,

                yawRate = 100f,
                yawMin = -360f,
                yawMax = 360f,

                pitchRate = 100f,
                pitchMin = -89f,
                pitchMax = 89f
            };
        }
    }
    public ModuleCameraAnchorData() { }
    public ModuleCameraAnchorData(ModuleCameraAnchorData moduleCameraAnchorData)
    {
        name = moduleCameraAnchorData.name;
        id = moduleCameraAnchorData.id;
        parentId = moduleCameraAnchorData.parentId;

        position = moduleCameraAnchorData.position;
        scale = moduleCameraAnchorData.scale;
        rotation = moduleCameraAnchorData.rotation;
        tags = moduleCameraAnchorData.tags;

        offset = moduleCameraAnchorData.offset;

        yawRate = moduleCameraAnchorData.yawRate;
        yawMin = moduleCameraAnchorData.yawMin;
        yawMax = moduleCameraAnchorData.yawMax;

        pitchRate = moduleCameraAnchorData.pitchRate;
        pitchMin = moduleCameraAnchorData.pitchMin;
        pitchMax = moduleCameraAnchorData.pitchMax;
    }

    internal float offset;

    internal float yawRate;
    internal float yawMin;
    internal float yawMax;

    internal float pitchRate;
    internal float pitchMin;
    internal float pitchMax;

    internal override ModuleData Clone()
    {
        return new ModuleCameraAnchorData(this);
    }
}

internal sealed class ModuleCameraAnchor : Module
{
    private ModuleCameraAnchorData _data = new ModuleCameraAnchorData();
    internal override ModuleData data => _data;

    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModuleCameraAnchorData((ModuleCameraAnchorData)moduleData);
        ApplyData();
    }

    protected override void LateUpdateRuntime()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (user == null)
            return;
        if (!user.allowRuntimeInput)
            return;

        Vector3 rotation = base.transform.localEulerAngles;

        rotation.y += _data.yawRate * user.input.look.x * Time.deltaTime;
        rotation.y = ClampAngle(rotation.y, new Vector2(_data.yawMin, _data.yawMax));

        rotation.x += _data.pitchRate * user.input.look.y * Time.deltaTime;
        rotation.x = ClampAngle(rotation.x, new Vector2(_data.pitchMin, _data.pitchMax));

        base.transform.localEulerAngles = rotation;


        user.controllableCamera.transform.rotation = base.transform.rotation;
        user.controllableCamera.transform.position = base.transform.position + (base.transform.forward * -_data.offset);
    }

    private float ClampAngle(float angle, Vector2 minMax)
    {
        if (angle > 180)
            angle = -(360 - angle);

        return Mathf.Clamp(angle, minMax.x, minMax.y);
    }

    internal override bool OnAttach(User user)
    {
        this.user = user;
        return true;
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();

        SandboxValue offset = new SandboxValue
        {
            module = this,
            id = "Offset",
            description = "The offset of the camera from this object.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.offset,
            set = (object obj) =>
            {
                _data.offset = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(offset.id, offset);

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
}
