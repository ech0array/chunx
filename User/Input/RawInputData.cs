using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Q: Where is a types base input formed?
// A: Current[Type] EX: CurrentXboxOne()

// Q: I would like to add a button or axis?
// A: Add to the relative input class, make sure to add to hasButtonDown if the addition is a button - Add the new input to the relative enum, add the enum case to the relative Get(Axis or Button)


internal enum PCAxis
{
    WASD,
    Mouse
}
internal enum PCButton
{
    Space,
    Shift,
    Enter,
    Escape,
    MouseLeft,
    MouseRight
}

public enum TouchAxis
{
    LeftStick,
    RightStick
}
public enum TouchButton
{
    A,
    B,
    Home,
    Menu
}

public enum NSwitchAxis
{
    LeftStick,
    RightStick,
    DPad,
    LeftTrigger,
    RightTrigger
}
public enum NSwitchButton
{
    LeftStickIn,
    RightStickIn,
    A,
    B,
    X,
    Y,
    Home,
    Menu,
    Plus,
    Minus,
    LeftBumper,
    RightBumper
}

public enum XboxOneAxis
{
    LeftStick,
    RightStick,
    DPad,
    LeftTrigger,
    RightTrigger
}
public enum XboxOneButton
{
    LeftStickIn,
    RightStickIn,
    A,
    B,
    X,
    Y,
    Home,
    Menu,
    Xbox,
    LeftBumper,
    RightBumper
}

public enum PlayStation4Axis
{
    LeftStick,
    RightStick,
    DPad,
    LeftTrigger,
    RightTrigger
}
public enum Playstation4Button
{
    LeftStickIn,
    RightStickIn,
    X,
    Circle,
    Triangle,
    Square,
    Home,
    Menu,
    Playstation,
    LeftBumper,
    RightBumper
}

public enum InputType
{
    PC,
    Touch,
    NSwitch,
    XboxOne,
    Playstation4
}

sealed internal class RawInputData
{
    #region Values
    internal class PC
    {
        internal Vector2 wasd;
        internal Vector2 mouse;

        internal bool space;
        internal bool shift;
        internal bool enter;
        internal bool esc;
        internal bool mouseLeft;
        internal bool mouseRight;

        internal bool hasButtonDown
        {
            get
            {
                return space || shift || enter || esc || mouseLeft || mouseRight;
            }
        }
    }
    internal PC pC;

    internal class Touch
    {
        internal Vector2 leftStick;
        internal Vector2 rightStick;

        internal bool a;
        internal bool b;

        internal bool home;
        internal bool menu;

        internal bool hasButtonDown
        {
            get
            {
                return a || b || home || menu;
            }
        }
    }
    internal Touch touch;

    internal class NSwitch
    {
        internal class Controller
        {
            internal int id;

            internal Vector2 leftStick;
            internal Vector2 rightStick;
            internal Vector2 dPad;

            internal bool leftStickIn;
            internal bool rightStickIn;

            internal bool a;
            internal bool b;
            internal bool x;
            internal bool y;

            internal bool home;
            internal bool menu;

            internal bool plus;
            internal bool minus;

            internal bool leftBumper;
            internal bool rightBumper;

            internal float leftTrigger;
            internal float rightTrigger;

            internal bool hasButtonDown
            {
                get
                {
                    return a || b || x || y || home || menu || plus || minus || leftBumper || rightBumper || leftTrigger > 0 || rightTrigger > 0 || rightStickIn || leftStickIn;
                }
            }

        }
        internal Controller[] controllers;
    }
    internal NSwitch nSwitch;

    internal class XboxOne
    {
        internal class Controller
        {
            internal int id;

            internal Vector2 leftStick;
            internal Vector2 rightStick;
            internal Vector2 dPad;

            internal bool leftStickIn;
            internal bool rightStickIn;

            internal bool a;
            internal bool b;
            internal bool y;
            internal bool x;

