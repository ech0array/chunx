using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
internal abstract class WUI : MonoBehaviour
{
    #region Values
    internal User user { get; private set; }

    protected class BaseComponents
    {
        [SerializeField]
        internal Graphic background;
        internal List<WUIElement> wUIElements = new List<WUIElement>();
        internal Stack<WUIMenu> wUIMenuStack = new Stack<WUIMenu>();

        [SerializeField]
        internal WUIKeyboard keyboard;
        [SerializeField]
        internal WUINumberPad numberPad;
        [SerializeField]
        internal WUIEnumMenu enumMenu;
        [SerializeField]
        internal WUIColorMenu colorMenu;
        [SerializeField]
        internal WUIObjectMenu objectMenu;
        [SerializeField]
        internal WUITextureMenu textureMenu;
        [SerializeField]
        internal WUICurveMenu wUICurveMenu;
        [SerializeField]
        internal WUISandboxValueReferenceObjectMenu wUISandboxValueReferenceObjectMenu;
        [SerializeField]
        internal WUISandboxValueReferenceMenu wUISandboxValueReferenceMenu;

        [SerializeField]
        internal RectTransform wUIMenuContainer;

        [SerializeField]
        internal WUITip wUITip;

        [SerializeField]
        internal WUIMenuConfirm wUIMenuConfirm;
    }
    protected abstract BaseComponents baseComponents { get; }

    protected class BaseData
    {
        [SerializeField]
        internal Material wUIMaterial;
        [SerializeField]
        internal float inverseTraversalAssitanceScale = 1f;
        internal Vector3 deltaPosition;
    }
    protected abstract BaseData baseData { get; }

    internal WUIColorMenu wUIColorMenu => baseComponents.colorMenu;
    internal WUIMenu focalWUIMenu;
    internal Vector3 origin { get; private set; }
    internal bool isStackEmpty => baseComponents.wUIMenuStack.Count == 0;
    internal Vector3 rotation => baseComponents.wUIMenuContainer.transform.eulerAngles;
    internal Vector3 deltaPosition => baseData.deltaPosition;

    internal Action onClick = new Action(() => { });
    #endregion

    #region Unity Framework Entry Functions
    protected virtual void Awake()
    {
        Initialize();
    }
    protected void LateUpdate()
    {
        if (user == null)
            return;
        BillboardAll();
    } 
    #endregion

    #region Functions
    private void Initialize()
    {
        baseComponents.wUIMenuContainer.gameObject.SetActive(true);
    }

    internal void Register(User user)
    {
        this.user = user;
        Canvas canvas = base.gameObject.GetComponent<Canvas>();
        canvas.worldCamera = user.controllableCamera.camera;
    }
    internal void Register(WUIElement value)
    {
        if (baseComponents.wUIElements.Contains(value))
            return;

        baseComponents.wUIElements.Add(value);

        if (value.billboardType == WUIElementBillboardType.Initial)
            Billboard(value);

        TMP_SelectionCaret[] selectionCarets = value.gameObject.GetComponentsInChildren<TMP_SelectionCaret>();
        foreach (TMP_SelectionCaret selectionCaret in selectionCarets)
            selectionCaret.material = baseData.wUIMaterial;
    }


    internal void Unstack()
    {
        baseComponents.wUIMenuStack.Pop();

        if (baseComponents.wUIMenuStack.Count > 0)
        {
            WUIMenu currentWUIMenu = baseComponents.wUIMenuStack.Peek();
            if (currentWUIMenu != null)
                currentWUIMenu.Focus();
        }
        else
        {
            baseComponents.background.gameObject.SetActive(false);
        }
    }
    internal void UnstackAll()
    {
        RESTART:
        foreach (WUIMenu wUIMenu in baseComponents.wUIMenuStack)
        {
            wUIMenu.Unstack();
            goto RESTART;
        }
        baseComponents.wUIMenuStack.Clear();

        baseComponents.background.gameObject.SetActive(false);
    }
    internal bool UnstackCurrent()
    {
        WUIMenu wUIMenu = baseComponents.wUIMenuStack.Peek();
        if (wUIMenu == null)
            return true;
        wUIMenu.Unstack();
        return baseComponents.wUIMenuStack.Count == 0;
    }
    internal bool Stack(WUIMenu wUIMenu)
    {
        if (baseComponents.wUIMenuStack.Contains(wUIMenu))
            return false;


        if (baseComponents.wUIMenuStack.Count > 0)
        {
            WUIMenu currentWUIMenu = baseComponents.wUIMenuStack.Peek();
            if (currentWUIMenu != null)
            {
                if (currentWUIMenu.blockStack)
                    return false;
                currentWUIMenu.Unfocus();
            }
        }

        wUIMenu.transform.position = baseComponents.wUIMenuStack.Count == 0 ? baseComponents.wUIMenuContainer.transform.position : baseData.deltaPosition;
        wUIMenu.Focus();

        baseComponents.wUIMenuStack.Push(wUIMenu);

        return true;
    }
    internal void Focus(WUIMenu wUIMenu, Color color)
    {
        focalWUIMenu = wUIMenu;
        wUIMenu.transform.SetAsLastSibling();
        int sibilingIndex = wUIMenu.transform.GetSiblingIndex();
        baseComponents.background.gameObject.transform.SetSiblingIndex(sibilingIndex - 1);
        baseComponents.background.color = color;
        baseComponents.background.gameObject.SetActive(true);
    }


