using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleArtDecalData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtDecalData
            {
                name = "Decal",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                color = new SerializableColor(Color.white),
                decalTextureIndex = 0
            };
        }
    }
    public ModuleArtDecalData() { }
    public ModuleArtDecalData(ModuleArtDecalData cosmeticDecalData)
    {
        id = cosmeticDecalData.id;
        parentId = cosmeticDecalData.parentId;

        name = cosmeticDecalData.name;
        position = cosmeticDecalData.position;
        rotation = cosmeticDecalData.rotation;
        scale = cosmeticDecalData.scale;
        tags = cosmeticDecalData.tags;

        color = new SerializableColor(cosmeticDecalData.color.color);
        decalTextureIndex = cosmeticDecalData.decalTextureIndex;
    }


    internal SerializableColor color;
    internal int decalTextureIndex;

    internal override ModuleData Clone()
    {
        return new ModuleArtDecalData(this);
    }
}

internal sealed class ModuleArtDecal : ModuleArt
{
    internal ModuleArtDecalData _data = new ModuleArtDecalData();
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
        internal MeshRenderer meshRenderer;
        [SerializeField]
        internal MeshCollider meshCollider;
    }
    [SerializeField]
    private Components _components;


    internal override void PopulateData(ModuleData objectData)
    {
            _components.meshCollider = base.gameObject.AddComponent<MeshCollider>();
        _data = new ModuleArtDecalData((ModuleArtDecalData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();

        Texture2D texture2D = TextureHead.instance.GetDecal(_data.decalTextureIndex);
        _components.meshRenderer.sharedMaterial = MaterialHead.instance.GetDecalMaterial(texture2D, _data.color.color);
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
            description = "The color of the decal.",
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

        SandboxValue texture = new SandboxValue
        {
            module = this,
            id = "Image",
            description = "The image of the decal.",
            wuiValueType = typeof(WUITextureReferenceValue),
            get = () => _data.decalTextureIndex,
            set = (object obj) =>
            {
                _data.decalTextureIndex = (int)obj;
                ApplyData();
            },
            meta = new object[] { TextureSet.Decals}
        };
        sandboxValuesById.Add(texture.id, texture);
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