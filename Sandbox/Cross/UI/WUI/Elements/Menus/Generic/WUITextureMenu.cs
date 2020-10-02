using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUITextureMenu : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform objectContainer;
        internal WUITextureReferenceValue WUITextureReferenceValue;
        internal List<WUITextureMenuButton> buttons = new List<WUITextureMenuButton>();
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUITextureMenuButton objectButton;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal void Open(WUITextureReferenceValue WUITextureReferenceValue)
    {
        _components.WUITextureReferenceValue = WUITextureReferenceValue;
        Populate();
        Stack();
    }

    private void Set(int index)
    {
        Unstack();
        _components.WUITextureReferenceValue.Set(index);
    }

    private void Populate()
    {
        Clear();

        List<Texture2D> textures = ((TextureSet)_components.WUITextureReferenceValue.valueReference.meta[0]) == TextureSet.Decals ? TextureHead.instance.GetAllDecalTextures() : TextureHead.instance.GetAllEffectTextures();
        for (int i = 0; i < textures.Count; i++)
            CreateObjectButton(i, textures[i]);
    }
    private void Clear()
    {
        foreach (WUITextureMenuButton wUITextureMenuButton in _components.buttons)
            Destroy(wUITextureMenuButton.gameObject);
        _components.buttons.Clear();
    }

    private void CreateObjectButton(int index, Texture2D texture2D)
    {
        GameObject gameObject = Instantiate(_prefabs.objectButton.gameObject, _components.objectContainer, false);
        WUITextureMenuButton wUITextureMenuButton = gameObject.GetComponent<WUITextureMenuButton>();
        _components.buttons.Add(wUITextureMenuButton);
        wUITextureMenuButton.Populate(()=> { Set(index); }, texture2D);
    }
}
