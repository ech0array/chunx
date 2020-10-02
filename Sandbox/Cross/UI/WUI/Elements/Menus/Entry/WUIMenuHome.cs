using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIMenuHome : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIMenuBuildHome wUIMenuBuildHome;

        [SerializeField]
        internal WUIMenuPlayHome wUIMenuPlayHome;
    }
    [SerializeField]
    private Components _components;

    public void UE_ShowBuildHome()
    {
        _components.wUIMenuBuildHome.Stack();
    }
    public void UE_ShowPlayHome()
    {
        _components.wUIMenuPlayHome.Stack();
    }
}
