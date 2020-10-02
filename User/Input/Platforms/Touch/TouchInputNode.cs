using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputNode : MonoBehaviour
{
    internal static Vector2 leftStick = new Vector2();
    internal static Vector2 rightStick = new Vector2();

    internal static bool a = false;
    internal static bool b = false;
    internal static bool home = false;
    internal static bool menu = false;

    internal static void SetAxisState(TouchAxis touchAxis, Vector2 value)
    {
        switch (touchAxis)
        {
            case TouchAxis.LeftStick:
                leftStick = value;
                break;
            case TouchAxis.RightStick:
                rightStick = value;
                break;
        }
    }
    internal static void SetButtonState(TouchButton touchButton, bool value)
    {
        switch (touchButton)
        {
            case TouchButton.A:
                a = value;
                break;
            case TouchButton.B:
                b = value;
                break;
            case TouchButton.Home:
                home = value;
                break;
            case TouchButton.Menu:
                menu = value;
                break;
        }
    }
}
