using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIMenuRuntimeStart : WUIMenu
{
    public void UE_Exit()
    {
        wUI.UnstackAll();
        if (GameHead.instance.gameState == GameState.Runtime)
            GameHead.instance.EnterGameState(GameState.Entry);
        else
            EditorHead.instance.ReturnToEdit();
    }
}
