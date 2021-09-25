using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
public class MB_DisplayPlayerUI : MonoBehaviour
{
    #region PARAMETERS
    [SerializeField, BoxGroup("Panel Info")] private bool tweenPanels = false;
    [SerializeField, BoxGroup("Panel Info")] private RectTransform inventoryPanel;
    [SerializeField, BoxGroup("Panel Info")] private RectTransform hotbarPanel;
    [SerializeField, BoxGroup("Panel Info")] private RectTransform recipePanel;
    [SerializeField, BoxGroup("Panel Info")] private RectTransform ingredientsPanel;
    [SerializeField, BoxGroup("Panel Info")] private RectTransform chestPanel;

    [SerializeField, BoxGroup("Slot Prefabs")] private RectTransform inventorySlotPrefab;
    [SerializeField, BoxGroup("Slot Prefabs")] private RectTransform recipeSlotPrefab;
    [SerializeField, BoxGroup("Slot Prefabs")] private RectTransform ingredientsSlotPrefab;
    [SerializeField, BoxGroup("Slot Prefabs")] private RectTransform chestSlotPrefab;

    [SerializeField, FoldoutGroup("Slot Count")] private int defualtRecipeSlotsCount = 10;
    [SerializeField, FoldoutGroup("Slot Count")] private int defualtIngredientsSlotsCount = 10;
    [SerializeField, FoldoutGroup("Slot Count")] private int defualtChestSlotsCount = 10;

    [SerializeField, ReadOnly, FoldoutGroup("UI Slot Arrays")] private MB_InventorySlotUI[] inventorySlots;
    [SerializeField, ReadOnly, FoldoutGroup("UI Slot Arrays")] private List<MB_RecipeUISlot> recipeSlots;
    [SerializeField, ReadOnly, FoldoutGroup("UI Slot Arrays")] private List<MB_IngredientUISlot> ingredientsSlots;
    [SerializeField, ReadOnly, FoldoutGroup("UI Slot Arrays")] private List<MB_ChestUISlot> chestSlotsUI;

