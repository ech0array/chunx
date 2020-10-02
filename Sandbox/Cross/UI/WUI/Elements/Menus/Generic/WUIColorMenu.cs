using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUIColorMenu : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Image colorDisplayImage;
        [SerializeField]
        internal Image initialColorDisplayImage;
        [SerializeField]
        internal Image alphaColorDisplayImage;

        [SerializeField]
        internal Image colorRangeImage;
        [SerializeField]
        internal WUIGraph colorRangeGraph;
        [SerializeField]
        internal WUISlider hueSlider;
        [SerializeField]
        internal WUISlider alphaSlider;


        [SerializeField]
        internal RectTransform favoritesContainer;

        internal WUIValue wUIValue;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Color activeColor;
        internal float alpha;
        internal bool favoritesPopulated;
    }
    private Data _data = new Data();

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUIColorMenuFavorite wUIColorMenuFavorite;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal Color activeColor => _data.activeColor;

    protected override void Awake()
    {
        base.Awake();

        _components.colorRangeGraph.onValueChanged += SetSaturationAndValue;
        _components.hueSlider.onValueChanged += SetHue;
        _components.alphaSlider.onValueChanged += SetAlpha;
    }

    internal void Open(WUIValue wUIValue)
    {
        Stack();
        _components.wUIValue = wUIValue;

        Color color = wUIValue is WUIColorValue ? ((WUIColorValue)wUIValue).color : ((WUIGradientValue)wUIValue).color;

        _components.initialColorDisplayImage.color = color;
        SetColor(color);
        PopulateSavedColors();
    }

    internal override void Unstack()
    {
        base.Unstack();
        if(_components.wUIValue is WUIColorValue)
           ((WUIColorValue)_components.wUIValue).Set(_data.activeColor);
        else if(_components.wUIValue is WUIGradientValue)
            ((WUIGradientValue)_components.wUIValue).Set(_data.activeColor);
    }

    private void SetHue(float value)
    {
        float hue;
        float saturation;
        float brightness;
        Color.RGBToHSV(_data.activeColor, out hue, out saturation, out brightness);

        _data.activeColor = Color.HSVToRGB(value, saturation, brightness);
        Output();
    }
    private void SetAlpha(float value)
    {
        _data.alpha = value;
        Output();
    }
    private void SetSaturationAndValue(Vector2 value)
    {
        float hue;
        float saturation;
        float brightness;
        Color.RGBToHSV(_data.activeColor, out hue, out saturation, out brightness);

        _data.activeColor = Color.HSVToRGB(hue, value.x, value.y);
        Output();
    }
    private Color GetHueColor(Color color)
    {
        float hue;
        float saturation;
        float brightness;
        Color.RGBToHSV(_data.activeColor, out hue, out saturation, out brightness);
        return Color.HSVToRGB(hue, 1, 1);
    }
    private void Output()
    {
        _data.activeColor.a = _data.alpha;
        _components.colorRangeImage.color = GetHueColor(_data.activeColor);
        _components.alphaColorDisplayImage.color = new Color(_data.activeColor.r, _data.activeColor.g, _data.activeColor.b, 1f);
        _components.colorDisplayImage.color = _data.activeColor;
    }
    internal void SetColor(Color color)
    {
        _data.activeColor = color;
        _data.alpha = _data.activeColor.a;
        _components.alphaColorDisplayImage.color = new Color(_data.activeColor.r, _data.activeColor.g, _data.activeColor.b, 1f);
        _components.colorDisplayImage.color = _data.activeColor;


        float hue;
        float saturation;
        float brightness;
        Color.RGBToHSV(_data.activeColor, out hue, out saturation, out brightness);
        if (_data.activeColor.r + _data.activeColor.g + _data.activeColor.b == 3f)
        {
            hue = 0f;
            saturation = 0f;
            brightness = 1f;
        }
        _components.hueSlider.Chart(hue);
        _components.colorRangeGraph.Chart(saturation, brightness);
        _components.alphaSlider.Chart(_data.alpha);
        _components.colorRangeImage.color = GetHueColor(_data.activeColor);
    }


    private void PopulateSavedColors()
    {
        if (_data.favoritesPopulated)
            return;
        _data.favoritesPopulated = true;

        if (GameHead.instance.universeData.savedColors == null)
            return;
            List<SerializableColor> serializableColors = GameHead.instance.universeData.savedColors;
        foreach (SerializableColor serializableColor in serializableColors)
            CreateFavoriteColor(serializableColor);
    }
    private void CreateFavoriteColor(SerializableColor serializableColor)
    {
        GameObject gameObject = Instantiate(_prefabs.wUIColorMenuFavorite.gameObject, _components.favoritesContainer, false);
        WUIColorMenuFavorite wUIColorMenuFavorite = gameObject.GetComponent<WUIColorMenuFavorite>();
        gameObject.transform.SetAsLastSibling();
        wUIColorMenuFavorite.Populate(serializableColor);
    }
    public void UE_AddFavorite()
    {
        if (GameHead.instance.universeData.savedColors == null)
            GameHead.instance.universeData.savedColors = new List<SerializableColor>();
        SerializableColor serializableColor = new SerializableColor(_data.activeColor);
        GameHead.instance.universeData.savedColors.Add(serializableColor);
        CreateFavoriteColor(serializableColor);
    }

}
