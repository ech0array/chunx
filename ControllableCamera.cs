using System;
using UnityEngine;


[RequireComponent(typeof(Camera))]
internal sealed class ControllableCamera : MonoBehaviour
{
    #region Values
    internal User user { get; private set; }
    internal Camera camera { get; private set; }

    internal enum Mode
    {
        Runtime,
        Module,
        WUI,
        Widget,
        Weld,
        Unweld,
        Link,
        Unlink,
        QuickMove,
        QuickRotate,
        QuickScale,
        MimicPosition,
        MimicRotation,
        MimicScale,
        MimicAll
    }
    private Mode _mode;
    internal Mode mode
    {
        get
        {
            return _mode;
        }
    }

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIInteractor wUIInteractor;
        [SerializeField]
        internal EditableInteractor editableInteractor;
        [SerializeField]
        internal WidgetInteractor widgetInteractor;
        [SerializeField]
        internal ConnectionInteractor connectionInteractor;
        [SerializeField]
        internal QuickTransformInteractor quickTransformInteractor;

        [SerializeField]
        internal CameraFilter cameraEffector;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Movement
    {
        [SerializeField]
        internal float speed;
        [SerializeField]
        internal float wUISpeed;
    }
    [SerializeField]
    private Movement _movement;

    [Serializable]
    private class Looking
    {
        [SerializeField]
        internal float speed;
        [SerializeField]
        internal float wUISpeed;
        [SerializeField]
        internal float preivewScale;
        [SerializeField]
        internal Vector2 verticalRange;

        internal bool previewing;
    }
    [SerializeField]
    private Looking _looking;


    private static Color _defaultBackgroundColor;

    internal TransformData enteredSelectionTransform { get; private set; }

    #endregion

    #region Unity Framework Entry Functions
    private void Awake()
    {
        camera = base.GetComponent<Camera>();
        _defaultBackgroundColor = camera.backgroundColor;
    }
    private void Update()
    {
        if (user == null)
            return;

        DevelopmentHead.Track($"camera_position - {base.gameObject.name.ToLower()}", base.transform.position.ToString());
        DevelopmentHead.Track($"camera_mode", _mode.ToString().ToLower());

        Move(user.input.move);
        Look(user.input.look);

        ActionA();
        ActionB();
        ActionX();
        ActionMenu();
    }
    #endregion

    #region Extending Functions (Controllable)
    internal void Register(User user)
    {
        this.user = user;

        _components.editableInteractor.Register(this);
        _components.wUIInteractor.Register(this);
        _components.widgetInteractor.Register(this);
        _components.connectionInteractor.Register(this);
        _components.quickTransformInteractor.Register(this);
        EnterMode(Mode.WUI, null);
    }
    internal Vector3 cursorPosition;
    internal void Move(Vector2 value)
    {
        if (_mode == Mode.Runtime)
            return;

        bool isInWUI = _mode == Mode.WUI;

        if (isInWUI)
        {
            Vector2 wuiScreenPosition = camera.WorldToScreenPoint(_components.wUIInteractor.wUI.focalWUIMenu.transform.position);
            wuiScreenPosition -= new Vector2(Screen.width / 2f, Screen.height / 2f);

            bool right = wuiScreenPosition.x < 0f;
            bool up = wuiScreenPosition.y < 0f;

            float distance = _components.wUIInteractor.GetDistanceToWUI(base.transform.forward);
            Vector3 cursorHitPosition = base.transform.position + (base.transform.forward * distance);
            float distanceToCenter = Vector3.Distance(_components.wUIInteractor.wUI.focalWUIMenu.transform.position, cursorHitPosition);

            float distanceScale = 6f * (distance / 3f);
            distanceScale = Mathf.Min(distanceScale, 6f);

            if (distanceToCenter > distanceScale)
            {
                float scale = 1f - ((distanceToCenter - distanceScale) / distanceScale);
                scale = Mathf.Min(scale, 1f);
                if (right && value.x > 0f || !right && value.x < 0f)
                    value.x *= scale;
            }



            bool movingForward = value.y > 0f;
            distance = _components.wUIInteractor.GetDistanceToWUI(base.transform.forward, 3f);

            if (distance < 3f)
            {
                float scale = distance / 3f;
                if(movingForward)
                    value.y *= distance / 3f;
            }

            if (distance > 6f)
            {
                float scale = Mathf.Max(1f - ((distance - 6f) / 3f), 0f);
                if (!movingForward)
                    value.y *= scale;
            }
        }

        float speed = isInWUI ? _movement.wUISpeed : _movement.speed;
        Vector3 scaledDirection = Vector3.zero;
        scaledDirection += base.transform.forward * value.y * Time.deltaTime;
        scaledDirection += base.transform.right * value.x * Time.deltaTime;

        base.transform.position += scaledDirection * speed;
    }
    internal void Look(Vector2 value)
    {
        if (_mode == Mode.Runtime)
            return;

        bool isInWUI = _mode == Mode.WUI;

        if (isInWUI)
        {

            Vector2 wuiScreenPosition = camera.WorldToScreenPoint(_components.wUIInteractor.wUI.focalWUIMenu.transform.position);
            wuiScreenPosition -= new Vector2(Screen.width / 2f, Screen.height / 2f);

            bool right = wuiScreenPosition.x < 0f;
            bool up = wuiScreenPosition.y < 0f;


            float distance = _components.wUIInteractor.GetDistanceToWUI(base.transform.forward);
            cursorPosition = base.transform.position + (base.transform.forward * distance);
            float distanceToCenter = Vector3.Distance(_components.wUIInteractor.wUI.focalWUIMenu.transform.position, cursorPosition);


            float distanceScale = 6f * (distance / 3f);
            distanceScale = Mathf.Min(distanceScale, 6f);

            if (distanceToCenter > distanceScale)
            {
                float scale = 1f - ((distanceToCenter - distanceScale) / distanceScale);
                scale = Mathf.Min(scale , 1f);
                if(right && value.x > 0f || !right && value.x < 0f)
                    value.x *= scale;
                if (up && value.y < 0f || !up && value.y > 0f)
                    value.y *= scale;
            }
        }

        Vector3 lookRotation = Vector3.zero;

        value *= (isInWUI ? _looking.wUISpeed : _looking.speed) * (_mode == Mode.Module && _looking.previewing ? _looking.preivewScale : 1f);
        lookRotation.x += value.y;
        lookRotation.y += value.x;

        lookRotation = base.transform.eulerAngles + (lookRotation * Time.deltaTime);
        lookRotation.x = ClampAngle(lookRotation.x, _looking.verticalRange);
        base.transform.eulerAngles = lookRotation;

    }

