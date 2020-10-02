using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleArtBoxData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtBoxData
            {
                name = "Box",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                width = 1f,
                depth = 1f,
                height = 1f,
                color = new SerializableColor(Color.white)
            };
        }
    }
    public ModuleArtBoxData() { }
    public ModuleArtBoxData(ModuleArtBoxData artBoxData)
    {
        id = artBoxData.id;
        parentId = artBoxData.parentId;

        name = artBoxData.name;
        position = artBoxData.position;
        rotation = artBoxData.rotation;
        scale = artBoxData.scale;
        tags = artBoxData.tags;

        width = artBoxData.width;
        depth = artBoxData.depth;
        height = artBoxData.height;
        color = new SerializableColor(artBoxData.color.color);
    }

    internal float width;
    internal float depth;
    internal float height;

    internal SerializableColor color;

    internal override ModuleData Clone()
    {
        return new ModuleArtBoxData(this);
    }
}

internal sealed class ModuleArtBox : ModuleArt
{
    internal ModuleArtBoxData _data = new ModuleArtBoxData();
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
        _data = new ModuleArtBoxData((ModuleArtBoxData)objectData);
        ApplyData();
    }
    protected override void ApplyData()
    {
        base.ApplyData();

        Mesh mesh = ProceduralMeshBox.GetMesh(new Vector3(_data.width, _data.height, _data.depth),
            new Vector3(-_data.width, _data.height, _data.depth),
            new Vector3(_data.width, -_data.height, _data.depth),
            new Vector3(-_data.width,- _data.height, _data.depth),
            new Vector3(_data.width, _data.height, -_data.depth),
            new Vector3(-_data.width, _data.height,-_data.depth),
            new Vector3(_data.width, -_data.height, -_data.depth),
            new Vector3(-_data.width, -_data.height, -_data.depth));


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

        SandboxValue width = new SandboxValue
        {
            module = this,
            id = "Width",
            description = "The width of the box.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.width,
            set = (object obj) =>
            {
                _data.width = (float)obj;
                ApplyData();
            },
            meta = new object[] { },
            header = "Shape Aspects"
        };
        sandboxValuesById.Add(width.id, width);

        SandboxValue depth = new SandboxValue
        {
            module = this,
            id = "Depth",
            description = "The depth of the box.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.depth,
            set = (object obj) =>
            {
                _data.depth = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(depth.id, depth);

        SandboxValue height = new SandboxValue
        {
            module = this,
            id = "Height",
            description = "The height of the box.",
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

    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}