            internal bool home;
            internal bool menu;

            internal bool xbox;

            internal bool leftBumper;
            internal bool rightBumper;

            internal float leftTrigger;
            internal float rightTrigger;

            internal bool hasButtonDown
            {
                get
                {
                    return a || b || x || y || home || menu || xbox || leftBumper || rightBumper || leftTrigger > 0 || rightTrigger > 0 || rightStickIn || leftStickIn;
                }
            }

        }
        internal Controller[] controllers;
    }
    internal XboxOne xboxOne;

    internal class Playstation4
    {
        internal class Controller
        {
            internal int id;

            internal Vector2 leftStick;
            internal Vector2 rightStick;
            internal Vector2 dPad;

            internal bool leftStickIn;
            internal bool rightStickIn;

            internal bool x;
            internal bool circle;
            internal bool triangle;
            internal bool square;


            internal bool home;
            internal bool menu;

            internal bool playstation;

            internal bool leftBumper;
            internal bool rightBumper;

            internal float leftTrigger;
            internal float rightTrigger;

            internal bool hasButtonDown
            {
                get
                {
                    return x || circle || triangle || square || home || menu || playstation || leftBumper || rightBumper || leftTrigger > 0 || rightTrigger > 0 || rightStickIn || leftStickIn;
                }
            }
        }
        internal Controller[] controllers;
    }
    internal Playstation4 playstation4;

    internal static RawInputData current
    {
        get
        {
            RawInputData rawInputState = new RawInputData();
            rawInputState.pC = CurrentPC();
            rawInputState.touch = CurrentTouch();
            rawInputState.nSwitch = CurrentNSwitch();
            rawInputState.xboxOne = CurrentXboxOne();
            rawInputState.playstation4 = CurrentPlaystation4();

            return rawInputState;
        }
    }
    #endregion

    #region Functions
    // Needs implementation
    private static PC CurrentPC()
    {

        // PC needs to use a platfrom specific manager aswell
        PC pC = new PC();
        pC.space = Input.GetButton("Jump");

        pC.mouseLeft = Input.GetMouseButton(0);
        pC.mouseRight = Input.GetMouseButton(1);

        return pC;
    }
    internal Vector2 GetPCAxis(PCAxis pCAxis)
    {
        switch (pCAxis)
        {
            case PCAxis.WASD:
                return pC.wasd;
            case PCAxis.Mouse:
                return pC.mouse;
        }
        return Vector2.zero;
    }
    internal bool GetPCButton(PCButton pCButton)
    {
        switch (pCButton)
        {
            case PCButton.Space:
                return pC.space;
            case PCButton.Shift:
                return pC.shift;
            case PCButton.Enter:
                return pC.enter;
            case PCButton.Escape:
                return pC.esc;
            case PCButton.MouseLeft:
                return pC.mouseLeft;
            case PCButton.MouseRight:
                return pC.mouseRight;
        }
        return false;
    }

    private static Touch CurrentTouch()
    {
        Touch touch = new Touch();
        touch.a = TouchInputNode.a;
        touch.b = TouchInputNode.b;
        touch.home = TouchInputNode.home;
        touch.menu = TouchInputNode.menu;

        touch.leftStick = TouchInputNode.leftStick;
        touch.rightStick = TouchInputNode.rightStick;
        return touch;
    }
    internal Vector2 GetTouchAxis(TouchAxis touchAxis)
    {
        switch (touchAxis)
        {
            case TouchAxis.LeftStick:
                return touch.leftStick;
            case TouchAxis.RightStick:
                return touch.rightStick;
        }
        return Vector2.zero;
    }
    internal bool GetTouchButton(TouchButton touchButton)
    {
        switch (touchButton)
        {
            case TouchButton.A:
                return touch.a;
            case TouchButton.B:
                return touch.b;
            case TouchButton.Home:
                return touch.home;
            case TouchButton.Menu:
                return touch.menu;
        }
        return false;
    }

