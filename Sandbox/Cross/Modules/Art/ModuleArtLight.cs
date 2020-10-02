
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum MinimalLightType
{
    Spot = 0,
    Omni = 2
}

[Serializable]
internal class ModuleArtLightData : ModuleData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtLightData
            {
                name = "Light",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                color = Color.white,
                range = 3f,
                intensity = 12f,
                lightType = MinimalLightType.Omni
            };
        }
    }

    internal MinimalLightType lightType;
    internal float intensity;

    internal float range;

    internal SerializableColor serializableColor;
    internal Color color
    {
        get
        {
            return new Color(serializableColor.r, serializableColor.g, serializableColor.b, serializableColor.a);
        }
        set
        {
            serializableColor = new SerializableColor(value);
        }
    }

    public ModuleArtLightData() { }
    public ModuleArtLightData(ModuleArtLightData moduleLightData)
    {
        id = moduleLightData.id;
        parentId = moduleLightData.parentId;

        name = moduleLightData.name;
        position = moduleLightData.position;
        rotation = moduleLightData.rotation;
        scale = moduleLightData.scale;
        tags = moduleLightData.tags;

        lightType = moduleLightData.lightType;
        intensity = moduleLightData.intensity;
        range = moduleLightData.range;
        color = moduleLightData.color;
    }

    internal override ModuleData Clone()
    {
        return new ModuleArtLightData(this);
    }
}

sealed internal class ModuleArtLight : Module
{
    #region Values
    private ModuleArtLightData _data = new ModuleArtLightData();
    internal override ModuleData data
    {
        get
        {
            return _data;
        }
    }

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Light light;
    }
    [SerializeField]
    private Components _components;
    #endregion

    #region Extending Functions
    internal override void PopulateData(ModuleData moduleData)
    {
        _data = new ModuleArtLightData((ModuleArtLightData)moduleData);
        ApplyData();
    }
    protected override void ApplyData()
    {
        base.ApplyData();
        _components.light.type = (LightType)_data.lightType;
        _components.light.color = _data.color;
        _components.light.intensity = _data.intensity;
        _components.light.range = _data.range;
    }


    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();
        SandboxValue lightTypeValueReference = new SandboxValue
        {
            module = this,
            id = "Type",
            description = "The type of the light.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.lightType,
            set = (object obj) => 
            {
                _data.lightType = (MinimalLightType)obj;
                ApplyData();
            },
            meta = new object[] { typeof(MinimalLightType) }
        };
        sandboxValuesById.Add(lightTypeValueReference.id, lightTypeValueReference);


        SandboxValue intensityValueReference = new SandboxValue
        {
            module = this,
            id = "Intensity",
            description = "The intensity of the light.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.intensity,
            set = (object obj) =>
            {
                _data.intensity = (float)obj;
                ApplyData();
            },
            meta = new object[] { false, 0f, 20f }
        };
        sandboxValuesById.Add(intensityValueReference.id, intensityValueReference);


        SandboxValue rangeValueReference = new SandboxValue
        {
            module = this,
            id = "Range",
            description = "The range of the light.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.range,
            set = (object obj) => 
            {
                _data.range = (float)obj;
                ApplyData();
            },
            meta = new object[] { false, 0f, 20f}
        };
        sandboxValuesById.Add(rangeValueReference.id, rangeValueReference);

        SandboxValue colorValueReference = new SandboxValue
        {
            module = this,
            id = "Color",
            description = "The color of the light.",
            wuiValueType = typeof(WUIColorValue),
            get = () => _data.color,
            set = (object obj) => 
            {
                _data.color = (Color)obj;
                ApplyData();
            }
        };
        sandboxValuesById.Add(colorValueReference.id, colorValueReference);
    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
    #endregion
}