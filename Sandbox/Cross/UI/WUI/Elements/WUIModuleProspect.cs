using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUIModuleProspect : WUIElement
{
    #region Values

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Image iconImage;
        [SerializeField]
        internal Graphic colorGraphic;
        [SerializeField]
        internal TextMeshProUGUI titleText;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal Sprite passiveIcon;
    }
    [SerializeField]
    private Data _data;
    #endregion
     
    #region Display
    internal void Inspect(Module module, string name, Vector3 position)
    {
        Type type = module.GetType();
        InspectionData inspectionData = ModuleHead.instance.GetInspectionDataOfType(type);
        if (inspectionData == null)
            return;
        if (!base.gameObject.activeSelf)
            base.gameObject.SetActive(true);

        _components.colorGraphic.color = inspectionData.color;
        _components.iconImage.sprite = module.passive ? _data.passiveIcon : inspectionData.icon;
        _components.titleText.text = name;
        base.transform.position = position;
    }
    internal void Hide()
    {
        base.gameObject.SetActive(false);
    } 
    #endregion
}
