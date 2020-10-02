using System;
using System.Collections.Generic;
using UnityEngine;

sealed internal class UserHead : SingleMonoBehaviour<UserHead>
{
    protected override bool isPersistant => true;

    #region Values
    [Serializable]
    private class InputSchemes
    {
        [Serializable]
        internal class PCAxisScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class AxisConversion
            {
                [SerializeField]
                internal PCAxis pCAxis;
                [SerializeField]
                internal UserAxis userAxis;
            }
            [SerializeField]
            internal AxisConversion[] conversions;
        }
        [SerializeField]
        internal PCAxisScheme[] pCAxisSchemes;
        [Serializable]
        internal class PCButtonScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class ButtonConversion
            {
                [SerializeField]
                internal PCButton pCButton;
                [SerializeField]
                internal UserButton userButton;
            }
            [SerializeField]
            internal ButtonConversion[] conversions;
        }
        [SerializeField]
        internal PCButtonScheme[] pCButtonSchemes;

        [Serializable]
        internal class TouchAxisScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class AxisConversion
            {
                [SerializeField]
                internal TouchAxis touchAxis;
                [SerializeField]
                internal UserAxis userAxis;
            }
            [SerializeField]
            internal AxisConversion[] conversions;
        }
        [Space(15)]
        [SerializeField]
        internal TouchAxisScheme[] touchAxisSchemes;
        [Serializable]
        internal class TouchButtonScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class ButtonConversion
            {
                [SerializeField]
                internal TouchButton touchButton;
                [SerializeField]
                internal UserButton userButton;
            }
            [SerializeField]
            internal ButtonConversion[] conversions;
        }
        [SerializeField]
        internal TouchButtonScheme[] touchButtonScheme;


        [Serializable]
        internal class NSwitchAxisScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class AxisConversion
            {
                [SerializeField]
                internal NSwitchAxis nSwitchAxis;
                [SerializeField]
                internal UserAxis userAxis;
            }
            internal AxisConversion[] conversions;
        }
        [Space(15)]
        [SerializeField]
        internal NSwitchAxisScheme[] nSwitchAxisSchemes;
        [Serializable]
        internal class NSwitchButtonScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class ButtonConversion
            {
                [SerializeField]
                internal NSwitchButton nSwitchButton;
                [SerializeField]
                internal UserButton userButton;
            }
            [SerializeField]
            internal ButtonConversion[] conversions;
        }
        [SerializeField]
        internal NSwitchButtonScheme[] nSwitchButtonSchemes;


        [Serializable]
        internal class XboxOneAxisScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class AxisConversion
            {
                [SerializeField]
                internal XboxOneAxis xboxOneAxis;
                [SerializeField]
                internal UserAxis userAxis;
            }
            [SerializeField]
            internal AxisConversion[] conversions;
        }
        [Space(15)]
        [SerializeField]
        internal XboxOneAxisScheme[] xboxOneAxisSchemes;
        [Serializable]
        internal class XboxOneButtonScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class ButtonConversion
            {
                [SerializeField]
                internal XboxOneButton xboxOneButton;
                [SerializeField]
                internal UserButton userButton;
            }
            [SerializeField]
            internal ButtonConversion[] conversions;
        }
        [SerializeField]
        internal XboxOneButtonScheme[] xboxOneButtonSchemes;


        [Serializable]
        internal class Playstation4AxisScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class AxisConversion
            {
                [SerializeField]
                internal PlayStation4Axis playstation4Axis;
                [SerializeField]
                internal UserAxis userAxis;
            }
            [SerializeField]
            internal AxisConversion[] conversions;
        }
        [Space(15)]
        [SerializeField]
        internal Playstation4AxisScheme[] playstation4AxisScheme;
        [Serializable]
        internal class Playstation4ButtonScheme
        {
            [SerializeField]
            internal string name;
            [Serializable]
            internal class ButtonConversion
            {
                [SerializeField]
                internal Playstation4Button playstation4Button;
                [SerializeField]
                internal UserButton userButton;
            }
            [SerializeField]
            internal ButtonConversion[] conversions;
        }
        [SerializeField]
        internal Playstation4ButtonScheme[] playstation4ButtonScheme;
    }
    [SerializeField]
    private InputSchemes _inputSchemes;

    [Serializable]
    internal class Components
    {
        [SerializeField]
        internal Transform controllableContainer;

        [SerializeField]
        internal Camera entryCamera;
    }
    [SerializeField]
    internal Components components;

    [Serializable]
    internal class Prefabs
    {
        [SerializeField]
        internal ControllableCamera controllableCamera;
    }
    [SerializeField]
    internal Prefabs prefabs;

    private static List<string> _genericUsernames = new List<string> { "Donut", "Penguin", "Stumpy", "Whicker", "Shadow", "Howard", "Wilshire", "Darling", "Disco", "Jack", "The Bear", "Sneak", "The Big L", "Whisp", "Wheezy", "Crazy", "Goat", "Pirate", "Saucy", "Hambone", "Butcher", "Walla Walla", "Snake", "Caboose", "Sleepy", "Killer", "Stompy", "Mopey", "Dopey", "Weasel", "Ghost", "Dasher", "Grumpy", "Hollywood", "Tooth", "Noodle", "King", "Cupid", "Prancer" };
    private List<string> _unusedGenericUsernames = null;

    private List<User> _localUsers = new List<User>();
    internal List<User> localUsers
    {
        get
        {
            return _localUsers;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        _unusedGenericUsernames = _genericUsernames;
    }

    #region Input Functions
    // Raw Input --> User? 
    // True: Raw Input Data --> Schemed User Input Data --> User
    // False : Login, break
    internal void ResolveUserInput(RawInputData rawInputData)
    {
       //User pCUSer = TryGetUserByInput(0, InputType.PC, rawInputData.pC.hasButtonDown);
        //if (pCUSer != null)
        //{
        //    //UserInputData userInputState = GetSchemedPcUserInputData(rawInputData, pCUSer);
        //   // pCUSer.TrySetInput(userInputState);
        //}

        User touchUser = TryGetUserByInput(0, InputType.Touch, rawInputData.touch.hasButtonDown);
        if (touchUser != null)
        {
            UserInputData userInputState = GetSchemedTouchUserInputState(rawInputData, touchUser);
            touchUser.TrySetInput(userInputState);
        }

        foreach (RawInputData.NSwitch.Controller controller in rawInputData.nSwitch.controllers)
        {
            User nSwitchUser = TryGetUserByInput(controller.id, InputType.NSwitch, controller.hasButtonDown);
            if (nSwitchUser != null)
            {
                UserInputData userInputState = GetSchemedNSwitchUserInputState(rawInputData, nSwitchUser);
                nSwitchUser.TrySetInput(userInputState);
            }
        }
        foreach (RawInputData.XboxOne.Controller controller in rawInputData.xboxOne.controllers)
        {
            User xboxOneUser = TryGetUserByInput(controller.id, InputType.XboxOne, controller.hasButtonDown);
            if (xboxOneUser != null)
            {
                UserInputData userInputState = GetSchemedXboxOneUserInputState(rawInputData, xboxOneUser);
                xboxOneUser.TrySetInput(userInputState);
            }
        }
        foreach (RawInputData.Playstation4.Controller controller in rawInputData.playstation4.controllers)
        {
            User playstation4User = TryGetUserByInput(controller.id, InputType.Playstation4, controller.hasButtonDown);
            if (playstation4User != null)
            {
                UserInputData userInputState = GetSchemedPlaystation4UserInputState(rawInputData, playstation4User);
                playstation4User.TrySetInput(userInputState);
            }
        }
    }

    // Raw Input Data --> Schemed User Input Data
    private UserInputData GetSchemedPcUserInputData(RawInputData rawInputData, User user)
    {
        InputSchemes.PCAxisScheme axisScheme = Array.Find(_inputSchemes.pCAxisSchemes, p => p.name == user.userData.settingsData.pcAxisInputScheme);
        if (axisScheme == null)
            axisScheme = _inputSchemes.pCAxisSchemes[0];

        InputSchemes.PCButtonScheme buttonScheme = Array.Find(_inputSchemes.pCButtonSchemes, p => p.name == user.userData.settingsData.pcButtonInputScheme);
        if (buttonScheme == null)
            buttonScheme = _inputSchemes.pCButtonSchemes[0];

        UserInputData userInputState = new UserInputData();

        InputSchemes.PCAxisScheme.AxisConversion moveAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Move);
        if (moveAxisConversion != null)
            userInputState.move = rawInputData.GetPCAxis(moveAxisConversion.pCAxis);

        InputSchemes.PCAxisScheme.AxisConversion lookAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Look);
        if (lookAxisConversion != null)
            userInputState.look = rawInputData.GetPCAxis(lookAxisConversion.pCAxis);

        InputSchemes.PCButtonScheme.ButtonConversion actionAButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionA);
        if (actionAButtonConversion != null)
            userInputState.actionA = rawInputData.GetPCButton(actionAButtonConversion.pCButton);

        InputSchemes.PCButtonScheme.ButtonConversion actionBButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionB);
        if (actionBButtonConversion != null)
            userInputState.actionB = rawInputData.GetPCButton(actionBButtonConversion.pCButton);


        InputSchemes.PCButtonScheme.ButtonConversion actionXButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionX);
        if (actionXButtonConversion != null)
            userInputState.actionX = rawInputData.GetPCButton(actionXButtonConversion.pCButton);

        InputSchemes.PCButtonScheme.ButtonConversion menuButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.Menu);
        if (menuButtonConversion != null)
            userInputState.menu = rawInputData.GetPCButton(menuButtonConversion.pCButton);

        return userInputState;
    }
    private UserInputData GetSchemedTouchUserInputState(RawInputData rawInputData, User user)
    {
        InputSchemes.TouchAxisScheme axisScheme = Array.Find(_inputSchemes.touchAxisSchemes, p => p.name == user.userData.settingsData.touchAxisInputScheme);
        if (axisScheme == null)
            axisScheme = _inputSchemes.touchAxisSchemes[0];
        InputSchemes.TouchButtonScheme buttonScheme = Array.Find(_inputSchemes.touchButtonScheme, p => p.name == user.userData.settingsData.touchButtonInputScheme);
        if (buttonScheme == null)
            buttonScheme = _inputSchemes.touchButtonScheme[0];

        UserInputData userInputState = new UserInputData();

        InputSchemes.TouchAxisScheme.AxisConversion moveAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Move);
        if (moveAxisConversion != null)
            userInputState.move = rawInputData.GetTouchAxis(moveAxisConversion.touchAxis);

        InputSchemes.TouchAxisScheme.AxisConversion lookAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Look);
        if (lookAxisConversion != null)
            userInputState.look = rawInputData.GetTouchAxis(lookAxisConversion.touchAxis);

        InputSchemes.TouchButtonScheme.ButtonConversion actionAButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionA);
        if (actionAButtonConversion != null)
            userInputState.actionA = rawInputData.GetTouchButton(actionAButtonConversion.touchButton);

        InputSchemes.TouchButtonScheme.ButtonConversion actionBButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionB);
        if (actionBButtonConversion != null)
            userInputState.actionB = rawInputData.GetTouchButton(actionBButtonConversion.touchButton);

        InputSchemes.TouchButtonScheme.ButtonConversion actionXButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionX);
        if (actionXButtonConversion != null)
            userInputState.actionX = rawInputData.GetTouchButton(actionXButtonConversion.touchButton);

        InputSchemes.TouchButtonScheme.ButtonConversion menuButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.Menu);
        if (menuButtonConversion != null)
            userInputState.menu = rawInputData.GetTouchButton(actionBButtonConversion.touchButton);

        return userInputState;
    }
    private UserInputData GetSchemedNSwitchUserInputState(RawInputData rawInputData, User user)
    {
        InputSchemes.NSwitchAxisScheme axisScheme = Array.Find(_inputSchemes.nSwitchAxisSchemes, p => p.name == user.userData.settingsData.nSwitchAxisInputScheme);
        if (axisScheme == null)
            axisScheme = _inputSchemes.nSwitchAxisSchemes[0];
        InputSchemes.NSwitchButtonScheme buttonScheme = Array.Find(_inputSchemes.nSwitchButtonSchemes, p => p.name == user.userData.settingsData.nSwitchButtonInputScheme);
        if (buttonScheme == null)
            buttonScheme = _inputSchemes.nSwitchButtonSchemes[0];

        UserInputData userInputState = new UserInputData();

        InputSchemes.NSwitchAxisScheme.AxisConversion moveAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Move);
        if (moveAxisConversion != null)
            userInputState.move = rawInputData.GetNSwitchAxis(moveAxisConversion.nSwitchAxis, user.inputId);

        InputSchemes.NSwitchAxisScheme.AxisConversion lookAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Look);
        if (lookAxisConversion != null)
            userInputState.look = rawInputData.GetNSwitchAxis(lookAxisConversion.nSwitchAxis, user.inputId);

        InputSchemes.NSwitchButtonScheme.ButtonConversion actionAButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionA);
        if (actionAButtonConversion != null)
            userInputState.actionA = rawInputData.GetNSwitchButton(actionAButtonConversion.nSwitchButton, user.inputId);

        InputSchemes.NSwitchButtonScheme.ButtonConversion actionBButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionB);
        if (actionBButtonConversion != null)
            userInputState.actionB = rawInputData.GetNSwitchButton(actionBButtonConversion.nSwitchButton, user.inputId);

        InputSchemes.NSwitchButtonScheme.ButtonConversion actionXButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionX);
        if (actionXButtonConversion != null)
            userInputState.actionX = rawInputData.GetNSwitchButton(actionXButtonConversion.nSwitchButton, user.inputId);

        InputSchemes.NSwitchButtonScheme.ButtonConversion menuButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.Menu);
        if (menuButtonConversion != null)
            userInputState.menu = rawInputData.GetNSwitchButton(menuButtonConversion.nSwitchButton, user.inputId);

        return userInputState;
    }
    private UserInputData GetSchemedXboxOneUserInputState(RawInputData rawInputData, User user)
    {
        InputSchemes.XboxOneAxisScheme axisScheme = Array.Find(_inputSchemes.xboxOneAxisSchemes, p => p.name == user.userData.settingsData.xboxOneAxisInputScheme);
        if (axisScheme == null)
            axisScheme = _inputSchemes.xboxOneAxisSchemes[0];
        InputSchemes.XboxOneButtonScheme buttonScheme = Array.Find(_inputSchemes.xboxOneButtonSchemes, p => p.name == user.userData.settingsData.xboxOneButtonInputScheme);
        if (buttonScheme == null)
            buttonScheme = _inputSchemes.xboxOneButtonSchemes[0];

        UserInputData userInputState = new UserInputData();

        InputSchemes.XboxOneAxisScheme.AxisConversion moveAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Move);
        if (moveAxisConversion != null)
            userInputState.move = rawInputData.GetXboxOneAxis(moveAxisConversion.xboxOneAxis, user.inputId);

        InputSchemes.XboxOneAxisScheme.AxisConversion lookAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Look);
        if (lookAxisConversion != null)
            userInputState.look = rawInputData.GetXboxOneAxis(lookAxisConversion.xboxOneAxis, user.inputId);

        InputSchemes.XboxOneButtonScheme.ButtonConversion actionAButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionA);
        if (actionAButtonConversion != null)
            userInputState.actionA = rawInputData.GetXboxOneButton(actionAButtonConversion.xboxOneButton, user.inputId);

        InputSchemes.XboxOneButtonScheme.ButtonConversion actionBButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionB);
        if (actionBButtonConversion != null)
            userInputState.actionB = rawInputData.GetXboxOneButton(actionBButtonConversion.xboxOneButton, user.inputId);

        InputSchemes.XboxOneButtonScheme.ButtonConversion actionXButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionX);
        if (actionXButtonConversion != null)
            userInputState.actionX = rawInputData.GetXboxOneButton(actionXButtonConversion.xboxOneButton, user.inputId);

        InputSchemes.XboxOneButtonScheme.ButtonConversion menuButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.Menu);
        if (menuButtonConversion != null)
            userInputState.menu = rawInputData.GetXboxOneButton(menuButtonConversion.xboxOneButton, user.inputId);

        return userInputState;
    }
    private UserInputData GetSchemedPlaystation4UserInputState(RawInputData rawInputData, User user)
    {
        InputSchemes.Playstation4AxisScheme axisScheme = Array.Find(_inputSchemes.playstation4AxisScheme, p => p.name == user.userData.settingsData.playstation4AxisInputScheme);
        if (axisScheme == null)
            axisScheme = _inputSchemes.playstation4AxisScheme[0];
        InputSchemes.Playstation4ButtonScheme buttonScheme = Array.Find(_inputSchemes.playstation4ButtonScheme, p => p.name == user.userData.settingsData.playstation4ButtonInputScheme);
        if (buttonScheme == null)
            buttonScheme = _inputSchemes.playstation4ButtonScheme[0];

        UserInputData userInputState = new UserInputData();

        InputSchemes.Playstation4AxisScheme.AxisConversion moveAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Move);
        if (moveAxisConversion != null)
            userInputState.move = rawInputData.GetPlaystation4Axis(moveAxisConversion.playstation4Axis, user.inputId);

        InputSchemes.Playstation4AxisScheme.AxisConversion lookAxisConversion = Array.Find(axisScheme.conversions, p => p.userAxis == UserAxis.Look);
        if (lookAxisConversion != null)
            userInputState.look = rawInputData.GetPlaystation4Axis(lookAxisConversion.playstation4Axis, user.inputId);

        InputSchemes.Playstation4ButtonScheme.ButtonConversion actionAButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionA);
        if (actionAButtonConversion != null)
            userInputState.actionA = rawInputData.GetPlaystation4Button(actionAButtonConversion.playstation4Button, user.inputId);

        InputSchemes.Playstation4ButtonScheme.ButtonConversion actionBButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionB);
        if (actionBButtonConversion != null)
            userInputState.actionB = rawInputData.GetPlaystation4Button(actionBButtonConversion.playstation4Button, user.inputId);

        InputSchemes.Playstation4ButtonScheme.ButtonConversion actionXButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.ActionX);
        if (actionXButtonConversion != null)
            userInputState.actionX = rawInputData.GetPlaystation4Button(actionXButtonConversion.playstation4Button, user.inputId);


        InputSchemes.Playstation4ButtonScheme.ButtonConversion menuButtonConversion = Array.Find(buttonScheme.conversions, p => p.userButton == UserButton.Menu);
        if (menuButtonConversion != null)
            userInputState.menu = rawInputData.GetPlaystation4Button(menuButtonConversion.playstation4Button, user.inputId);

        return userInputState;
    }
    #endregion

    #region User Functions
    internal User TryGetUserByInput(int inputId, InputType inputType, bool hasButtonDown)
    {
        User user = _localUsers.Find(u => u.inputId == inputId && u.inputType == inputType);
        if (hasButtonDown && user == null)
            user = RegisterUser(inputId, inputType);
        return user;
    }

    internal User RegisterUser(int inputId, InputType inputType)
    {
        User user = new User(Environment.UserName);
        user.inputId = inputId;
        user.inputType = inputType;
        user.name = DeindexRandomUsername();
        _localUsers.Add(user);

        CreateControllable(user);
        components.entryCamera.gameObject.SetActive(false);
        UIHead.instance.RegisterUser(user);
        return user;
    }
    internal void UnregisterUser(User user)
    {
        TryIndexRandomUsername(user.name);
        UIHead.instance.UnregisterUser(user);
        Destroy(user.controllableCamera.gameObject);
        AppropriateCameraRects();
    }

    internal User GetUserById(string userId)
    {
        return _localUsers.Find(u => u.userId == userId);
    }

    internal static void SaveAllUsersData()
    {
        foreach (User user in instance._localUsers)
            SaveUserData(user);
    }
    internal static void SaveUserData(User user)
    {
        UserDataHead.Save(user);
    }


    private string DeindexRandomUsername()
    {
        int randomIndice = UnityEngine.Random.Range(0, _unusedGenericUsernames.Count);
        string username = _unusedGenericUsernames[randomIndice].ToLower();
        _unusedGenericUsernames.Remove(username);
        return username;
    }
    private void TryIndexRandomUsername(string username)
    {
        if (_genericUsernames.Count == _unusedGenericUsernames.Count)
            return;
        if (_genericUsernames.Contains(username))
            _unusedGenericUsernames.Add(username);
    }
    #endregion

    private void CreateControllable(User user)
    {
        GameObject gameObject = Instantiate(prefabs.controllableCamera.gameObject, components.controllableContainer);
        gameObject.name = $"{prefabs.controllableCamera.gameObject.name}_{user.name}";

        ControllableCamera controllableCamera = gameObject.GetComponent<ControllableCamera>();
        controllableCamera.Register(user);
        user.controllableCamera = controllableCamera;
        AppropriateCameraRects();
    }

    private void AppropriateCameraRects()
    {
        for (int i = 0; i < _localUsers.Count; i++)
            _localUsers[i].controllableCamera.AppropriateRect(i, _localUsers.Count);
    }
}