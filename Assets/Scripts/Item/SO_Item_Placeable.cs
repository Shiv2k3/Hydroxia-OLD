using Sirenix.OdinInspector;
using UnityEngine;

public class SO_Item_Placeable : SO_Item
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left), BoxGroup("Placeable Settings")] public GameObject placedPrefab;
}