    internal void Position(Vector3 value)
    {
        this.origin = value;
        baseComponents.wUIMenuContainer.transform.position = value;
    }
    internal void Traverse(Ray ray)
    {
        float distance = CastPlane(ray).distance;

        Vector3 position = (ray.origin + (ray.direction * distance));

        baseData.deltaPosition = position;
        position -= origin;
        position *= baseData.inverseTraversalAssitanceScale;
        baseComponents.wUIMenuContainer.transform.position = origin - position;
    }
    internal (bool hit, float distance) CastPlane(Ray ray, float offset = 0)
    {
        Plane plane = new Plane(baseComponents.wUIMenuContainer.forward, origin - (baseComponents.wUIMenuContainer.forward * offset));
        float distance = 0;
        bool hit = plane.Raycast(ray, out distance);
        return (hit, distance);
    }


    private void BillboardAll()
    {
        foreach (WUIElement wUIElement in baseComponents.wUIElements)
        {
            if (wUIElement == null)
                continue;
            if (wUIElement.billboardType != WUIElementBillboardType.Constant)
                continue;
            if (!wUIElement.gameObject.activeInHierarchy)
                continue;

            Billboard(wUIElement);
        }
    }
    private void Billboard(WUIElement wUIElement)
    {
        wUIElement.transform.rotation = Quaternion.LookRotation(user.controllableCamera.transform.forward);
    }


    internal void ShowTip(WUITipable wUITipable)
    {
        baseComponents.wUITip.Show(baseData.deltaPosition, wUITipable);
    }
    internal void HideTip()
    {
        baseComponents.wUITip.Hide();
    }

    internal void Edit(WUICurveValue wUICurveValue)
    {
        baseComponents.wUICurveMenu.Edit(wUICurveValue);
    }
    internal void Edit(WUITextureReferenceValue wUITextureReferenceValue)
    {
        baseComponents.textureMenu.Open(wUITextureReferenceValue);
    }
    internal void Edit(WUIObjectReferenceValue wUIObjectReferenceValue)
    {
        baseComponents.objectMenu.Open(wUIObjectReferenceValue);
    }
    internal void Edit(WUIValue wUIValue)
    {
        baseComponents.colorMenu.Open(wUIValue);
    }
    internal void Edit(WUIEnumValue wUIEnumValue)
    {
        baseComponents.enumMenu.Open(wUIEnumValue);
    }
    internal void Edit(TMP_InputField inputField)
    {
        if (inputField.contentType == TMP_InputField.ContentType.DecimalNumber || inputField.contentType == TMP_InputField.ContentType.IntegerNumber)
            baseComponents.numberPad.BeginEdit(inputField.text, (str) => { inputField.text = str; }, inputField.contentType == TMP_InputField.ContentType.DecimalNumber);
        else
            baseComponents.keyboard.BeginEdit(inputField.text, (str) => { inputField.text = str; });
    }
    internal void Edit(string text, Action<string> callback, bool numeric = false)
    {
        if (numeric)
            baseComponents.numberPad.BeginEdit(text, callback);
        else
            baseComponents.keyboard.BeginEdit(text, callback);
    }
    internal void Edit(WUISandboxValueReference wUISandboxValueReference)
    {
        baseComponents.wUISandboxValueReferenceObjectMenu.Edit(wUISandboxValueReference);
    }
    internal void Edit(Module module, Action<SandboxValue> callback, List<Type> types)
    {
        baseComponents.wUISandboxValueReferenceMenu.Edit(module, callback, types);
    }

    internal void Confirm(string intent, Action action)
    {
        baseComponents.wUIMenuConfirm.Confirm(intent, action);
    }

    internal void RebuildVisibleLayouts()
    {
        RebuildLayoutRecursive((RectTransform)base.transform);
    }
    internal static void RebuildLayoutRecursive(RectTransform parentTransform)
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            RectTransform childTransform = (RectTransform)parentTransform.GetChild(i);
            if (childTransform.gameObject.activeSelf)
                RebuildLayoutRecursive(childTransform);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentTransform);
    }
    #endregion
}