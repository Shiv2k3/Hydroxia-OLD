using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MB_IngredientUISlot : AB_MB_SlotUI
{
    [SerializeField] private TextMeshProUGUI _countText;

    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdateSlot(SO_Item item, int amount = 0)
    {
        if (_icon)
        {
            _item = item;

            if (item)
            {
                _iconColor.a = 1;

                _icon.color = _iconColor;
                _icon.sprite = item.icon;
                _countText.text = amount.ToString();

                background.enabled = true;
            }
            else
            {
                _iconColor.a = 0;

                _icon.color = _iconColor;
                _icon.sprite = null;

                _countText.text = "";

                background.enabled = false;
            }
        }
        else Debug.Log("Tried to change item icon of " + this + "but couldn't find a refernce to it", this);
    }
}
