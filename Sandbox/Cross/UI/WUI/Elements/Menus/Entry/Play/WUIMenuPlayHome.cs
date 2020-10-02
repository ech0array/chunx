using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
internal sealed class WUIMenuPlayHome : WUIMenu
{

    [Serializable]
    private class Components
    {
    }
    [SerializeField]
    private Components _components;

    public void UE_TestFirst()
    {
        RuntimeHead.instance.Launch(wUI.user.userData.universeDatas[0]);
    }
}
