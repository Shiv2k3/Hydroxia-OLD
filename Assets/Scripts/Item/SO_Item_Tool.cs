using Sirenix.OdinInspector;
using UnityEngine;
public class SO_Item_Tool : SO_Item
{
    [BoxGroup("Tool Settings")]
    public ToolType toolType;
    [BoxGroup("Tool Settings")]
    public int damage = 10;
}
public enum ToolType { sword, axe, pickaxe };
