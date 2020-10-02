using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed internal class User
{
    public User(string userid)
    {
        this.userId = userid;
        userData = UserDataHead.Load(this);
    }

    #region Values
    internal string name;
    internal string userId;

    internal HUD hUD;
    internal EditorWUI editorWUI;
    internal RuntimeWUI runtimeWUI;
    internal ControllableCamera controllableCamera;

    internal SandboxObject attachement;
    internal bool allowRuntimeInput => controllableCamera.mode == ControllableCamera.Mode.Runtime;

    internal UserData userData { get; private set;}

    private Dictionary<Type, object> _typeValueMap = new Dictionary<Type, object>();

    #region Input
    internal int inputId;
    internal InputType inputType;
    internal bool allowInput = true;

    internal UserInputData input { get; private set; }
    internal UserInputData deltaInput { get; private set; }
    #endregion
    #endregion

    #region Input Functionss
    internal void TrySetInput(UserInputData userInputData)
    {
        if (!allowInput)
        {
            input = new UserInputData();
            deltaInput = new UserInputData();
            return;
        }

        userInputData.look.y *= userData.settingsData.invertYLook ? -1 : 1;

        deltaInput = input == null ? new UserInputData() : input;
        input = userInputData;
    }
    #endregion

    internal void SaveData()
    {
        UserHead.SaveUserData(this);
    }
    internal void Reload()
    {
        userData = UserDataHead.Load(this);
    }

    internal void SetClipboard(Type type, object value)
    {
        if (_typeValueMap.ContainsKey(type))
            _typeValueMap[type] = value;
        else
            _typeValueMap.Add(type, value);
    }
    internal object GetClipboard(Type type)
    {
        if (_typeValueMap.ContainsKey(type))
            return _typeValueMap[type];
        return null;
    }
    internal bool CheckClipboardForType(Type type)
    {
        return _typeValueMap.ContainsKey(type);
    }
}