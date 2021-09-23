using UnityEngine;
public class SO_Item_Armor : SO_Item
{
    [Header("Armor Settings")]
    public ArmorType armorType;
    public int defence = 10;
}
public enum ArmorType { Helmet, Chestplate, Leggings };