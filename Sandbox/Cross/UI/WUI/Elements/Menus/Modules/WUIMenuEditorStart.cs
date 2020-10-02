using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIMenuEditorStart : WUIMenu
{

    public void UE_Exit()
    {
        wUI.UnstackAll();
        GameHead.instance.EnterGameState(GameState.Entry);
    }

    public void UE_Save()
    {
        EditorHead.instance.Save();
    }

    public void UE_Preview()
    {
        wUI.UnstackAll();
        EditorHead.instance.Save();
        PreviewHead.instance.Launch(EditorHead.instance.editableData);
    }
}
