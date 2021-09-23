using UnityEngine;
using Sirenix.OdinInspector;
public class SO_CraftingRecipe : ScriptableObject
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left)] public SO_Item returnItem;
    public int returnAmount;
    public Ingredient[] ingredients;
}
[System.Serializable]
public struct Ingredient
{
    public SO_Item item;
    public int amount;
}