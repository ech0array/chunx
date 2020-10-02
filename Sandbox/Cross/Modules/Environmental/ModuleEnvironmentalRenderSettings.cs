using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleEnvironmentalRenderSettingsData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleEnvironmentalRenderSettingsData
            {
                name = "Render Settings",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,

                fogColor = new SerializableColor(Color.white),
                skyColor = new SerializableColor(Color.white),
                fogDensity = 0.01f
            };
        }
    }
    public ModuleEnvironmentalRenderSettingsData() { }
    public ModuleEnvironmentalRenderSettingsData(ModuleEnvironmentalRenderSettingsData environmentalRenderSettingsData)
    {
        id = environmentalRenderSettingsData.id;
        parentId = environmentalRenderSettingsData.parentId;

        name = environmentalRenderSettingsData.name;
        position = environmentalRenderSettingsData.position;
        rotation = environmentalRenderSettingsData.rotation;
        scale = environmentalRenderSettingsData.scale;
        tags = environmentalRenderSettingsData.tags;

        fogColor = environmentalRenderSettingsData.fogColor;
        skyColor = environmentalRenderSettingsData.skyColor;
        fogDensity = environmentalRenderSettingsData.fogDensity;
    }


    internal SerializableColor fogColor;
    internal SerializableColor skyColor;
    internal float fogDensity;

    internal override ModuleData Clone()
    {
        return new ModuleEnvironmentalRenderSettingsData(this);
    }
}

internal sealed class ModuleEnvironmentalRenderSettings : ModuleArt
{
    internal ModuleEnvironmentalRenderSettingsData _data = new ModuleEnvironmentalRenderSettingsData();
    internal override ModuleData data
    {
        get
        {
            return _data;
        }
    }


    private void OnDestroy()
    {
        ControllableCamera.RestoreAllColor();
        RenderSettings.fog = false;
    }

    internal override void PopulateData(ModuleData objectData)
    {
        _data = new ModuleEnvironmentalRenderSettingsData((ModuleEnvironmentalRenderSettingsData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();
        ControllableCamera.SetAllColor(_data.skyColor.color);
        RenderSettings.fogColor = _data.fogColor.color;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = _data.fogDensity;
        RenderSettings.fog = true;
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        SandboxValue skyColor = new SandboxValue
        {
            module = this,
            id = "Sky Color",
            description = "The color of environments sky.",
            wuiValueType = typeof(WUIColorValue),
            get = () => _data.skyColor.color,
            set = (object obj) =>
            {
                _data.skyColor.color = (Color)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(skyColor.id, skyColor);

        SandboxValue fogColor = new SandboxValue
        {
            module = this,
            id = "Fog Color",
            description = "The color of environments fog.",
            wuiValueType = typeof(WUIColorValue),
            get = () => _data.fogColor.color,
            set = (object obj) =>
            {
                _data.fogColor.color = (Color)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(fogColor.id, fogColor);

        SandboxValue fogDensity = new SandboxValue
        {
            module = this,
            id = "Fog Density",
            description = "The density of environments fog.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.fogDensity,
            set = (object obj) =>
            {
                _data.fogDensity = (float)obj;
                ApplyData();
            },
            meta = new object[] {false, 0f, 1f }
        };
        sandboxValuesById.Add(fogDensity.id, fogDensity);
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