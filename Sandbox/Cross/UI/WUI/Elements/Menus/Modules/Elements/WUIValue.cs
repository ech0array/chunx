using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class WUIValue : WUIElement
{
    #region Values
    [SerializeField]
    protected WUITipable wUITipable;

    internal SandboxValue valueReference;

    internal WUIMenu wUIMenu;
    #endregion

    internal abstract void Bind(SandboxValue valueReference);
}