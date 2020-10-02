using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal enum GameState
{
    Entry,
    Editor,
    Runtime,
    Preview
}

internal sealed class GameHead : SingleMonoBehaviour<GameHead>
{
    protected override bool isPersistant => true;

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal GameObject sun;
    }
    [SerializeField]
    private Components _components;

    internal GameState gameState { get; private set; }
    internal bool isPreviewOrRuntime => gameState == GameState.Preview || gameState == GameState.Runtime;

    internal UniverseData universeData;

    protected override void Awake()
    {
        EnterGameState(GameState.Entry);
        base.Awake();
    }


    internal void EnterGameState(GameState gameState)
    {
        DevelopmentHead.Track($"game_state", gameState.ToString().ToLower());
        GameState previousGamestate = this.gameState;
        this.gameState = gameState;
        ManageStateChange(gameState, previousGamestate);
    }

    private void ManageStateChange(GameState current, GameState previous)
    {
        if (current == previous)
            return;

        switch (previous)
        {
            case GameState.Editor:
                EditorHead.instance.OnStateExit();
                break;
            case GameState.Runtime:
                RuntimeHead.instance.OnStateExit();
                break;
            case GameState.Preview:
                PreviewHead.instance.OnStateExit();
                break;
            case GameState.Entry:
                EntryHead.instance.OnStateExit();
                break;
        }
        switch (current)
        {
            case GameState.Editor:
                EditorHead.instance.OnStateEnter();
                break;
            case GameState.Runtime:
                RuntimeHead.instance.OnStateEnter();
                break;
            case GameState.Preview:
                PreviewHead.instance.OnStateEnter();
                break;
            case GameState.Entry:
                EntryHead.instance.OnStateEnter();
                break;
        }
    }

    internal void SetSunState(bool value)
    {
        _components.sun.SetActive(value);
    }
}