using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed internal class WUITextureReferenceValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RawImage rawImage;
    }
    [SerializeField]
    private Components _components;

    internal int index;

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        index = ((int)sandboxValue.get());

        SetTexture(index);

        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }


    internal static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIObjectReferenceValue);
    }

    internal void Set(int index)
    {
        this.index = index;
        base.valueReference.set(index);
        SetTexture(index);
    }

    private void SetTexture(int index)
    {
        TextureSet textureSet = (TextureSet)valueReference.meta[0];
        switch (textureSet)
        {
            case TextureSet.Decals:
                _components.rawImage.texture = TextureHead.instance.GetDecal(index);
                break;
            case TextureSet.Effects:
                _components.rawImage.texture = TextureHead.instance.GetEffect(index);
                break;
        }
    }

    public void UE_Click()
    {
        wUI.Edit(this);
    }
}
