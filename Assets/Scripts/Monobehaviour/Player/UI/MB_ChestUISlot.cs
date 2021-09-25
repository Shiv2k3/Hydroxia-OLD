using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class MB_ChestUISlot : AB_MB_SelectableUISlot, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI indexUI;
    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown(eventData);
    }

    protected override void _SetIndex(int index)
    {
        base._SetIndex(index);
        indexUI.text = index.ToString();
    }
}
