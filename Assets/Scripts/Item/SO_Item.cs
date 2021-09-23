using Sirenix.OdinInspector;
using UnityEngine;
public class SO_Item : ScriptableObject
{
    [BoxGroup("Item Settings")]
    new public string name = "New Item";

    [BoxGroup("Item Settings"), PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public Sprite icon;

    [BoxGroup("Item Settings"), PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public MB_DroppedItem droppedPrefab;

    [BoxGroup("Item Settings")]
    public int maxHoldAmountInventory = 10;

}