    internal void ActionA()
    {
        if (user.input.actionA && !user.deltaInput.actionA)
        {
            _components.wUIInteractor.ActionClick();
            _components.editableInteractor.ActionSelect();
            _components.widgetInteractor.ActionDown();
            _components.quickTransformInteractor.ActionClick();
            _components.connectionInteractor.ActionClick();
            _components.quickTransformInteractor.ActionExit();
        }
        else if (user.deltaInput.actionA && !user.input.actionA)
        {
        }
        else
        {
        }
    }
    internal void ActionB()
    {
        if (user.input.actionB && !user.deltaInput.actionB)
        {
            _components.wUIInteractor.ActionExit();
            _components.widgetInteractor.ActionExit();
            _components.connectionInteractor.ActionExit();
            _components.quickTransformInteractor.ActionExit();
        }
        else if (user.deltaInput.actionB && !user.input.actionB)
        {
        }
        else
        {
        }
    }
    internal void ActionX()
    {
        if (user.input.actionX && !user.deltaInput.actionX)
        {
            if (GameHead.instance.gameState == GameState.Editor)
            {
                EnterMode(Mode.WUI);

                if (user.editorWUI.isStackEmpty)
                {
                    user.editorWUI.Position(base.transform.position + (base.transform.forward * 4f));
                    user.editorWUI.wUIMenuEditorCreateModule.Stack();
                }
            }
        }
        else if (user.deltaInput.actionX && !user.input.actionX)
        {
        }
        else
        {
        }
    }
    internal void ActionMenu()
    {
        if (user.input.menu && !user.deltaInput.menu)
        {
            if (GameHead.instance.gameState == GameState.Editor)
            {
                EnterMode(Mode.WUI);
                if (user.editorWUI.isStackEmpty)
                {
                    user.editorWUI.Position(base.transform.position + (base.transform.forward * 4f));
                }
                user.editorWUI.wUIMenuEditorStart.Stack();
            }
            if (GameHead.instance.isPreviewOrRuntime)
            {
                EnterMode(Mode.WUI);
                if (user.runtimeWUI.isStackEmpty)
                    user.runtimeWUI.Position(base.transform.position + (base.transform.forward * 4f));
                user.runtimeWUI.wUIMenuRuntimeStart.Stack();
            }
        }
        else if (user.deltaInput.menu && !user.input.menu)
        {
        }
        else
        {
        }
    }
    #endregion

    #region Functions
    internal void EnterMode(Mode mode, Module module = null, Vector3 cursorPosition = default)
    {
        Mode lastMode = _mode;
        _mode = mode;

        user.hUD?.SetReticlePassive();
        ModuleHead.instance.HideSpoofConnection();

        if (GameHead.instance.gameState == GameState.Editor)
            user.editorWUI.wUIEditablePreview.Hide();
        
        _components.widgetInteractor.EnterMode(WidgetInteractor.Mode.None, null);
        _components.editableInteractor.EnterMode(EditableInteractor.Mode.None, null);
        _components.wUIInteractor.EnterMode(WUIInteractor.Mode.None, null);
        _components.connectionInteractor.EnterMode(ConnectionInteractor.Mode.None, Vector3.zero, null);
        _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.None, Vector3.zero, null);

