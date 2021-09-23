using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AB_MB_SelectableUISlot : AB_MB_SlotUI
{
    [SerializeField] protected Image selectedImage;
    [SerializeField] protected TextMeshProUGUI _countText;
    public void PointerDown(PointerEventData eventData)
    {
        if (DisplayUI.selection1 && !DisplayUI.selection2 && !input.Player.SecondaryAttack.triggered)   // SELECTION 2
        {
            DisplayUI.selection2 = this;
            //Debug.Log("Selection 2");
        }

        if (!DisplayUI.selection1)    // SELECTION 1
        {
            DisplayUI.selection1 = this;
            // Debug.Log("Selection 1");
            if (DisplayUI.selection1.GetItem) DisplayUI.selection1.SetSelectionImageState(true);
        }

        if (DisplayUI.selection1.GetItem == null)
        {
            DisplayUI.ClearSelection();
            //Debug.Log("Selection is invalid");
        }
        else
        if (DisplayUI.selection1 == DisplayUI.selection2) // SAME SLOT SELECTED
        {
            DisplayUI.ClearSelection();
            // Debug.Log("Selections are the same slot");
        }

        if (DisplayUI.selection1 && DisplayUI.selection2) // SWAP SLOT
        {
            if (DisplayUI.selection1.GetItem == DisplayUI.selection2.GetItem && DisplayUI.selection1.GetItem != null)
            {
                //Debug.Log("Selections are the same item, adding selection1 to 2");

                while (DisplayUI.selection2.GetCount < DisplayUI.selection2.GetItem.maxHoldAmountInventory && DisplayUI.selection1.GetCount > 0)
                {
                    if (DisplayUI.selection2.GetCount < DisplayUI.selection2.GetItem.maxHoldAmountInventory)
                        DisplayUI.selection2.SetCount(DisplayUI.selection2.GetCount + 1);
                    if (DisplayUI.selection1.GetCount > 0)
                        DisplayUI.selection1.SetCount(DisplayUI.selection1.GetCount - 1);
                }
                if (DisplayUI.selection1.GetCount <= 0) DisplayUI.selection1.SetItem(null);
                DisplayUI.ClearSelection();
            }
            else
            {
                SwapSelections();
                DisplayUI.ClearSelection();
            }
        }

        void SwapSelections()
        {
            SO_Item item = DisplayUI.selection1.GetItem;
            int count = DisplayUI.selection1.GetCount;

            DisplayUI.selection1.UpdateSlot(DisplayUI.selection2.GetItem, DisplayUI.selection2.GetCount);
            DisplayUI.selection2.UpdateSlot(item, count);

            DisplayUI.UpdateInventoryFromUI(DisplayUI.selection1.GetIndex);
            DisplayUI.UpdateInventoryFromUI(DisplayUI.selection2.GetIndex);

            if (DisplayUI.selection1 is MB_ChestUISlot || DisplayUI.selection2 is MB_ChestUISlot)
            {
                DisplayUI.UpdateChestInventoryFromUI(DisplayUI.selection1.GetIndex);
                DisplayUI.UpdateChestInventoryFromUI(DisplayUI.selection2.GetIndex);
            }

            //Debug.Log("Selections swapped");
        }
    }

    #region HELPERS
    public void UpdateSlot(SO_Item item, int amount)
    {
        if (_icon)
        {
            _count = amount;
            _item = item;

            if (item != null && _count != 0)
                EnableSlot(item, amount);
            else
                DisableSlot();
        }
        else Debug.Log("Tried to change item icon of " + this + "but couldn't find a refernce to it", this);
    }
    private void DisableSlot()
    {
        _iconColor.a = 0;

        _icon.color = _iconColor;
        _icon.sprite = null;

        _countText.text = "";
    }
    private void EnableSlot(SO_Item item, int amount)
    {
        _iconColor.a = 1;

        _icon.color = _iconColor;
        _icon.sprite = item.icon;

        if (amount == 0)
            _countText.text = "";
        else
            _countText.text = amount.ToString();
    }
    public void SetItem(SO_Item item)
    {
        _item = item;
        if (!_item)
            SetCount(0);
    }
    public int GetCount => _count;
    public void SetCount(int count)
    {
        _count = count;
        if (_count != 0) _countText.text = _count.ToString();
        else _countText.text = "";
    }
    public void SetSelectionImageState(bool state) => selectedImage.enabled = state;
    public bool GetSelectoinImageState => selectedImage.enabled;
    #endregion
}
