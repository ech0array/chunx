using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleArtConeData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtConeData
            {
                name = "Cone",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                topRadius = 0.25f,
                bottomRadius = 1f,
                height = 1f,
                segments = 12,
                color = new SerializableColor(Color.white)
            };
        }
    }
    public ModuleArtConeData() { }
    public ModuleArtConeData(ModuleArtConeData cosmeticConeData)
    {
        id = cosmeticConeData.id;
        parentId = cosmeticConeData.parentId;

        name = cosmeticConeData.name;
        position = cosmeticConeData.position;
        rotation = cosmeticConeData.rotation;
        scale = cosmeticConeData.scale;
        tags = cosmeticConeData.tags;

        topRadius = cosmeticConeData.topRadius;
        bottomRadius = cosmeticConeData.bottomRadius;
        height = cosmeticConeData.height;
        color = new SerializableColor(cosmeticConeData.color.color);
        segments = cosmeticConeData.segments;
    }


    internal SerializableColor color;

    internal float topRadius;
    internal float bottomRadius;
    internal float height;
    internal int segments;

    internal override ModuleData Clone()
    {
        return new ModuleArtConeData(this);
    }
}

internal sealed class ModuleArtCone : ModuleArt
{
    internal ModuleArtConeData _data = new ModuleArtConeData();
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
        _data = new ModuleArtConeData((ModuleArtConeData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();

        Mesh mesh = ProceduralMeshCone.GetMesh(_data.topRadius, _data.bottomRadius, _data.height / 2f, _data.segments);

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

        SandboxValue topRadius = new SandboxValue
        {
            module = this,
            id = "Top Radius",
            description = "The radius of the top of the cone.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.topRadius,
            set = (object obj) =>
            {
                _data.topRadius = (float)obj;
                ApplyData();
            },
            meta = new object[] { },
            header = "Shape Aspects"
        };
        sandboxValuesById.Add(topRadius.id, topRadius);

        SandboxValue bottomRadius = new SandboxValue
        {
            module = this,
            id = "Bottom Radius",
            description = "The radius of the bottom of the cone.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.bottomRadius,
            set = (object obj) =>
            {
                _data.bottomRadius = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(bottomRadius.id, bottomRadius);

        SandboxValue height = new SandboxValue
        {
            module = this,
            id = "Height",
            description = "The height of the cone.",
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
            description = "The segments of the cone.",
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