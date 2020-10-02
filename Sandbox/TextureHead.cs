using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum TextureSet
{
    Decals,
    Effects
}
internal sealed class TextureHead : SingleMonoBehaviour<TextureHead>
{
    protected override bool isPersistant => true;

    [Serializable]
    private class Textures
    {
        [SerializeField]
        internal List<Texture2D> decalTextures = new List<Texture2D>();
        [SerializeField]
        internal List<Texture2D> effectTextures = new List<Texture2D>();
    }
    [SerializeField]
    private Textures _textures;

    internal Texture2D GetDecal(int index)
    {
        return _textures.decalTextures[index];
    }
    internal Texture2D GetEffect(int index)
    {
        return _textures.effectTextures[index];
    }

    internal List<Texture2D> GetAllDecalTextures()
    {
        return _textures.decalTextures;
    }
    internal List<Texture2D> GetAllEffectTextures()
    {
        return _textures.effectTextures;
    }
}
