using UnityEngine;
using UnityEngine.UI;

public abstract class AB_MB_SlotUI : MonoBehaviour
{
    [HideInInspector] protected SO_Item _item;
    [HideInInspector] protected int _count;
    [SerializeField] protected Image _icon;
    [SerializeField] protected Image background;
    [HideInInspector] protected Color _iconColor;
    [HideInInspector] protected MB_DisplayPlayerUI DisplayUI;
    [HideInInspector] protected PlayerInteraction input;
    [HideInInspector] protected int _index;

    public int GetIndex => _index;
    public void SetIndex(int index)
    {
        _index = index;
        _SetIndex(index);
    }
    protected virtual void _SetIndex(int index) => _index = index;
    public SO_Item GetItem => _item;

    protected virtual void Awake()
    {
        Transform parent = transform.parent;
        MB_DisplayPlayerUI display;
        while (DisplayUI == null)
        {
            display = parent.GetComponent<MB_DisplayPlayerUI>();
            if (display)
                DisplayUI = display;
            else
                parent = parent.parent;
        }
        if (DisplayUI == null)
        {
            Debug.Log("Couldn't find a parent with MB_PlayerInventoryDisplay");
        }

        _iconColor = _icon.color;

        input = new PlayerInteraction();
        input.Enable();
    }
}
