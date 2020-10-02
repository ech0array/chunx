using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

sealed internal class XboxInputNode : SingleMonoBehaviour<XboxInputNode>
{
    protected override bool isPersistant => true;

    #region Values
    internal static int connectedControllerCount;

    private const float TRIGGER_DEAD_SPACE = 0.1215f;

    internal enum XboxInputButton
    {
        Xbox,
        Menu,
        Home,
        A,
        B,
        X,
        Y,
        LeftBumper,
        RightBumper,
        LeftStickIn,
        RightStickIn,
        DpadUp,
        DpadDown,
        DpadLeft,
        DpadRight,
        RightTrigger,
        LeftTrigger
    }
    internal enum XboxInputDirection
    {
        LeftStick,
        RightStick,
        Dpad
    }
    internal enum XboxInputAxis
    {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        LeftTrigger,
        RightTrigger
    }


    internal GamePadState[] controllers = new GamePadState[0];

    //private List<Vibration> _vibrations = new List<Vibration>();
    //private float[] _controllerVibrations;
    #endregion

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Update()
    {
        // UpdateVibrations();
        UpdateStates();
    }
    #endregion

    #region Functions
    private void Initialize()
    {
        controllers = new GamePadState[] { new GamePadState(), new GamePadState(), new GamePadState(), new GamePadState() };
        //_controllerVibrations = new float[] { 0, 0, 0, 0 };
    }

    private void UpdateStates()
    {
        connectedControllerCount = 0;
        for (int i = 0; i < 4; i++)
        {
            controllers[i] = GamePad.GetState((PlayerIndex)i);

            if (controllers[i].IsConnected)
                connectedControllerCount += 1;

            //  float vibrationIntensity = Mathf.Min(_controllerVibrations[i], 1);
            //  GamePad.SetVibration((PlayerIndex)i, vibrationIntensity, vibrationIntensity);
        }
    }


    internal static float GetAxis(int gamePad, XboxInputAxis axis)
    {
        if (axis == XboxInputAxis.LeftStickX)
            return instance.controllers[gamePad].ThumbSticks.Left.X;
        else if (axis == XboxInputAxis.LeftStickY)
            return instance.controllers[gamePad].ThumbSticks.Left.Y;
        else if (axis == XboxInputAxis.RightStickX)
            return instance.controllers[gamePad].ThumbSticks.Right.X;
        else if (axis == XboxInputAxis.RightStickY)
            return instance.controllers[gamePad].ThumbSticks.Right.Y;
        else if (axis == XboxInputAxis.LeftTrigger)
            return instance.controllers[gamePad].Triggers.Left;
        else if (axis == XboxInputAxis.RightTrigger)
            return instance.controllers[gamePad].Triggers.Right;
        return 0;
    }
    internal static Vector2 GetDirection(int gamePad, XboxInputDirection direction)
    {
        if (direction == XboxInputDirection.LeftStick)
            return new Vector2(instance.controllers[gamePad].ThumbSticks.Left.X, instance.controllers[gamePad].ThumbSticks.Left.Y);
        else if (direction == XboxInputDirection.RightStick)
            return new Vector2(instance.controllers[gamePad].ThumbSticks.Right.X, instance.controllers[gamePad].ThumbSticks.Right.Y);
        else if (direction == XboxInputDirection.Dpad)
        {
            float x = 0;
            if (instance.controllers[gamePad].DPad.Right == ButtonState.Pressed)
                x = 1;
            if (instance.controllers[gamePad].DPad.Left == ButtonState.Pressed)
                x = -1;

            float y = 0;
            if (instance.controllers[gamePad].DPad.Down == ButtonState.Pressed)
                y = 1;
            if (instance.controllers[gamePad].DPad.Up == ButtonState.Pressed)
                y = -1;
            return new Vector2(x, y);
        }


        return Vector2.zero;
    }

    internal static bool GetButton(int gamePad, XboxInputButton button)
    {
        return GetButtonState(instance.controllers[gamePad], gamePad, button);
    }

    internal static bool GetButtonState(GamePadState gamePadState, int gamePad, XboxInputButton button)
    {
        ButtonState currentState = ButtonState.Released;
        if (button == XboxInputButton.Xbox)
            currentState = gamePadState.Buttons.Guide;
        else if (button == XboxInputButton.Menu)
            currentState = gamePadState.Buttons.Start;
        else if (button == XboxInputButton.Home)
            currentState = gamePadState.Buttons.Back;
        else if (button == XboxInputButton.A)
            currentState = gamePadState.Buttons.A;
        else if (button == XboxInputButton.B)
            currentState = gamePadState.Buttons.B;
        else if (button == XboxInputButton.X)
            currentState = gamePadState.Buttons.X;
        else if (button == XboxInputButton.Y)
            currentState = gamePadState.Buttons.Y;
        else if (button == XboxInputButton.LeftBumper)
            currentState = gamePadState.Buttons.LeftShoulder;
        else if (button == XboxInputButton.RightBumper)
            currentState = gamePadState.Buttons.RightShoulder;
        else if (button == XboxInputButton.LeftStickIn)
            currentState = gamePadState.Buttons.LeftStick;
        else if (button == XboxInputButton.RightStickIn)
            currentState = gamePadState.Buttons.RightStick;
        else if (button == XboxInputButton.DpadUp)
            currentState = gamePadState.DPad.Up;
        else if (button == XboxInputButton.DpadDown)
            currentState = gamePadState.DPad.Down;
        else if (button == XboxInputButton.DpadLeft)
            currentState = gamePadState.DPad.Left;
        else if (button == XboxInputButton.DpadRight)
            currentState = gamePadState.DPad.Right;
        else if (button == XboxInputButton.LeftTrigger)
            currentState = gamePadState.Triggers.Left > TRIGGER_DEAD_SPACE ? ButtonState.Pressed : ButtonState.Released;
        else if (button == XboxInputButton.RightTrigger)
            currentState = gamePadState.Triggers.Right > TRIGGER_DEAD_SPACE ? ButtonState.Pressed : ButtonState.Released;

        return currentState == ButtonState.Pressed;
    }


    //private void UpdateVibrations()
    //{
    //    _controllerVibrations = new float[] { 0, 0, 0, 0 };
    //    for (int i = 0; i < _vibrations.Count; i++)
    //    {
    //        Vibration vibration = _vibrations[i];
    //        float duration = vibration.killTime - vibration.startTime;
    //        float timeRemaining = vibration.killTime - Time.time;

    //        _controllerVibrations[vibration.controllerId] += vibration.intensity * (timeRemaining / duration);

    //        if (Time.time >= vibration.killTime)
    //        {
    //            i--;
    //            _vibrations.Remove(vibration);
    //        }
    //    }
    //}

    //internal static void AddVibration(Vibration vibration)
    //{
    //    singleton._vibrations.Add(vibration);
    //}

    //internal static void ClearAllVibrations()
    //{
    //    singleton._vibrations.Clear();
    //}
    //internal static void ClearControllerVibrations(int controllerId)
    //{
    //    singleton._vibrations.RemoveAll(v => v.controllerId == controllerId);
    //}
    #endregion
}