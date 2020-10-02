using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed internal class RuntimeHead : SingleMonoBehaviour<RuntimeHead>
{
    protected override bool isPersistant => true;

    internal void OnStateExit()
    {
        ModuleHead.instance.Cleanup();
    }
    internal void OnStateEnter()
    {
        LinkHead.instance.ResetAll();
        ControllableCamera.SetAllMode(ControllableCamera.Mode.Runtime);
    }

    internal void Launch(UniverseData universeData)
    {
        GameHead.instance.universeData = universeData;
        GameHead.instance.EnterGameState(GameState.Runtime);

        Editable editable = ModuleHead.instance.Spawn(universeData.maps[0]);
        editable.OnAttach(UserHead.instance.localUsers[0]);
    }
}