    // Needs implementation
    private static NSwitch CurrentNSwitch()
    {
        NSwitch nSwitch = new NSwitch();
        nSwitch.controllers = new NSwitch.Controller[0];
        return nSwitch;
    }
    internal Vector2 GetNSwitchAxis(NSwitchAxis nSwitchAxis, int controller)
    {
        switch (nSwitchAxis)
        {
            case NSwitchAxis.LeftStick:
                return nSwitch.controllers[controller].leftStick;
            case NSwitchAxis.RightStick:
                return nSwitch.controllers[controller].rightStick;
            case NSwitchAxis.DPad:
                return nSwitch.controllers[controller].dPad;
            case NSwitchAxis.LeftTrigger:
                return Vector2.one * nSwitch.controllers[controller].leftTrigger;
            case NSwitchAxis.RightTrigger:
                return Vector2.one * nSwitch.controllers[controller].rightTrigger;
        }
        return Vector2.zero;
    }
    internal bool GetNSwitchButton(NSwitchButton nSwitchButton, int controller)
    {
        switch (nSwitchButton)
        {
            case NSwitchButton.LeftStickIn:
                return nSwitch.controllers[controller].leftStickIn;
            case NSwitchButton.RightStickIn:
                return nSwitch.controllers[controller].rightStickIn;
            case NSwitchButton.A:
                return nSwitch.controllers[controller].a;
            case NSwitchButton.B:
                return nSwitch.controllers[controller].b;
            case NSwitchButton.X:
                return nSwitch.controllers[controller].x;
            case NSwitchButton.Y:
                return nSwitch.controllers[controller].y;
            case NSwitchButton.Home:
                return nSwitch.controllers[controller].home;
            case NSwitchButton.Menu:
                return nSwitch.controllers[controller].menu;
            case NSwitchButton.Plus:
                return nSwitch.controllers[controller].plus;
            case NSwitchButton.Minus:
                return nSwitch.controllers[controller].minus;
            case NSwitchButton.LeftBumper:
                return nSwitch.controllers[controller].leftBumper;
            case NSwitchButton.RightBumper:
                return nSwitch.controllers[controller].rightBumper;
        }
        return false;
    }

