using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class EntryHead : SingleMonoBehaviour<EntryHead>
{
    protected override bool isPersistant => true;

    internal void OnStateEnter()
    {
        UIHead.instance.SetEntryUIState(true);
        ControllableCamera.SetAllMode(ControllableCamera.Mode.WUI);
    }

    internal void OnStateExit()
    {
        UIHead.instance.SetEntryUIState(false);
    }
}