using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class WUIColorMenuFavorite : WUIElement
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIColorMenuFavoriteOptions wUIColorMenuFavoriteOptions;

        [SerializeField]
        internal Graphic colorGraphic;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal SerializableColor serializableColor;
    }
    private Data _data = new Data();

    internal void Populate(SerializableColor serializableColor)
    {
        _components.colorGraphic.color = serializableColor.color;
        _data.serializableColor = serializableColor;
    }

    public void UE_Inspect()
    {
        _components.wUIColorMenuFavoriteOptions.Inspect(this);
    }

    internal void Delete()
    {
        GameHead.instance.universeData.savedColors.Remove(_data.serializableColor);
        Destroy(base.gameObject);
    }
    internal void Select()
    {
        wUI.wUIColorMenu.SetColor(_components.colorGraphic.color);
    }
    internal void Replace()
    {
        _data.serializableColor.color = wUI.wUIColorMenu.activeColor;
        _components.colorGraphic.color = _data.serializableColor.color;
    }
}
