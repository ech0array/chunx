using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleArtTubeData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtTubeData
            {
                name = "Tube",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                outerRadius = 1f,
                innerRadius = 0.5f,
                height = 1f,
                segments = 12,
                color = new SerializableColor(Color.white)
            };
        }
    }
    public ModuleArtTubeData() { }
    public ModuleArtTubeData(ModuleArtTubeData cosmeticTubeData)
    {
        id = cosmeticTubeData.id;
        parentId = cosmeticTubeData.parentId;

        name = cosmeticTubeData.name;
        position = cosmeticTubeData.position;
        rotation = cosmeticTubeData.rotation;
        scale = cosmeticTubeData.scale;
        tags = cosmeticTubeData.tags;

        outerRadius = cosmeticTubeData.outerRadius;
        innerRadius = cosmeticTubeData.innerRadius;
        height = cosmeticTubeData.height;
        segments = cosmeticTubeData.segments;
        color = new SerializableColor(cosmeticTubeData.color.color);
    }

    internal float outerRadius;
    internal float innerRadius;
    internal float height;
    internal int segments;

    internal SerializableColor color;

    internal override ModuleData Clone()
    {
        return new ModuleArtTubeData(this);
    }
}

internal sealed class ModuleArtTube : ModuleArt
{
    internal ModuleArtTubeData _data = new ModuleArtTubeData();
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
        internal MeshFilter meshFilter;
        [SerializeField]
        internal MeshRenderer meshRenderer;
        [SerializeField]
        internal MeshCollider meshCollider;
    }
    [SerializeField]
    private Components _components;

    private void Awake()
    {
    }

    internal override void PopulateData(ModuleData objectData)
    {
            _components.meshCollider = base.gameObject.AddComponent<MeshCollider>();
        _data = new ModuleArtTubeData((ModuleArtTubeData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();

        Mesh mesh = ProceduralMeshConeTube.GetMesh(_data.outerRadius, _data.outerRadius, _data.innerRadius, _data.innerRadius, _data.height, _data.height, _data.height, _data.height, _data.segments);
        _components.meshRenderer.sharedMaterial = MaterialHead.instance.GetMaterial(_data.color.color, MaterialType.basic);
        _components.meshFilter.sharedMesh = mesh;

            _components.meshCollider.sharedMesh = mesh;
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();
        SandboxValue color = new SandboxValue
        {
            module = this,
            id = "Color",
            description = "The color of the tube.",
            wuiValueType = typeof(WUIColorValue),
            get = () => _data.color.color,
            set = (object obj) =>
            {
                _data.color.color = (Color)obj;
                ApplyData();
            },
            meta = new object[] { },
            spaceAfter = 5f
        };
        sandboxValuesById.Add(color.id, color);

        SandboxValue outerRadius = new SandboxValue
        {
            module = this,
            id = "Outer Radius",
            description = "The outer radius of the tube.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.outerRadius,
            set = (object obj) =>
            {
                _data.outerRadius = (float)obj;
                ApplyData();
            },
            meta = new object[] { },
            header = "Shape Aspects"
        };
        sandboxValuesById.Add(outerRadius.id, outerRadius);

        SandboxValue innerRadius = new SandboxValue
        {
            module = this,
            id = "Inner Radius",
            description = "The inner radius of the tube.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.innerRadius,
            set = (object obj) =>
            {
                _data.innerRadius = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(innerRadius.id, innerRadius);

        SandboxValue height = new SandboxValue
        {
            module = this,
            id = "Height",
            description = "The height of the tube.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.height,
            set = (object obj) =>
            {
                _data.height = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(height.id, height);

        SandboxValue segments = new SandboxValue
        {
            module = this,
            id = "Segments",
            description = "The segments of the tube.",
            wuiValueType = typeof(WUIIntergerValue),
            get = () => _data.segments,
            set = (object obj) =>
            {
                _data.segments = (int)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(segments.id, segments);

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