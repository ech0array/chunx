using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleEnvironmentalSunData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleEnvironmentalSunData
            {
                name = "Sun",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,

                color = new SerializableColor(Color.white),
                shadowStrength = 0.5f
            };
        }
    }
    public ModuleEnvironmentalSunData() { }
    public ModuleEnvironmentalSunData(ModuleEnvironmentalSunData environmentalSunData)
    {
        id = environmentalSunData.id;
        parentId = environmentalSunData.parentId;

        name = environmentalSunData.name;
        position = environmentalSunData.position;
        rotation = environmentalSunData.rotation;
        scale = environmentalSunData.scale;
        tags = environmentalSunData.tags;

        color = environmentalSunData.color;
        shadowStrength = environmentalSunData.shadowStrength;
        serializableGradient = new SerializableGradient(environmentalSunData.serializableGradient);
        serializableCurve = new SerializableCurve(environmentalSunData.serializableCurve);
    }


    internal SerializableColor color;
    internal float shadowStrength;
    internal SerializableGradient serializableGradient;
    internal SerializableCurve serializableCurve;

    internal override ModuleData Clone()
    {
        return new ModuleEnvironmentalSunData(this);
    }
}

internal sealed class ModuleEnvironmentalSun : ModuleArt
{
    internal ModuleEnvironmentalSunData _data = new ModuleEnvironmentalSunData();
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

    private void OnDestroy()
    {
        GameHead.instance.SetSunState(true);
    }

    internal override void PopulateData(ModuleData objectData)
    {
        _data = new ModuleEnvironmentalSunData((ModuleEnvironmentalSunData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();
        GameHead.instance.SetSunState(false);
        _components.light.color = _data.color.color;
        _components.light.shadowStrength = _data.shadowStrength;

    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        SandboxValue color = new SandboxValue
        {
            module = this,
            id = "Color",
            description = "The color of the sun.",
            wuiValueType = typeof(WUIColorValue),
            get = () => _data.color.color,
            set = (object obj) =>
            {
                _data.color.color = (Color)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(color.id, color);

        SandboxValue shadowStrength = new SandboxValue
        {
            module = this,
            id = "Shadow Strength",
            description = "The strength of the suns shadows.",
            wuiValueType = typeof(WUIRangeValue),
            get = () => _data.shadowStrength,
            set = (object obj) =>
            {
                _data.shadowStrength = (float)obj;
                ApplyData();
            },
            meta = new object[] {false, 0f, 1f }
        };
        sandboxValuesById.Add(shadowStrength.id, shadowStrength);

        SandboxValue testGradient = new SandboxValue
        {
            module = this,
            id = "Test Gradient",
            description = "---",
            wuiValueType = typeof(WUIGradientValue),
            get = () => _data.serializableGradient,
            set = (object obj) =>
            {
                _data.serializableGradient = (SerializableGradient)obj;
                ApplyData();
            },
            meta = new object[] {}
        };
        sandboxValuesById.Add(testGradient.id, testGradient);

        SandboxValue testCurve = new SandboxValue
        {
            module = this,
            id = "Test Curve",
            description = "---",
            wuiValueType = typeof(WUICurveValue),
            get = () => _data.serializableCurve,
            set = (object obj) =>
            {
                _data.serializableCurve = (SerializableCurve)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(testCurve.id, testCurve);
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