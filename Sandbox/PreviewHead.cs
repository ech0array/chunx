using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class PreviewHead : SingleMonoBehaviour<PreviewHead>
{
    protected override bool isPersistant => true;


    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Transform environment;
    }
    [SerializeField]
    private Components _components;

    internal void OnStateExit()
    {
        ModuleHead.instance.Cleanup();
        _components.environment.gameObject.SetActive(false);
    }
    internal void OnStateEnter()
    {
        LinkHead.instance.ResetAll();
        ControllableCamera.SetAllMode(ControllableCamera.Mode.Runtime);
        _components.environment.gameObject.SetActive(true);
    }
    internal void Launch(EditableData editableData)
    {
        GameHead.instance.EnterGameState(GameState.Preview);

        Editable editable = ModuleHead.instance.Spawn(editableData);
        editable.OnAttach(UserHead.instance.localUsers[0]);
    }
}
