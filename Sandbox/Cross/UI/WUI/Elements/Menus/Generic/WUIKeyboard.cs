using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

sealed internal class WUIKeyboard : WUIMenu
{
    #region Values
    private bool _shifting;

    private int _targetCaretPosition;

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI[] alphaLabels;
        [SerializeField]
        internal TMP_InputField inputField;
    }
    [SerializeField]
    private Components _components;

    internal Action<string> _callback;
    #endregion

    internal void BeginEdit(string baseText, Action<string> callback)
    {
        _callback = callback;
        _components.inputField.text = baseText;
        _targetCaretPosition = _components.inputField.text.Length;
        _components.inputField.ActivateInputField();
        _components.inputField.MoveTextEnd(false);
        Stack();
    }

    internal override void Unstack()
    {
        base.Unstack();
        _callback.Invoke(_components.inputField.text);
    }

    #region Unity Event Functions

    public void UE_KeyBackspace()
    {
        string result = _components.inputField.text;

        if (result == string.Empty || _targetCaretPosition == 0)
            return;

        result = result.Remove(_targetCaretPosition - 1, 1);
        _targetCaretPosition -= 1;
        _components.inputField.text = result;
    }

    public void UE_KeyShift()
    {
        SetShiftState(!_shifting);
    }
    
    public void UE_MoveCaretRight()
    {
        int sum = _targetCaretPosition + 1;
        if (sum > _components.inputField.text.Length)
            return;
        _targetCaretPosition = sum;
    }
    public void UE_MoveCaretLeft()
    {
        int sum = _targetCaretPosition - 1;
        if (sum > _components.inputField.text.Length)
            return;
        _targetCaretPosition = sum;
    }


    public void UE_Space()
    {
        InsertStringAtCaret(" ");
    }
    public void UE_Enter()
    {
        InsertStringAtCaret("\n");
    }


    public void UE_KeyPeriod()
    {
        InsertStringAtCaret(".");
    }
    public void UE_KeyExclamation()
    {
        InsertStringAtCaret("!");
    }
    public void UE_KeyQuestion()
    {
        InsertStringAtCaret("?");
    }
    public void UE_KeyComma()
    {
        InsertStringAtCaret(",");
    }
    public void UE_KeyQuote()
    {
        InsertStringAtCaret("\"");
    }


    public void UE_KeyA()
    {
        InsertAlphaCharAtCaret('a');
    }
    public void UE_KeyB()
    {
        InsertAlphaCharAtCaret('b');
    }
    public void UE_KeyC()
    {
        InsertAlphaCharAtCaret('c');
    }
    public void UE_KeyD()
    {
        InsertAlphaCharAtCaret('d');
    }
    public void UE_KeyE()
    {
        InsertAlphaCharAtCaret('e');
    }
    public void UE_KeyF()
    {
        InsertAlphaCharAtCaret('f');
    }
    public void UE_KeyG()
    {
        InsertAlphaCharAtCaret('g');
    }
    public void UE_KeyH()
    {
        InsertAlphaCharAtCaret('h');
    }
    public void UE_KeyI()
    {
        InsertAlphaCharAtCaret('i');
    }
    public void UE_KeyJ()
    {
        InsertAlphaCharAtCaret('j');
    }
    public void UE_KeyK()
    {
        InsertAlphaCharAtCaret('k');
    }
    public void UE_KeyL()
    {
        InsertAlphaCharAtCaret('l');
    }
    public void UE_KeyM()
    {
        InsertAlphaCharAtCaret('m');
    }
    public void UE_KeyN()
    {
        InsertAlphaCharAtCaret('n');
    }
    public void UE_KeyO()
    {
        InsertAlphaCharAtCaret('o');
    }
    public void UE_KeyP()
    {
        InsertAlphaCharAtCaret('p');
    }
    public void UE_KeyQ()
    {
        InsertAlphaCharAtCaret('q');
    }
    public void UE_KeyR()
    {
        InsertAlphaCharAtCaret('r');
    }
    public void UE_KeyS()
    {
        InsertAlphaCharAtCaret('s');
    }
    public void UE_KeyT()
    {
        InsertAlphaCharAtCaret('t');
    }
    public void UE_KeyU()
    {
        InsertAlphaCharAtCaret('u');
    }
    public void UE_KeyV()
    {
        InsertAlphaCharAtCaret('v');
    }
    public void UE_KeyW()
    {
        InsertAlphaCharAtCaret('w');
    }
    public void UE_KeyX()
    {
        InsertAlphaCharAtCaret('x');
    }
    public void UE_KeyY()
    {
        InsertAlphaCharAtCaret('y');
    }
    public void UE_KeyZ()
    {
        InsertAlphaCharAtCaret('z');
    }

    public void UE_Key0()
    {
        InsertStringAtCaret("0");
    }
    public void UE_Key1()
    {
        InsertStringAtCaret("1");
    }
    public void UE_Key2()
    {
        InsertStringAtCaret("2");
    }
    public void UE_Key3()
    {
        InsertStringAtCaret("3");
    }
    public void UE_Key4()
    {
        InsertStringAtCaret("4");
    }
    public void UE_Key5()
    {
        InsertStringAtCaret("5");
    }
    public void UE_Key6()
    {
        InsertStringAtCaret("6");
    }
    public void UE_Key7()
    {
        InsertStringAtCaret("7");
    }
    public void UE_Key8()
    {
        InsertStringAtCaret("8");
    }
    public void UE_Key9()
    {
        InsertStringAtCaret("9");
    }
    #endregion

    #region Functions
    private void Update()
    {
        if (_components.inputField.caretPosition != _targetCaretPosition)
            _components.inputField.caretPosition = _targetCaretPosition;
    }

    private void InsertAlphaCharAtCaret(char value)
    {
        string valueString = value.ToString();
        if (_shifting)
            valueString = valueString.ToUpper();

        InsertStringAtCaret(valueString);

        if (_shifting)
            SetShiftState(false);
    }
    private void InsertStringAtCaret(string value)
    {
        string result = _components.inputField.text;
        string left = result.Substring(0, _components.inputField.caretPosition);
        string right = result.Substring(_targetCaretPosition, result.Length - _targetCaretPosition);
        result = left + value + right;
        if (result.Length > _components.inputField.characterLimit)
            return;
        _components.inputField.text = result;

        _targetCaretPosition += 1;
    }

    private void SetShiftState(bool value)
    {
        _shifting = value;
        foreach (TextMeshProUGUI textMeshProUGUI in _components.alphaLabels)
            textMeshProUGUI.text = _shifting ? textMeshProUGUI.text.ToUpper() : textMeshProUGUI.text.ToLower();
    } 
    #endregion
}
