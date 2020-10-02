using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal sealed class WUIEditorCreateSandboxObjectItem : WUIElement
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
        internal SandboxObjectData sandboxObjectData;
    }
    private Data _data = new Data();


    internal void Populate(SandboxObjectData sandboxObjectData)
    {
        _data.sandboxObjectData = sandboxObjectData;

        InspectionData inspectionData = null;
        if (EditorHead.instance.isMap)
            inspectionData = ModuleHead.instance.GetInspectionDataOfType(typeof(SandboxObjectSpawn));
        else
            inspectionData = ModuleHead.instance.GetInspectionDataOfType(typeof(SandboxObjectReference));


        ColorBlock colorBlock = new ColorBlock();
        colorBlock = _components.colorImage.colors;
        colorBlock.normalColor = inspectionData.color;
        _components.colorImage.colors = colorBlock;

        _components.iconImage.sprite = inspectionData.icon;
        _components.nameLabel.text = sandboxObjectData.name;
    }

    public void UE_Click()
    {
        Vector3 position = wUI.user.controllableCamera.transform.position + (wUI.user.controllableCamera.transform.forward * 6f);

        Module module = null;
        if (EditorHead.instance.isMap)
            module = EditorHead.instance.NewSandboxObjectSpawn(new SandboxObjectSpawnData(_data.sandboxObjectData), position);
        else
            module = EditorHead.instance.NewChildSandboxObject(new SandboxObjectReferenceData(_data.sandboxObjectData), position);


        wUI.UnstackAll();
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.QuickMove, module, position);
    }
}