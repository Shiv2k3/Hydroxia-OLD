using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class MB_Interaction : MonoBehaviour
{
    [FoldoutGroup("Settings")]
    [SerializeField] private new Camera camera;
    [FoldoutGroup("Settings")]
    [SerializeField, InfoBox("Raycast point transform must be on the player, i.e the player's head")] private Transform raycastPoint;
    [FoldoutGroup("Settings")]
    [SerializeField] private float range = 3f;

    [FoldoutGroup("Attack Variables")]
    [SerializeField] private AttackCard attackCard;
    [FoldoutGroup("Attack Variables")]
    [SerializeField] private float attackCooldown = .05f;
    [FoldoutGroup("Attack Variables")]
    [SerializeField] private LayerMask attackLayer;
    [HideInInspector] private float lastAttacked;

    [FoldoutGroup("Interact Variables")]
    [SerializeField] public MB_PlayerInventory inventory;
    [FoldoutGroup("Interact Variables")]
    [SerializeField] private InteractionCard interactCard;
    [FoldoutGroup("Interact Variables")]
    [SerializeField] private float interactCooldown;
    [FoldoutGroup("Interact Variables")]
    [SerializeField] private LayerMask interactLayer;
    [HideInInspector] private float lastInteracted;

    [FoldoutGroup("Build Variables")]
    [SerializeField] private BuildCard buildCard;
    [FoldoutGroup("Build Variables")]
    [SerializeField] private float buildingCooldown = 0.5f;
    [FoldoutGroup("Build Variables")]
    [SerializeField] private LayerMask buildableLayer;
    [HideInInspector] private float lastBuilt;

    // REFERENCES
    [HideInInspector] private PlayerInteraction actions;
    [HideInInspector] public event EventHandler<AttackCard> OnAttack;

    // RAY CAST
    [HideInInspector] private Ray camRay;
    [HideInInspector] private RaycastHit camRayHit;
    [HideInInspector] private Ray playerRay;
    [HideInInspector] private RaycastHit playerRayHit;
    [HideInInspector] public int selectionIndex = 0;

    private void Update()
    {
        HotbarSelection();
        if (AttackInput || InteractInput)
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            // CAST RAY FROM CAMERA
            camRay = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(camRay, out camRayHit);

            if (BuildReady()) // BUILD
            {
                if (camRayHit.point! != Vector3.zero)
                {
                    playerRay.origin = raycastPoint.position;
                    playerRay.direction = (camRayHit.point - raycastPoint.position).normalized;
                    Physics.Raycast(playerRay, out playerRayHit, range, buildableLayer);
                    buildCard.objectHit = playerRayHit.transform?.gameObject;
                }
                else
                {
                    buildCard.objectHit = null;
                    return;
                }

                PerformBuild();
                lastBuilt = Time.time;
                return;
            }

            if (AttackReady()) // ATTACK 
            {
                if (camRayHit.point! != Vector3.zero)
                {
                    playerRay.origin = raycastPoint.position;
                    playerRay.direction = (camRayHit.point - raycastPoint.position).normalized;
                    Physics.Raycast(playerRay, out playerRayHit, range, attackLayer);
                    attackCard.objectHit = playerRayHit.transform?.gameObject;
                }
                else
                {
                    attackCard.objectHit = null;
                    return;
                }

                PerformAttack();
                lastAttacked = Time.time;
                return;
            }

            if (InteractReady()) // INTERACT
            {
                if (camRayHit.point! != Vector3.zero)
                {
                    playerRay.origin = raycastPoint.position;
                    playerRay.direction = (camRayHit.point - raycastPoint.position).normalized;
                    Physics.Raycast(playerRay, out playerRayHit, range, interactLayer);
                    interactCard.objectHit = playerRayHit.transform?.gameObject;
                }
                else
                {
                    interactCard.objectHit = null;
                    return;
                }

                PerformInteract();
                lastInteracted = Time.time;
                return;
            }
        }
    }
    private void PerformBuild()
    {
        if (buildCard.objectHit)
        {
            Debug.Log("Building " + buildCard.placeableHeld.name + " on " + buildCard.objectHit.name);
        }
    }
    private void PerformAttack()
    {
        if (attackCard.Damage != 0)
        {
            OnAttack.Invoke(this, attackCard);
        }
        else
        {
            Debug.Log("Can't attack when you can't deal damage");
        }
    }
    private void PerformInteract()
    {
        MB_DroppedItem droppedItem = interactCard.objectHit?.GetComponent<MB_DroppedItem>();
        if (droppedItem)
        {
            inventory.AddDroppedItemToInventory(droppedItem);
        }
        else
        {
            Debug.Log("Can't interact with this object, maybe the requipred script is on the wrong parent?");
        }
    }
    private void HotbarSelection()
    {
        if (Cursor.lockState == CursorLockMode.None || MouseDelta == 0) return; // ONLY SWAP SELECTION WHEN PLAYING

        selectionIndex -= MouseDelta;
        if (selectionIndex < 0) selectionIndex = inventory.hotbarCount - 1;
        if (selectionIndex > inventory.hotbarCount - 1) selectionIndex = 0;

        SO_Item_Tool tool = inventory.GetItem(selectionIndex) as SO_Item_Tool;
        if (tool)
            attackCard.weaponEquiped = tool;
        else
            attackCard.weaponEquiped = null;
        SO_Item_Placeable placeable = inventory.GetItem(selectionIndex) as SO_Item_Placeable;
        if (placeable)
            buildCard.placeableHeld = placeable;
        else
            buildCard.placeableHeld = null;
    }

    #region Bools And MSC
    bool AttackReady()
    {
        if (Time.time > lastAttacked + attackCooldown && AttackInput && attackCard.weaponEquiped)
            return true;
        else
            return false;
    }
    bool BuildReady()
    {
        if (Time.time > lastBuilt + buildingCooldown && AttackInput && buildCard.placeableHeld)
            return true;
        else
            return false;
    }
    bool InteractReady()
    {
        if (Time.time > lastInteracted + interactCooldown && InteractInput)
            return true;
        else
            return false;
    }
    bool AttackInput => actions.Player.Attack.ReadValue<float>() == 1;
    bool InteractInput => actions.Player.Interact.ReadValue<float>() == 1;
    int MouseDelta => (int)actions.Player.Selection.ReadValue<Vector2>().y;
    private void OnEnable()
    {
        actions = new PlayerInteraction();
        actions.Enable();
    }
    private void OnDisable()
    {
        actions.Disable();
    }
    #endregion
}

[Serializable]
public class AttackCard : EventArgs
{
    public int Damage => weaponEquiped ? weaponEquiped.damage : 0;
    [ShowIf("@weaponEquiped != null")] public SO_Item_Tool weaponEquiped;
    [ShowIf("@objectHit != null")] public GameObject objectHit;
}
[Serializable]
public class BuildCard : EventArgs
{
    [ShowIf("@placeableHeld != null")] public SO_Item_Placeable placeableHeld;
    [ShowIf("@objectHit != null")] public GameObject objectHit;
}
[Serializable]
public class InteractionCard : EventArgs
{
    public object inventory;
    [ShowIf("@objectHit != null")] public GameObject objectHit;

    public InteractionCard(object inventory)
    {
        this.inventory = inventory;
    }
}