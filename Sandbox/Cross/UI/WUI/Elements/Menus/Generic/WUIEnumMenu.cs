using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUIEnumMenu : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform optionContainer;

        internal List<Button> buttons = new List<Button>();
        internal WUIEnumValue wUIEnumValue;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal TextMeshProUGUI text;
        [SerializeField]
        internal Button option;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal void Open(WUIEnumValue wUIEnumValue)
    {
        _components.wUIEnumValue = wUIEnumValue;

        foreach (Button button in _components.buttons)
            Destroy(button.gameObject);
        _components.buttons.Clear();

        string[] names = wUIEnumValue.names;

        for (int i = 0; i < names.Length; i++)
        {
            _prefabs.text.text = WUIEnumValue.GetCorrectedName(names[i]);
            GameObject gameObject = Instantiate(_prefabs.option.gameObject, _components.optionContainer, false);
            Button button = gameObject.GetComponent<Button>();

            _components.buttons.Add(button);
            int value = i;
            button.onClick.AddListener(() => { SelectOption(value); });
        }

        Stack();
    }

    private void SelectOption(int value)
    {
        Unstack();
        _components.wUIEnumValue.Set(value);
    }
}
