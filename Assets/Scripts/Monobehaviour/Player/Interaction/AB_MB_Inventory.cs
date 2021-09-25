using UnityEngine;
using Sirenix.OdinInspector;

public abstract class AB_MB_Inventory : MonoBehaviour
{
    [SerializeField] protected SO_Inventory inventory;
    private void Awake()
    {
        if (inventory == null) Debug.LogError("Inventory is null", this);
    }
    public bool ContainsItem(SO_Item item, int count = 1)
    {
        if (count == 1)
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].item == item)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].count == count && inventory.slots[i].item == item)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public SO_Item GetItem(int slotIndex) => inventory.slots[slotIndex]?.item;
    public int GetCount(int slotIndex)
    {
        if (inventory.slots[slotIndex] != null) return inventory.slots[slotIndex].count;
        else return 0;
    }
    public int GetTotalItemCount(SO_Item item)
    {
        int totalCount = 0;
        for (int i = 0; i < TotalInventorySlots; i++)
        {
            if (GetItem(i) == item) totalCount += GetCount(i);
        }
        return totalCount;
    }
    public void SetItem(int index, SO_Item item, int count)
    {
        inventory.slots[index].item = item;
        inventory.slots[index].count = count;
    }
    public void SetCount(int index, int count)
    {
        inventory.slots[index].count = count;

        if (GetCount(index) <= 0)
            SetItem(index, null, 0);
    }
    public int TotalInventorySlots => inventory.slots.Length;
    private bool IsSlotEmpty(int i) => !inventory.slots[i]?.item;
    public void AddDroppedItemToInventory(MB_DroppedItem droppedItem)
    {
        // Look through all the slots
        for (int i = 0; i < inventory.slots.Length; i++)
            if (IsSlotEmpty(i))
            {
                inventory.slots[i].item = droppedItem.itemSO;

                inventory.slots[i].count = 1;
                Destroy(droppedItem.gameObject);

                Debug.Log("Added " + droppedItem.itemSO.name + " to player's inventory");
                return;
            }
            else if (inventory.slots[i].item == droppedItem.itemSO && inventory.slots[i].count < droppedItem.itemSO.maxHoldAmountInventory)
            {
                inventory.slots[i].count++;
                Destroy(droppedItem.gameObject);

                Debug.Log("Added " + droppedItem.itemSO.name + " to player's inventory");
                return;
            }

        Debug.Log("Couldn't place " + droppedItem.itemSO.name + " to player's inventory");
    }
    public void AddItemToInventory(SO_Item item, int amount)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
            if (IsSlotEmpty(i))
            {
                inventory.slots[i].item = item;

                inventory.slots[i].count = amount;

                Debug.Log("Added " + item?.name + " to player's inventory");
                return;
            }
            else if (inventory.slots[i].item == item && inventory.slots[i].count < item.maxHoldAmountInventory)
            {
                inventory.slots[i].count += amount;

                Debug.Log("Added " + item?.name + " to player's inventory");
                return;
            }

        Debug.Log("Couldn't place " + item?.name + " to player's inventory");
    }
    public void RemoveFirstItemFromInventory(SO_Item item, int removeAmount)
    {
        //Debug.Log(item.name + "'s to be removed " + count + " times");
        int totalRemoved = 0;
        for (int i = 0; i < TotalInventorySlots; i++)
        {
            if (GetItem(i) == item)
            {
                for (int j = 0; GetCount(i) > 0 && removeAmount > 0; j++)
                {
                    inventory.slots[i].count--;
                    removeAmount--;
                    totalRemoved++;
                }

                if (GetCount(i) <= 0)
                    SetItem(i, null, 0);
                if (removeAmount <= 0)
                {
                    //Debug.Log("Removed " + item.name + " " + removed + " times");
                    SetItem(i, null, 0);
                    return;
                }
            }
        }
        // Debug.Log(item.name + " couldn't be removed");
    }
    public void DropItem(int index, int count)
    {
        var itemToDrop = GetItem(index);
        if (itemToDrop)
        {
            int beforeDrop = GetCount(index);

            SetCount(index, GetCount(index) - count);

            Debug.Log("Dropping Item from inventory");
            TileManager.DropItem(itemToDrop, beforeDrop - GetCount(index), transform.position);
        }
        else
        {
            Debug.Log("Couldn't drop item becasue it doens't exist");
        }
    }
}