        switch (lastMode)
        {
            case Mode.Module:
                break;
            case Mode.WUI:
                break;
            case Mode.Widget:
                break;
            default:
                break;
        }

        RevertTransform();
        switch (_mode)
        {
            case Mode.Runtime:
                Reattach();
                break;
            case Mode.WUI:
                if (GameHead.instance.gameState == GameState.Editor)
                {
                    if (module != null)
                    {
                        SetPreviewing(false);
                        user.editorWUI.wUIEditablePreview.Hide();

                        if (module is ModuleLogicLink)
                            user.editorWUI.wUILoomInpsector.Inspect((ModuleLogicLink)module, cursorPosition);
                        else
                            user.editorWUI.wUIEditableInspectorMenu.Inspect(module, cursorPosition);

                        enteredSelectionTransform = new TransformData(base.transform);
                    }
                }
                base.gameObject.transform.SetParent(UserHead.instance.components.controllableContainer);
                _components.wUIInteractor.EnterMode(WUIInteractor.Mode.Selection, module);
                break;
            case Mode.Module:
                _components.editableInteractor.EnterMode(EditableInteractor.Mode.Selection, module);
                break;
            case Mode.Widget:
                _components.widgetInteractor.EnterMode(WidgetInteractor.Mode.Interact, module);
                break;
            case Mode.Weld:
                _components.connectionInteractor.EnterMode(ConnectionInteractor.Mode.Weld, cursorPosition, module);
                break;
            case Mode.Unweld:
                _components.connectionInteractor.EnterMode(ConnectionInteractor.Mode.Unweld, cursorPosition, module);
                break;
            case Mode.Link:
                _components.connectionInteractor.EnterMode(ConnectionInteractor.Mode.Link, cursorPosition, module);
                break;
            case Mode.Unlink:
                _components.connectionInteractor.EnterMode(ConnectionInteractor.Mode.Unlink, cursorPosition, module);
                break;
            case Mode.QuickMove:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.QuickMove, cursorPosition, module);
                break;
            case Mode.QuickRotate:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.QuickRotate, cursorPosition, module);
                break;
            case Mode.QuickScale:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.QuickScale, cursorPosition, module);
                break;
            case Mode.MimicPosition:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.MimicPosition, cursorPosition, module);
                break;
            case Mode.MimicRotation:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.MimicRotation, cursorPosition, module);
                break;
            case Mode.MimicScale:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.MimicScale, cursorPosition, module);
                break;
            case Mode.MimicAll:
                _components.quickTransformInteractor.EnterMode(QuickTransformInteractor.Mode.MimicAll, cursorPosition, module);
                break;
        }
    }

    internal void SetPreviewing(bool value)
    {
        _looking.previewing = value;
    }

    internal void RevertTransform()
    {
        if (enteredSelectionTransform == null)
            return;
        enteredSelectionTransform.ApplyTo(base.transform);
        enteredSelectionTransform = null;
    }
    private void Reattach()
    {
        if (user.attachement != null)
            user.attachement.OnAttach(user);
    }
    #endregion

    #region Helper Functions
    internal void AppropriateRect(int userIndex, int userCount)
    {
        if (userCount == 1)
            return;

        float x = 0;
        float y = userIndex < 2 ? 0.5f : 0;
        if (userCount == 2)
            y = userIndex == 0 ? 0.5f : 0;
        else if (userCount == 3)
            x = userIndex < 2 ? (userIndex % 2 == 0 ? 0 : 0.5f) : 0.25f;
        else
            x = userIndex % 2 == 0 ? 0 : 0.5f;
        camera.rect = new Rect(x, y, userCount == 2 ? 1 : 0.5f, 0.5f);
    }

    internal static void PositionAllAt(Vector3 value)
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i].transform.position = value;
    }
    internal static void RotateAllTo(Quaternion value)
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i].transform.rotation = value;
    }
    internal static void LookAllAt(Vector3 value)
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i].transform.LookAt(value);
    }

    internal static void SetAllMode(Mode mode)
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i].EnterMode(mode);
    }

    internal static void SetAllColor(Color color)
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i].camera.backgroundColor = color;
    }

    internal static void SetEffectAll (ModuleEnvironmentalCameraSettingsData environmentalCameraSettingsData)
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i]._components.cameraEffector.cameraEffectData = environmentalCameraSettingsData;
    }

    internal static void RestoreAllColor()
    {
        ControllableCamera[] controllableCameras = FindObjectsOfType<ControllableCamera>();
        for (int i = 0; i < controllableCameras.Length; i++)
            controllableCameras[i].camera.backgroundColor = _defaultBackgroundColor;
    }

    private float ClampAngle(float angle, Vector2 minMax)
    {
        if (angle > 180)
            angle = -(360 - angle);

        return Mathf.Clamp(angle, minMax.x, minMax.y);
    }
    #endregion
}