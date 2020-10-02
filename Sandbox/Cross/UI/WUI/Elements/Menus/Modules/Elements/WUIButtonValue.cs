using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

internal enum ButtonValueColor
{
    Normal,
    Dangerous,
    Warning
}

sealed internal class WUIButtonValue : WUIValue
{
    internal Action onClick = new Action(() => { });
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Button button;
        [SerializeField]
        internal Button confirmButton;
        [SerializeField]
        internal Button denyButton;
        [SerializeField]
        internal RectTransform confirmationContainer;
        [SerializeField]
        internal TextMeshProUGUI text;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Colors
    {
        [SerializeField]
        internal Color normal;
        [SerializeField]
        internal Color dangerous;
        [SerializeField]
        internal Color warning;
    }
    [SerializeField]
    private Colors _colors;

    private bool _closeParentMenu;
    private bool _requireConfirmation;

    private void Awake()
    {
        BindEvents();
    }
    private void BindEvents()
    {
        // Assign callback to button click
        _components.button.onClick.AddListener(()=> { OnClick();});
        _components.confirmButton.onClick.AddListener(() => { OnConfirm(); });
        _components.denyButton.onClick.AddListener(() => { OnDeny(); });
    }

    private void OnClick()
    {
        if (_requireConfirmation)
            _components.confirmationContainer.gameObject.SetActive(true);
        else
            PerformAction();
    }
    private void OnConfirm()
    {
        _components.confirmationContainer.gameObject.SetActive(false);
        PerformAction();
    }
    private void OnDeny()
    {
        _components.confirmationContainer.gameObject.SetActive(false);
    }
    private void PerformAction()
    {
        if (_closeParentMenu)
        {
            // Close parent menu
            WUIMenu wUIMenu = base.gameObject.GetComponentInParent<WUIMenu>();
            if (wUIMenu != null)
                wUIMenu.Unstack();
        }

        onClick.Invoke();
    }

    internal override void Bind(SandboxValue sandboxValue)
    {
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
        //_requireConfirmation = requireConfirmation;
        //_closeParentMenu = closeParentMenu;
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
        _components.text.text = sandboxValue.id;
        onClick += (Action)sandboxValue.meta[0];
        //OverrideColor(buttonValueColor);
    }

    private void OverrideColor(ButtonValueColor buttonValueColor)
    {
        Color color = _colors.normal;
        switch (buttonValueColor)
        {
            case ButtonValueColor.Dangerous:
                color = _colors.dangerous;
                break;
            case ButtonValueColor.Warning:
                color = _colors.warning;
                break;
        }

        ColorBlock colorBlock = _components.button.colors;
        colorBlock.normalColor = color;
        _components.button.colors = colorBlock;
        _components.confirmButton.colors = colorBlock;
        _components.denyButton.colors = colorBlock;
    }
}
