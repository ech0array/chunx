using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleEnvironmentalCameraSettingsData : ModuleArtData
{

    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleEnvironmentalCameraSettingsData
            {
                name = "Camera Filter",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                brightness = 0f,
                saturation = 0f,
                contrast = 0f,
                lightenColor = new SerializableColor(Color.black),
                redLevel = 0f,
                greenLevel = 0f,
                blueLevel = 0f
            };
        }
    }
    public ModuleEnvironmentalCameraSettingsData() { }
    public ModuleEnvironmentalCameraSettingsData(ModuleEnvironmentalCameraSettingsData environmentalCameraSettingsData)
    {
        id = environmentalCameraSettingsData.id;
        parentId = environmentalCameraSettingsData.parentId;

        name = environmentalCameraSettingsData.name;
        position = environmentalCameraSettingsData.position;
        rotation = environmentalCameraSettingsData.rotation;
        scale = environmentalCameraSettingsData.scale;
        tags = environmentalCameraSettingsData.tags;


        brightness = environmentalCameraSettingsData.brightness;
        saturation = environmentalCameraSettingsData.saturation;
        contrast = environmentalCameraSettingsData.contrast;

        lightenColor = new SerializableColor(environmentalCameraSettingsData.lightenColor.color);

        redLevel = environmentalCameraSettingsData.redLevel;
        greenLevel = environmentalCameraSettingsData.greenLevel;
        blueLevel = environmentalCameraSettingsData.blueLevel;
    }
    
    internal float brightness = 0f;
    internal float saturation = 0f;
    internal float contrast = 0f;

    internal float redLevel;
    internal float greenLevel;
    internal float blueLevel;
    internal SerializableColor lightenColor;

    internal override ModuleData Clone()
    {
        return new ModuleEnvironmentalCameraSettingsData(this);
    }
}

internal sealed class ModuleEnvironmentalCameraSettings : ModuleArt
{
    internal ModuleEnvironmentalCameraSettingsData _data = new ModuleEnvironmentalCameraSettingsData();
    internal override ModuleData data
    {
        get
        {
            return _data;
        }
    }

    private void OnDestroy()
    {
        ControllableCamera.SetEffectAll(null);
    }

    internal override void PopulateData(ModuleData objectData)
    {
        _data = new ModuleEnvironmentalCameraSettingsData((ModuleEnvironmentalCameraSettingsData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();

        ControllableCamera.SetEffectAll(_data);
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        SandboxValue lightenColor = new SandboxValue
        {
            module = this,
            id = "Lighten Color",
            description = "The lightened color of the camera.",
            wuiValueType = typeof(WUIColorValue),
            get = () => _data.lightenColor.color,
            set = (object obj) =>
            {
                _data.lightenColor.color = (Color)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(lightenColor.id, lightenColor);

        SandboxValue brightness = new SandboxValue
        {
            module = this,
            id = "Brightness",
            description = "The brightness of the camera.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.brightness,
            set = (object obj) =>
            {
                _data.brightness = (float)obj;
                ApplyData();
            },
            meta = new object[] {false, -2f, 2f }
        };
        sandboxValuesById.Add(brightness.id, brightness);

        SandboxValue saturation = new SandboxValue
        {
            module = this,
            id = "Saturation",
            description = "The saturation of the camera.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.saturation,
            set = (object obj) =>
            {
                _data.saturation = (float)obj;
                ApplyData();
            },
            meta = new object[] { false,-2f, 2f }
        };
        sandboxValuesById.Add(saturation.id, saturation);

        SandboxValue contrast = new SandboxValue
        {
            module = this,
            id = "Contrast",
            description = "The contrast of the camera.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.contrast,
            set = (object obj) =>
            {
                _data.contrast = (float)obj;
                ApplyData();
            },
            meta = new object[] { false,-2f, 2f }
        };
        sandboxValuesById.Add(contrast.id, contrast);

        SandboxValue redLevel = new SandboxValue
        {
            module = this,
            id = "Red Level",
            description = "The red level of the camera.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.redLevel,
            set = (object obj) =>
            {
                _data.redLevel = (float)obj;
                ApplyData();
            },
            meta = new object[] { false, -2f, 2f }
        };
        sandboxValuesById.Add(redLevel.id, redLevel);

        SandboxValue greenLevel = new SandboxValue
        {
            module = this,
            id = "Green Level",
            description = "The green level of the camera.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.greenLevel,
            set = (object obj) =>
            {
                _data.greenLevel = (float)obj;
                ApplyData();
            },
            meta = new object[] { false, -2f, 2f }
        };
        sandboxValuesById.Add(greenLevel.id, greenLevel);

        SandboxValue blueLevel = new SandboxValue
        {
            module = this,
            id = "Blue Level",
            description = "The blue level of the camera.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.blueLevel,
            set = (object obj) =>
            {
                _data.blueLevel = (float)obj;
                ApplyData();
            },
            meta = new object[] { false, -2f, 2f }
        };
        sandboxValuesById.Add(blueLevel.id, blueLevel);
    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();
    }

   

    internal override bool OnAttach(User user)
    {
        return false;
    }
}