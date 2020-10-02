using UnityEngine;

internal enum WUIElementBillboardType
{
    None,
    Initial,
    Constant
}

internal class WUIElement : MonoBehaviour
{
    internal WUI wUI;

    [SerializeField]
    internal WUIElementBillboardType billboardType;

    protected virtual void Awake()
    {
        Register();
    }

    protected void Register()
    {
        wUI = base.gameObject.GetComponentInParent<WUI>();
        wUI.Register(this);
    }
}