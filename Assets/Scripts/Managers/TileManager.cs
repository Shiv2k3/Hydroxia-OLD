using System.Collections;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager _instance;
    public static TileManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return _instance = FindObjectOfType<TileManager>();
            }
            else
                return _instance;
        }
        private set
        {
            if (_instance == null)
                _instance = value;
            else if (value != null)
                Destroy(value.gameObject);
        }
    }

    [SerializeField] private float timeBetweenDrop = 0.5f;
    private void Awake()
    {
        var players = FindObjectsOfType<MB_Interaction>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].OnAttack += MB_Interaction_OnAttack;
            Debug.Log("Tile Manager Was Subscribed To OnAttack");
        }
    }

    MB_DroppableItem dropable;
    private void MB_Interaction_OnAttack(object sender, AttackCard attackCard)
    {
        dropable = attackCard.objectHit?.GetComponentInParent<MB_DroppableItem>();
        if (dropable)
        {
            dropable.Attacked(attackCard);
        }
        else
            Debug.Log("You can't damage " + attackCard.objectHit?.name + ", make sure the script MB_Dropable is attached to the parent of the collider you are hitting");
    }

    public static IEnumerator DropTileCoroutine(MB_DroppableItem item)
    {
        item.GetComponentInChildren<MeshRenderer>().enabled = false;
        item.GetComponentInChildren<Collider>().enabled = false;

        for (int i = 0; i < item.Inventory.Length; i++)
        {
            for (int j = 0; j < item.Inventory[i].dropAmount; j++)
            {
                int r = Random.Range(0, 360);
                Quaternion rotation = Quaternion.AngleAxis(r, new Vector3(r, r, r));
                Instantiate(item.Inventory[i].item.droppedPrefab, item.transform.position + item.transform.position.normalized, rotation);

                yield return new WaitForSeconds(Instance.timeBetweenDrop);
            }
            Debug.Log("Dropped Item");
        }

        Destroy(item.gameObject);
    }

    public static void DropItem(SO_Item item, int amount, Vector3 position)
    {
        if (item)
        {
            Vector3 offset = position.normalized * 0.95f;
            for (int i = 0; i < amount; i++)
            {
                Instantiate(item.droppedPrefab, position, Random.rotation);
                position += (offset * 1.25f) / (i + 0.5f);
                Debug.Log("Tile Manager dropped item");
            }
            Debug.Log("Dropped All itmes");
        }
        else
        {
            Debug.Log("Drop item is invalid");
        }
    }

}