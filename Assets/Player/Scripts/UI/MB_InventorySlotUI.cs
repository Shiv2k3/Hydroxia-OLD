using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MB_InventorySlotUI : AB_MB_SelectableUISlot, IPointerDownHandler
{
    [SerializeField] private Image hotbarSelected;
    [SerializeField] private TextMeshProUGUI indexUI;
    public void UpdateHotbarSelection(bool selected) => hotbarSelected.enabled = selected;
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
