using UnityEngine;
public class SO_Inventory : ScriptableObject
{
    public ItemSlot[] slots;
}
[System.Serializable]
public class ItemSlot
{
    public SO_Item item;
    public int count;
}