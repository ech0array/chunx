using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

internal sealed class WUITip : WUIElement
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameText;
        [SerializeField]
        internal TextMeshProUGUI descriptionText;

        [SerializeField]
        internal Graphic colorGraphic;
        [SerializeField]
        internal Graphic colorGraphic2;
    }
    [SerializeField]
    private Components _components;



    internal void Show(Vector3 position, WUITipable wUITipable)
    {
        _components.colorGraphic.color = wUITipable.color;
        _components.colorGraphic2.color = wUITipable.color;
        base.transform.position = position;

        _components.nameText.text = wUITipable.name;
        _components.descriptionText.text = wUITipable.description;
        _components.descriptionText.gameObject.SetActive(!(wUITipable.description == "" || wUITipable.description == string.Empty));

        if (!base.gameObject.activeSelf)
            base.gameObject.SetActive(true);
    }

    internal void Hide()
    {
        base.gameObject.SetActive(false);
    }
}
