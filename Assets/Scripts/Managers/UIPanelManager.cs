using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField] public RectTransform inventoryWindow, craftingWindow, chestWindow;
    [SerializeField] private Vector2 inventoryWindowOn, craftingWindowOn, chestWindowOn;
    [HideInInspector] private Vector2 inventoryWindowOff, craftingWindowOff, chestWindowOff;
    [SerializeField] private float tweenTime = 0.3f;

    private void Awake()
    {
        inventoryWindowOff = inventoryWindow.localPosition;
        craftingWindowOff = craftingWindow.localPosition;
        chestWindowOff = chestWindow.localPosition;
        DOTween.defaultRecyclable = true;
    }

    public void TweenInCraftingWindow() => craftingWindow.DOAnchorPos(craftingWindowOn, tweenTime);
    public void TweenOutCraftingWindow() => craftingWindow.DOAnchorPos(craftingWindowOff, tweenTime);

    public void TweenInInventoryWindow() => inventoryWindow.DOAnchorPos(inventoryWindowOn, tweenTime);
    public void TweenOutInventoryWindow() => inventoryWindow.DOAnchorPos(inventoryWindowOff, tweenTime);

    public void TweenInChestWindow() => chestWindow.DOAnchorPos(chestWindowOn, tweenTime);
    public void TweenOutChestWindow() => chestWindow.DOAnchorPos(chestWindowOff, tweenTime);

    private bool Autofill => inventoryWindow && craftingWindow && chestWindow;
    [EnableIf("@Autofill"), Button("Auto Fill Position")]
    void AutoFill()
    {
        inventoryWindowOn = inventoryWindow.localPosition;
        craftingWindowOn = craftingWindow.localPosition;
        chestWindowOn = chestWindow.localPosition;
    }
}
