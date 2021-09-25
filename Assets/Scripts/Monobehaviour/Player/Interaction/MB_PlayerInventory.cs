using UnityEngine;

public class MB_PlayerInventory : AB_MB_Inventory
{
    [SerializeField] public int slotCount = 10;
    [SerializeField] public int hotbarCount = 10;
    [SerializeField] public bool clearOnPlay;
    private void Awake()
    {
        if (inventory)
        {
            if (clearOnPlay || inventory.slots == null || inventory.slots.Length != slotCount + hotbarCount)
                inventory.slots = new ItemSlot[slotCount + hotbarCount];
        }
        else Debug.Log("Inventory on player is null");
    }
}
