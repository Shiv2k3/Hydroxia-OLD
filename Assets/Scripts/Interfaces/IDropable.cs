public interface IDropable
{
    public C_DroppableItem[] Inventory { get; }
}
[System.Serializable]
public class C_DroppableItem
{
    public SO_Item item;
    public int dropAmount;
}