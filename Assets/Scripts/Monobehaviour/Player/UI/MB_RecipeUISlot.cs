using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MB_RecipeUISlot : AB_MB_SlotUI, IPointerDownHandler
{
    [SerializeField] private Image onSelected;
    [SerializeField] private TextMeshProUGUI _countText;
    [HideInEditorMode] public SO_CraftingRecipe recipe;

    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdateSlot(SO_CraftingRecipe recipe, int amount)
    {
        if (_icon)
        {
            _count = amount;
            if (recipe) _item = recipe.returnItem;
            this.recipe = recipe;

            if (recipe)
            {
                _iconColor.a = 1;

                _icon.color = _iconColor;
                _icon.sprite = recipe.returnItem.icon;
                _countText.text = amount.ToString();
            }
            else
            {
                _iconColor.a = 0;

                _icon.color = _iconColor;
                _icon.sprite = null;

                _countText.text = "";
            }
        }
        else Debug.Log("Tried to change item icon of " + this + "but couldn't find a refernce to it", this);
    }
    public static MB_RecipeUISlot selectedSlot;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectedSlot)
            selectedSlot.onSelected.enabled = false;

        onSelected.enabled = true;
        selectedSlot = this;

        DisplayUI.UpdateIngridentSlots(false);
    }
}
