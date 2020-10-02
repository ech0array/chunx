using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

internal sealed class WUITextureMenuButton : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RawImage rawImage;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Action callback;
        internal int id;
    }
    private Data _data = new Data();

    internal void Populate(Action callback, Texture2D texture2D)
    {
        _components.rawImage.texture = texture2D;
        _data.callback = callback;
    }

    public void UE_Click()
    {
        _data.callback.Invoke();
    }
}