    private static XboxOne CurrentXboxOne()
    {
        XboxOne xboxOne = new XboxOne();
        xboxOne.controllers = new XboxOne.Controller[XboxInputNode.connectedControllerCount];
        for (int i = 0; i < XboxInputNode.connectedControllerCount; i++)
        {

            XboxOne.Controller controller = xboxOne.controllers[i] = new XboxOne.Controller();
            controller.id = i;
            controller.leftStick = XboxInputNode.GetDirection(i, XboxInputNode.XboxInputDirection.LeftStick);
            controller.rightStick = XboxInputNode.GetDirection(i, XboxInputNode.XboxInputDirection.RightStick);
            controller.dPad = XboxInputNode.GetDirection(i, XboxInputNode.XboxInputDirection.Dpad);

            controller.leftTrigger = XboxInputNode.GetAxis(i, XboxInputNode.XboxInputAxis.LeftTrigger);
            controller.rightTrigger = XboxInputNode.GetAxis(i, XboxInputNode.XboxInputAxis.RightTrigger);

            controller.a = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.A);
            controller.b = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.B);
            controller.y = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.Y);
            controller.x = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.X);

            controller.leftBumper = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.LeftBumper);
            controller.rightBumper = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.RightBumper);

            controller.leftStickIn = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.LeftStickIn);
            controller.rightStickIn = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.RightStickIn);

            controller.menu = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.Menu);
            controller.home = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.Home);

            controller.xbox = XboxInputNode.GetButton(i, XboxInputNode.XboxInputButton.Xbox);
        }
        return xboxOne;
    }
    internal Vector2 GetXboxOneAxis(XboxOneAxis xboxOneAxis, int controller)
    {
        switch (xboxOneAxis)
        {
            case XboxOneAxis.LeftStick:
                return xboxOne.controllers[controller].leftStick;
            case XboxOneAxis.RightStick:
                return xboxOne.controllers[controller].rightStick;
            case XboxOneAxis.DPad:
                return xboxOne.controllers[controller].dPad;
            case XboxOneAxis.LeftTrigger:
                return Vector2.one * xboxOne.controllers[controller].leftTrigger;
            case XboxOneAxis.RightTrigger:
                return Vector2.one * xboxOne.controllers[controller].rightTrigger;
        }
        return Vector2.zero;
    }
    internal bool GetXboxOneButton(XboxOneButton xboxOneButton, int controller)
    {
        switch (xboxOneButton)
        {
            case XboxOneButton.LeftStickIn:
                return xboxOne.controllers[controller].leftStickIn;
            case XboxOneButton.RightStickIn:
                return xboxOne.controllers[controller].rightStickIn;
            case XboxOneButton.A:
                return xboxOne.controllers[controller].a;
            case XboxOneButton.B:
                return xboxOne.controllers[controller].b;
            case XboxOneButton.X:
                return xboxOne.controllers[controller].x;
            case XboxOneButton.Y:
                return xboxOne.controllers[controller].y;
            case XboxOneButton.Home:
                return xboxOne.controllers[controller].home;
            case XboxOneButton.Menu:
                return xboxOne.controllers[controller].menu;
            case XboxOneButton.Xbox:
                return xboxOne.controllers[controller].xbox;
            case XboxOneButton.LeftBumper:
                return xboxOne.controllers[controller].leftBumper;
            case XboxOneButton.RightBumper:
                return xboxOne.controllers[controller].rightBumper;
        }
        return false;
    }

    // Needs implementation
    private static Playstation4 CurrentPlaystation4()
    {
        Playstation4 playstation4 = new Playstation4();
        playstation4.controllers = new Playstation4.Controller[0];
        return playstation4;
    }
    internal Vector2 GetPlaystation4Axis(PlayStation4Axis playStation4Axis, int controller)
    {
        switch (playStation4Axis)
        {
            case PlayStation4Axis.LeftStick:
                return Vector2.one * playstation4.controllers[controller].leftStick;
            case PlayStation4Axis.RightStick:
                return Vector2.one * playstation4.controllers[controller].rightStick;
            case PlayStation4Axis.DPad:
                return Vector2.one * playstation4.controllers[controller].dPad;
            case PlayStation4Axis.LeftTrigger:
                return Vector2.one * playstation4.controllers[controller].leftTrigger;
            case PlayStation4Axis.RightTrigger:
                return Vector2.one * playstation4.controllers[controller].rightTrigger;
        }
        return Vector2.zero;
    }
    internal bool GetPlaystation4Button(Playstation4Button playstation4Button, int controller)
    {
        switch (playstation4Button)
        {
            case Playstation4Button.LeftStickIn:
                return playstation4.controllers[controller].leftStickIn;
            case Playstation4Button.RightStickIn:
                return playstation4.controllers[controller].rightStickIn;
            case Playstation4Button.X:
                return playstation4.controllers[controller].x;
            case Playstation4Button.Circle:
                return playstation4.controllers[controller].circle;
            case Playstation4Button.Triangle:
                return playstation4.controllers[controller].triangle;
            case Playstation4Button.Square:
                return playstation4.controllers[controller].square;
            case Playstation4Button.Home:
                return playstation4.controllers[controller].home;
            case Playstation4Button.Menu:
                return playstation4.controllers[controller].menu;
            case Playstation4Button.Playstation:
                return playstation4.controllers[controller].playstation;
            case Playstation4Button.LeftBumper:
                return playstation4.controllers[controller].leftBumper;
            case Playstation4Button.RightBumper:
                return playstation4.controllers[controller].rightBumper;
        }
        return false;
    } 
    #endregion
}