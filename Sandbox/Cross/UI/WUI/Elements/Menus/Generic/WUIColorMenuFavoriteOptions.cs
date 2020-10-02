using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIColorMenuFavoriteOptions : WUIMenu
{
    private class Components
    {
        internal WUIColorMenuFavorite wUIColorMenuFavorite;
    }
    private Components _components = new Components();

    internal void Inspect(WUIColorMenuFavorite wUIColorMenuFavorite)
    {
        Stack();
        _components.wUIColorMenuFavorite = wUIColorMenuFavorite;
    }

    public void UE_Select()
    {
        _components.wUIColorMenuFavorite.Select();
        Unstack();
    }
    public void UE_Delete()
    {
        wUI.Confirm("WARNING! YOU ARE ABOUT TO DELETE A FAVORITE COLOR. YOU CANNOT UNDO THIS. PLEASE RECONSIDER BEFORE CONTINUING.", () =>
        {
            _components.wUIColorMenuFavorite.Delete();
            Unstack();
        });
    }

    public void UE_Replace()
    {
        _components.wUIColorMenuFavorite.Replace();
        Unstack();
    }
}
