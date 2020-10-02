using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleArtCylinderData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtCylinderData
            {
                name = "Cylinder",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                radius = 1f,
                height = 1f,
                segments = 12,
                color = new SerializableColor(Color.white)
            };
        }
    }
    public ModuleArtCylinderData() { }
    public ModuleArtCylinderData(ModuleArtCylinderData cosmeticCylinderData)
    {
        id = cosmeticCylinderData.id;
        parentId = cosmeticCylinderData.parentId;

        name = cosmeticCylinderData.name;
        position = cosmeticCylinderData.position;
        rotation = cosmeticCylinderData.rotation;
        scale = cosmeticCylinderData.scale;
        tags = cosmeticCylinderData.tags;

        radius = cosmeticCylinderData.radius;
        height = cosmeticCylinderData.height;
        color = new SerializableColor(cosmeticCylinderData.color.color);
        segments = cosmeticCylinderData.segments;
    }


    internal SerializableColor color;

    internal float radius;
    internal float height;
    internal int segments;

    internal override ModuleData Clone()
    {
        return new ModuleArtCylinderData(this);
    }
}

internal sealed class ModuleArtCylinder : ModuleArt
{
    internal ModuleArtCylinderData _data = new ModuleArtCylinderData();
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
        _data = new ModuleArtCylinderData((ModuleArtCylinderData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();

        Mesh mesh = ProceduralMeshCone.GetMesh(_data.radius, _data.radius, _data.height / 2f, _data.segments);

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
            spaceAfter = 5
        };
        sandboxValuesById.Add(color.id, color);

        SandboxValue radius = new SandboxValue
        {
            module = this,
            id = "Radius",
            description = "The radius of the cylinder.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.radius,
            set = (object obj) =>
            {
                _data.radius = (float)obj;
                ApplyData();
            },
            header = "Shape Aspects",
            meta = new object[] { }
        };
        sandboxValuesById.Add(radius.id, radius);

        SandboxValue height = new SandboxValue
        {
            module = this,
            id = "Height",
            description = "The height of the cylinder.",
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
            description = "The segments of the cylinder.",
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