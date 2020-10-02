using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

internal sealed class WUIMemberBrowserButton : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI text;
        [SerializeField]
        internal Button button;
    }
    [SerializeField]
    private Components _components;

    internal Action onClick;

    private void Awake()
    {
        _components.button.onClick.AddListener(()=> { onClick.Invoke(); });
    }

    internal void SetText(string text)
    {
        // <3
        _components.text.text = text.ToLower();
    }
}