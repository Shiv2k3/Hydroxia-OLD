using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

public class MB_Interaction : MonoBehaviour
{
    #region PARAMETERS
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

    // REFERENCES
    [HideInInspector] private PlayerInteraction actions;
    [HideInInspector] private MB_Movement playerMovement;

    // RAY CAST
    [HideInInspector] private Ray camRay;
    [HideInInspector] private RaycastHit camRayHit;
    [HideInInspector] private Ray playerRay;
    [HideInInspector] private RaycastHit playerRayHit;
    [HideInInspector] public int selectionIndex = 0;

    #endregion
    #region MB
    private void Start()
    {
        playerMovement = GetComponent<MB_Movement>();
        attackCard = new AttackCard();
        interactCard = new InteractionCard(playerMovement, actions);
    }
    private void Update()
    {
        HotbarSelection();
        if (AttackInput || InteractInput || MountInput)
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            // CAST RAY FROM CAMERA
            camRay = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(camRay, out camRayHit);

            if (AttackReady()) // ATTACK 
            {
                if (camRayHit.point! != Vector3.zero)
                {
                    playerRay.origin = raycastPoint.position;
                    playerRay.direction = (camRayHit.point - raycastPoint.position).normalized;
                    Physics.Raycast(playerRay, out playerRayHit, range, attackLayer);
                    attackCard.attackedObject = playerRayHit.transform?.gameObject;
                }
                else
                {
                    attackCard.attackedObject = null;
                    return;
                }

                PerformAttack();
                lastAttacked = Time.time;
                return;
            }

            if (InteractReady()) // INTERACT
            {
                if (camRayHit.transform)
                {
                    playerRay.origin = raycastPoint.position;
                    playerRay.direction = (camRayHit.point - raycastPoint.position).normalized;

                    if (Physics.Raycast(playerRay, out playerRayHit, range, interactLayer))
                        interactCard.interactedObject = playerRayHit.transform.gameObject;
                    else
                    {
                        interactCard.interactedObject = null;
                        return;
                    }
                }
                else
                {
                    interactCard.interactedObject = null;
                    return;
                }

                PerformInteract();
                lastInteracted = Time.time;
                return;
            }
        }
    }
    #endregion
    #region METHODS
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
    }
    private void PerformAttack()
    {
        if (attackCard.weaponEquiped == null) return;

        MB_DroppableItem droppableItem = attackCard.attackedObject.GetComponentInParent<MB_DroppableItem>();
        if (droppableItem)
            droppableItem.Attacked(attackCard);
    }
    private void PerformInteract()
    {
        if (MountInput)
        {
            AB_MB_Mount mount = interactCard.interactedObject.GetComponentInParent<AB_MB_Mount>();
            if (mount) mount.ToggleMount(interactCard);
            return;
        }

        if (!InteractInput) return;

        MB_DroppedItem droppedItem = interactCard.interactedObject.GetComponent<MB_DroppedItem>();
        if ( droppedItem)
        {
            inventory.AddDroppedItemToInventory(droppedItem);
            return;
        }
    }
    #endregion
    #region Bools And MSC
    bool AttackReady()
    {
        if (Time.time > lastAttacked + attackCooldown)
            return true;
        else
            return false;
    }
    bool InteractReady()
    {
        if (Time.time > lastInteracted + interactCooldown)
            return true;
        else
            return false;
    }
    bool AttackInput => actions.Player.Attack.ReadValue<float>() == 1;
    bool InteractInput => actions.Player.Interact.ReadValue<float>() == 1;
    bool MountInput => actions.Player.Mount.ReadValue<float>() == 1;
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

#region CARD CLASS
public class AttackCard
{
    public int Damage => weaponEquiped ? weaponEquiped.damage : 0;
    [ReadOnly] public SO_Item_Tool weaponEquiped;
    [ReadOnly] public GameObject attackedObject;
}
public class InteractionCard
{
    [HideInInspector] public MB_Movement playerMovement;
    [HideInInspector] public PlayerInteraction interactionInput;
    [ReadOnly] public GameObject interactedObject;

    public InteractionCard(MB_Movement playerMovement, PlayerInteraction playerActions)
    {
        this.interactionInput = playerActions;
        this.playerMovement = playerMovement;
    }
}
#endregion