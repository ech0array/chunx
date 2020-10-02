using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

sealed internal class WUINumberPad : WUIMenu
{
    #region Values
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TMP_InputField inputField;

        [SerializeField]
        internal Button periodButton;
    }
    [SerializeField]
    private Components _components;

    private int _targetCaretPosition;
    private bool _isDecimal;

    internal Action<string> _callback;
    #endregion

    private void Update()
    {
        if (_components.inputField.caretPosition != _targetCaretPosition)
            _components.inputField.caretPosition = _targetCaretPosition;
    }

    internal void BeginEdit(string baseText, Action<string> callback, bool isDecimal = false)
    {
        _callback = callback;
        _components.inputField.text = baseText;
        _targetCaretPosition = _components.inputField.text.Length;
        _isDecimal = isDecimal;
        _components.periodButton.interactable = _isDecimal;
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

    public void UE_MoveCaretRight()
    {
        int sum = _targetCaretPosition + 1;
        if (sum > _components.inputField.text.Length)
            return;
        _targetCaretPosition = sum;
    }
    public void UE_MoveCaretLeft()
    {
        _targetCaretPosition -= 1;
    }


    public void UE_Add()
    {
        float additive = _isDecimal ? 0.1f : 1f;
        float value = float.Parse(_components.inputField.text);
        _components.inputField.text = (value + additive).ToString();
    }

    public void UE_Subtract()
    {
        float additive = _isDecimal ? 0.1f : 1f;
        float value = float.Parse(_components.inputField.text);
        _components.inputField.text = (value - additive).ToString();
    }

    public void UE_KeyPeriod()
    {
        if (_components.inputField.text.Contains("."))
            return;
        InsertStringAtCaret(".");
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

    public void UE_Invert()
    {
        if (_components.inputField.text[0] == '-')
            _components.inputField.text = _components.inputField.text.Replace("-", string.Empty);
        else
            _components.inputField.text = $"-{_components.inputField.text}";

        if (_components.inputField.text == "-0")
            _components.inputField.text = "0";
    }
    #endregion

    #region Functions
    private void InsertStringAtCaret(string value)
    {
        string result = _components.inputField.text;
        string left = result.Substring(0, _components.inputField.caretPosition);
        string right = result.Substring(_targetCaretPosition, result.Length - _targetCaretPosition);
        result = left + value + right;
        if (result.Length > _components.inputField.characterLimit)
            return;
        _components.inputField.text = result;

        // NOTE: Caret isnt moving in time for rapid input, so on each input add to a local var and set the text there, wait for caret change, etc 
        _targetCaretPosition += 1;
    }
    #endregion
}
