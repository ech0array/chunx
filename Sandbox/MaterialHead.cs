using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


internal enum MaterialType
{
    basic,
    transparent
}
internal sealed class MaterialHead : SingleMonoBehaviour<MaterialHead>
{
    protected override bool isPersistant => true;

    [Serializable]
    private class Materials
    {
        [SerializeField]
        internal Material effectMaterial;
        internal Dictionary<Texture2D, Material> effectMaterialMap = new Dictionary<Texture2D, Material>();

        [SerializeField]
        internal Material basicMaterial;
        internal Dictionary<Color, Material> basicColorMaterialMap = new Dictionary<Color, Material>();

        [SerializeField]
        internal Material transparentMaterial;
        internal Dictionary<Color, Material> transparentColorMaterialMap = new Dictionary<Color, Material>();

        [SerializeField]
        internal Material decalMaterial;
        internal Dictionary<(Texture2D, Color), Material> decalMaterialMap = new Dictionary<(Texture2D, Color), Material>();
    }
    [SerializeField]
    private Materials _materials;

    internal Material GetMaterial(Color color, MaterialType materialType)
    {
        Dictionary<Color, Material> map = GetMaterialMap(materialType);
        if (map.ContainsKey(color))
            return map[color];
        return GetNewMaterial(color, map);
    }

    private Material GetNewMaterial(Color color, Dictionary<Color, Material> map)
    {
        Material material = new Material(_materials.basicMaterial);
        map.Add(color, material);
        material.SetColor("_Color", color);
        return material;
    }

    private Dictionary<Color, Material> GetMaterialMap(MaterialType materialType)
    {
        switch (materialType)
        {
            case MaterialType.basic:
                return _materials.basicColorMaterialMap;
            case MaterialType.transparent:
                return _materials.transparentColorMaterialMap;
        }
        return null;
    }


    internal Material GetDecalMaterial(Texture2D texture2D, Color color)
    {

        if (_materials.decalMaterialMap.ContainsKey((texture2D, color)))
            return _materials.decalMaterialMap[(texture2D, color)];

        return GetNewDecalMaterial(texture2D, color);
    }

    internal Material GetNewDecalMaterial(Texture2D texture2D, Color color)
    {
        Material material = new Material(_materials.decalMaterial);
        _materials.decalMaterialMap.Add((texture2D, color), material);
        material.SetColor("_Color", color);
        material.SetTexture("_MainTex", texture2D);
        return material;
    }

    internal Material GetEffectMaterial(Texture2D texture2D)
    {

        if (_materials.effectMaterialMap.ContainsKey(texture2D))
            return _materials.effectMaterialMap[texture2D];

        return GetNewEffectMaterial(texture2D);
    }
    internal Material GetNewEffectMaterial(Texture2D texture2D)
    {
        Material material = new Material(_materials.effectMaterial);
        _materials.effectMaterialMap.Add(texture2D, material);
        material.SetTexture("_MainTex", texture2D);
        return material;
    }

}
