using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum UserAxis
{
    Look,
    Move
}
internal enum UserButton
{
    ActionA,
    ActionB,
    ActionX,
    ActionY,
    Menu
}

sealed internal class UserInputData
{
    internal Vector2 look;
    internal Vector2 move;

    internal bool actionA;
    internal bool actionB;
    internal bool actionX;
    internal bool actionY;
    internal bool menu;

    public static UserInputData operator +(UserInputData x, UserInputData y)
    {
        UserInputData userInputState = new UserInputData();

        userInputState.look = (x.look + y.look).normalized;
        userInputState.move = (x.move + y.move).normalized;
        userInputState.actionA = x.actionA && y.actionA;
        userInputState.actionB = x.actionB && y.actionB;
        userInputState.menu = x.menu && y.menu;

        return userInputState;
    }
}