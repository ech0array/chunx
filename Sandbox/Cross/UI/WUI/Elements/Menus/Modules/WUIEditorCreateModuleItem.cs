using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

internal sealed class WUIEditorCreateModuleItem : WUIElement
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;
        [SerializeField]
        internal Image iconImage;
        [SerializeField]
        internal Button colorImage;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Module module;
    }
    private Data _data = new Data();


    internal void Populate(Module module)
    {
        _data.module = module;

        ColorBlock colorBlock = _components.colorImage.colors;
        colorBlock.normalColor = module.inspectionData.color;
        _components.colorImage.colors = colorBlock;

        _components.iconImage.sprite = module.inspectionData.icon;
        _components.nameLabel.text = module.data.defaultData.name;
    }

    public void UE_Click()
    {
        Vector3 position = wUI.user.controllableCamera.transform.position + (wUI.user.controllableCamera.transform.forward * 6f);

        Module module = EditorHead.instance.NewModule(_data.module, position);
        wUI.UnstackAll();
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.QuickMove, module, position);
    }
}