    [Header("Private Items")]
    [HideInInspector] private MB_Interaction interactor;
    [HideInInspector] private MB_PlayerInventory inventory;
    [HideInInspector] private PlayerInteraction input;
    [HideInInspector] private UIPanelManager tween;
    [HideInInspector] private Transform player;
    [HideInInspector] public AB_MB_SelectableUISlot selection1;
    [HideInInspector] public AB_MB_SelectableUISlot selection2;
    #endregion
    #region INVENTORY
    bool uiActive = true;
    private void UpdateInventorySlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].GetItem != inventory.GetItem(i) || inventorySlots[i].GetCount != inventory.GetCount(i))
            {
                inventorySlots[i].UpdateSlot(
                    inventory.GetItem(i),
                    inventory.GetCount(i));
            }
            inventorySlots[i].UpdateHotbarSelection(interactor.selectionIndex == i);
        }
    }
    private void UpdateInventorySlotUIFromList(int index)
    {
        inventorySlots[index].UpdateSlot(inventory.GetItem(index), inventory.GetCount(index));
        inventorySlots[index].UpdateHotbarSelection(interactor.selectionIndex == index);
    }
    public void UpdateInventoryFromUI(int index)
    {
        inventory.SetItem(index, inventorySlots[index].GetItem, inventorySlots[index].GetCount);
    }
    public void ClearSelection()
    {
        selection1?.SetSelectionImageState(false);

        selection1?.UpdateSlot(selection1?.GetItem, selection1.GetCount);
        selection2?.UpdateSlot(selection2?.GetItem, selection2.GetCount);

        selection1 = null;
        selection2 = null;

        //Debug.Log("Ceared selections");
    }
    #endregion
    #region CRAFTING
    Collider[] craftingColliders;
    MB_CraftingStation playerRecipes;
    MB_CraftingStation selectedStation;
    void UpdateCraftingPanel()
    {
        craftingColliders = Physics.OverlapSphere(player.position, 1.5f);

        for (int i = 0; i < craftingColliders.Length; i++)
        {
            selectedStation = craftingColliders[i].GetComponentInParent<MB_CraftingStation>();
            if (selectedStation)
            {
                UpdateIngridentSlots(true);

                if (selectedStation != playerRecipes)
                {
                    UpdateRecipeSlots(selectedStation);

                    Debug.Log("Updated recipe slots using crafting station's recipe");
                    return;
                }
                if (selectedStation == playerRecipes)
                {
                    UpdateRecipeSlots(playerRecipes);
                    //  Debug.Log("Updated recipe slots using player's recipe");

                    return;
                }
            }
        }
    }
    public void UpdateIngridentSlots(bool reset)
    {
        if (reset)
            for (int j = 0; j < ingredientsSlots.Count; j++)
            {
                ingredientsSlots[j].UpdateSlot(null, 0);
            }
        else
        {
            for (int i = 0; i < recipeSlots.Count; i++)
            {
                if (MB_RecipeUISlot.selectedSlot.GetIndex < selectedStation.recipes.Length && recipeSlots[i] == MB_RecipeUISlot.selectedSlot)
                {
                    for (int j = 0; j < ingredientsSlots.Count; j++)
                    {
                        if (j < recipeSlots[i].recipe.ingredients.Length)
                            ingredientsSlots[j].UpdateSlot(recipeSlots[i].recipe.ingredients[j].item, recipeSlots[i].recipe.ingredients[j].amount);
                        else
                            ingredientsSlots[j].UpdateSlot(null);
                    }

                    return;
                }
            }

        }

    }
    void UpdateRecipeSlots(MB_CraftingStation craftingStation)
    {
        if (EnoughSlotsAvailabe(craftingStation.recipes.Length, playerRecipes.recipes.Length))
        {
            UpdateSlots(craftingStation);
        }
        else
        {
            AddSlots(craftingStation);
            Debug.Log("Added more slots to recipe panel");
            UpdateSlots(craftingStation);
        }

        void UpdateSlots(MB_CraftingStation craftingStation)
        {
            for (int j = 0; j < recipeSlots.Count; j++) // UPDATE SLOTS
            {
                if (j < craftingStation.recipes.Length)
                    recipeSlots[j].UpdateSlot(craftingStation.recipes[j], craftingStation.recipes[j].returnAmount);
                else
                    recipeSlots[j].UpdateSlot(null, 0);
            }
        }

        void AddSlots(MB_CraftingStation craftingStation)
        {
            int difference = craftingStation.recipes.Length - recipeSlots.Count;
            for (int j = 0; j < difference; j++) // ADD SLOTS
            {
                recipeSlots.Add(Instantiate(recipeSlotPrefab, recipePanel).GetComponent<MB_RecipeUISlot>());
            }
        }
    }
    bool EnoughSlotsAvailabe(int slot1, int slot2) => slot1 <= slot2;
    public void CraftItem()
    {
        if (MB_RecipeUISlot.selectedSlot && MB_RecipeUISlot.selectedSlot.GetItem) // IF SELECTED ITEM IN MENU IS VALID
        {
            selectedStation.CraftItem(MB_RecipeUISlot.selectedSlot.recipe, inventory, out SO_Item item, out int amount);
            if (item && amount != 0)
                inventory.AddItemToInventory(item, amount);
        }

    }
    #endregion
    #region MONOBEHAVIOUR
    public void Awake()
    {
        player = transform.parent.parent;
        interactor = player.GetComponent<MB_Interaction>();
        inventory = interactor.inventory;
        tween = GetComponent<UIPanelManager>();
    }
    public void Start()
    {
        input = new PlayerInteraction();
        input.Enable();
        input.Player.Inventory.performed += ctx => UI_Performed();
        playerRecipes = player.GetComponent<MB_CraftingStation>();

        AddAllSlots();
        void AddAllSlots()
        {
            inventorySlots = new MB_InventorySlotUI[inventory.TotalInventorySlots];
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (i < inventory.hotbarCount)
                    inventorySlots[i] = Instantiate(inventorySlotPrefab, hotbarPanel.transform).gameObject.GetComponent<MB_InventorySlotUI>();
                else
                    inventorySlots[i] = Instantiate(inventorySlotPrefab, inventoryPanel.transform).gameObject.GetComponent<MB_InventorySlotUI>();

                inventorySlots[i].SetIndex(i);
            }
            Debug.Log("Created inventory slots");

            recipeSlots = new List<MB_RecipeUISlot>(defualtRecipeSlotsCount);
            for (int i = 0; i < defualtRecipeSlotsCount; i++)
            {
                recipeSlots.Add(Instantiate(recipeSlotPrefab, recipePanel).gameObject.GetComponent<MB_RecipeUISlot>());
                recipeSlots[i].SetIndex(i);
            }
            Debug.Log("Created recipe slots");

            ingredientsSlots = new List<MB_IngredientUISlot>(defualtIngredientsSlotsCount);
            for (int i = 0; i < defualtIngredientsSlotsCount; i++)
            {
                ingredientsSlots.Add(Instantiate(ingredientsSlotPrefab, ingredientsPanel).gameObject.GetComponent<MB_IngredientUISlot>());
                ingredientsSlots[i].SetIndex(i);
            }
            Debug.Log("Created ingrident slots");

            chestSlotsUI = new List<MB_ChestUISlot>(defualtChestSlotsCount);
            for (int i = 0; i < defualtChestSlotsCount; i++)
            {
                AddNewChestSlot(i);
            }
            Debug.Log("Created chest slots");
        }

        if (!tweenPanels)
        {
            tween.TweenInChestWindow();
            tween.TweenInCraftingWindow();
            tween.TweenInInventoryWindow();
        }
    }
    public void Update()
    {
        UpdateInventorySlots();
    }
    public void DropItem(bool leftClick)
    {
        if (selection1)
        {
            int selectionIndex = selection1.GetIndex;

            if (selection1 is MB_InventorySlotUI)
            {
                int count = inventory.GetCount(selectionIndex);
                Debug.Log("Left Click Dropping item at index " + selectionIndex);
                inventory.DropItem(selectionIndex, leftClick ? count : 1);

                UpdateInventorySlotUIFromList(selectionIndex);
                ClearSelection();
            }
            else if (selection1 is MB_ChestUISlot)
            {
                int count = selectedChest.GetCount(selectionIndex);
                Debug.Log("Left Click Dropping item at index " + selectionIndex);
                selectedChest.DropItem(selectionIndex, leftClick ? count : 1);

                UpdateChestSlotUIFromList(selectionIndex);
                ClearSelection();
            }
        }
    }
    private void AddNewChestSlot(int index)
    {
        chestSlotsUI.Add(Instantiate(chestSlotPrefab, chestPanel).GetComponent<MB_ChestUISlot>());
        chestSlotsUI[index].SetIndex(index);
    }
    private void UI_Performed()
    {
        uiActive = !uiActive;
        ClearSelection();

        UpdateCraftingPanel();
        if (uiActive) // INVENTORY ENABLED
        {
            Cursor.lockState = CursorLockMode.None;
            if (tweenPanels)
            {
                tween.TweenInInventoryWindow();
                tween.TweenInChestWindow();
                tween.TweenInCraftingWindow();
            }
            else
            {
                tween.inventoryWindow.gameObject.SetActive(true);
                tween.craftingWindow.gameObject.SetActive(true);
                tween.chestWindow.gameObject.SetActive(true);
            }

            UpdateChestPanel();
        }
        else // INVENTORY DISABLED
        {
            Cursor.lockState = CursorLockMode.Locked;

            if (tweenPanels)
            {
                tween.TweenOutInventoryWindow();
                tween.TweenOutCraftingWindow();
                tween.TweenOutChestWindow();
            }
            else
            {
                tween.inventoryWindow.gameObject.SetActive(false);
                tween.craftingWindow.gameObject.SetActive(false);
                tween.chestWindow.gameObject.SetActive(false);
            }
        }
    }
    #endregion
    #region Chest
    Collider[] chestColliders;
    MB_Chest selectedChest;
    void UpdateChestPanel()
    {
        chestColliders = Physics.OverlapSphere(player.position, 1.5f);
        for (int i = 0; i < chestColliders.Length; i++)
        {
            selectedChest = chestColliders[i].GetComponentInParent<MB_Chest>();
            if (selectedChest != null)
            {
                UpdateChestSlots(selectedChest);
                return;
            }
        }

        UpdateChestSlots(null); // NO CHEST FOUND
        void UpdateChestSlots(MB_Chest chest)
        {
            if (chest)
            {
                if (chestSlotsUI.Count >= chest.TotalInventorySlots)  // IF WE HAVE ENOUGH UI SLOTS
                {
                    for (int i = 0; i < chest.TotalInventorySlots; i++) // ENABLE HOWEVER MANY WE NEED
                    {
                        chestSlotsUI[i].gameObject.SetActive(true);
                        chestSlotsUI[i].UpdateSlot(chest.GetItem(i), chest.GetCount(i));
                        // Debug.Log("Slot " + i + " was updated to hold item " + chest.GetItem(i));
                    }
                    for (int i = chest.TotalInventorySlots; i < chestSlotsUI.Count; i++) // DISABLE UNUSED SLOTS
                    {
                        chestSlotsUI[i].gameObject.SetActive(false);
                    }
                }
                else  // IF WE DON'T HAVE ENOUGH UI SLOTS
                {
                    for (int i = 0; i < chestSlotsUI.Count; i++) // ENABLE THE ONES WE HAVE
                    {
                        chestSlotsUI[i].gameObject.SetActive(true);
                        chestSlotsUI[i].UpdateSlot(chest.GetItem(i), chest.GetCount(i));
                        // Debug.Log("Slot " + i + " was updated to hold item " + chest.GetItem(i));
                    }

                    for (int i = chestSlotsUI.Count; i < chest.TotalInventorySlots; i++) // CREATE THE NEW ONES TILL WE HAVE ENOUGH AND ENABLE THEM
                    {
                        AddNewChestSlot(i);

                        chestSlotsUI[i].gameObject.SetActive(true);
                        chestSlotsUI[i].UpdateSlot(chest.GetItem(i), chest.GetCount(i));
                        // Debug.Log("Slot " + i + " was updated to hold item " + chest.GetItem(i));
                    }
                }
                //  Debug.Log("Updated slots with " + chest.name);
            }
            else // IF GIVEN CHEST IS NULL, DISABLE ALL SLOTS
            {
                for (int i = 0; i < chestSlotsUI.Count; i++)
                {
                    chestSlotsUI[i].gameObject.SetActive(false);
                }
                // Debug.Log("Disabled slots");
            }
        }
    }
    void UpdateChestSlotUIFromList(int index)
    {
        chestSlotsUI[index]?.UpdateSlot(selectedChest?.GetItem(index), selectedChest.GetCount(index));
    }
    public void UpdateChestInventoryFromUI(int index)
    {
        if (index < selectedChest.TotalInventorySlots)
            selectedChest.SetItem(index, chestSlotsUI[index].GetItem, chestSlotsUI[index].GetCount);
    }
    #endregion
}
