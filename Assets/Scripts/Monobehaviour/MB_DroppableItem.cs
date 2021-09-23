using UnityEngine;

public class MB_DroppableItem : MonoBehaviour, IDamageable, IDropable
{
    // DROPABLE 
    public C_DroppableItem[] Inventory => _inventory;
    [Tooltip("Items to drop when destoryed")] [SerializeField] private C_DroppableItem[] _inventory;

    // DAMAGEABLE
    public int Hp => _hp;
    [SerializeField] private int _hp = 100;
    public int Defence => _defence;
    [SerializeField] private int _defence = 10;
    public ToolType ToolType => _damagedByType;
    [SerializeField] private ToolType _damagedByType;

    public void Attacked(AttackCard attackCard)
    {
        if (attackCard.weaponEquiped.toolType != _damagedByType)
        {
            Debug.Log("Can't attack a non attacker");
            return;
        }
        else if (_damagedByType == attackCard.weaponEquiped.toolType)
        {
            _hp -= attackCard.Damage - _defence;
            Debug.Log(name + " took " + (attackCard.Damage - _defence) + " damage, " + "health is now " + _hp);
        }

        if (_hp <= 0)
        {
            StartCoroutine(TileManager.DropTileCoroutine(this));
        }
    }
}