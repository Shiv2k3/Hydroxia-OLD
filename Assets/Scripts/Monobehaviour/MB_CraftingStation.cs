using System.Collections.Generic;
using UnityEngine;

public class MB_CraftingStation : MonoBehaviour
{
    public SO_CraftingRecipe[] recipes;
    List<SO_Item> inventoryItems;
    int i;
    public void CraftItem(SO_CraftingRecipe recipe, AB_MB_Inventory inventory, out SO_Item returnItem, out int returnAmount)
    {
        returnItem = null;
        returnAmount = 0;

        inventoryItems = new List<SO_Item>(inventory.TotalInventorySlots);
        for (i = 0; i < inventory.TotalInventorySlots; i++)
        {
            SO_Item slotItem = inventory.GetItem(i);
            if (slotItem != null && DoesListContain(recipe.ingredients, slotItem) && !inventoryItems.Contains(slotItem))
                inventoryItems.Add(slotItem);
        }
        inventoryItems.TrimExcess();
        if (inventoryItems.Count != recipe.ingredients.Length)
        {
            Debug.Log("Not enough items are available");
            returnItem = null;
            returnAmount = 0;
            return;
        }
        else if (inventoryItems.Count == recipe.ingredients.Length)
        {
            for (i = 0; i < Length(recipe); i++)
            {
                if (inventory.GetTotalItemCount(recipe.ingredients[i].item) < recipe.ingredients[i].amount) // TOTAL NUMBER OF ITEMS < REQUIRED
                {
                    Debug.Log("Not enough items are available");
                    return;
                }
            }
            returnItem = recipe.returnItem;
            returnAmount = recipe.returnAmount;
            for (int i = 0; i < Length(recipe); i++)
            {
                inventory.RemoveFirstItemFromInventory(recipe.ingredients[i].item, recipe.ingredients[i].amount);
            }
            return;
        }
        return;

    }
    private int Length(SO_CraftingRecipe recipe) => recipe.ingredients.Length;
    private bool DoesListContain(Ingredient[] array, SO_Item item)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].item == item) return true;
        }

        return false;
    }
}