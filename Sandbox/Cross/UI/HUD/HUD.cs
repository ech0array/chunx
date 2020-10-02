using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class HUD : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Graphic reticleGraphic;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal Color highlightColor;
        [SerializeField]
        internal Color passiveColor;
    }
    [SerializeField]
    private Data _data;

    internal void SetReticleHighlight(Color? color = null)
    {
        _components.reticleGraphic.color = color == null ? _data.highlightColor : (Color)color;
    }
    internal void SetReticlePassive()
    {
        _components.reticleGraphic.color = _data.passiveColor;
    }

    internal void AppropriateRect(Camera camera)
    {
        RectTransform rectTransform = (RectTransform)base.transform;

        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        if (camera.rect.y > 0)
            anchorMin.y = 1 - camera.rect.y;
        else
            anchorMax.y = 1 - Mathf.Abs(camera.rect.y);

        if (camera.rect.x > 0)
            anchorMin.x = 1 - camera.rect.x;
        else
            anchorMax.x = 1 - Mathf.Abs(camera.rect.x);

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
