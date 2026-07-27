using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private UI_Inventory uiInventory;

    private int gridX;
    private int gridY;

    private Image itemImage;
    private TextMeshProUGUI amountText;

    private CanvasGroup canvasGroup;

    private RectTransform rectTransform;

    private Vector2 originalPosition;

    private bool isDragging;

    public void Initialize(
        UI_Inventory uiInventory,
        int x,
        int y)
    {
        this.uiInventory = uiInventory;

        gridX = x;
        gridY = y;

        rectTransform =
            GetComponent<RectTransform>();

        itemImage =
            transform
                .Find("image")
                .GetComponent<Image>();

        amountText =
            transform
                .Find("text")
                .GetComponent<TextMeshProUGUI>();

        canvasGroup =
            GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup =
                gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Refresh(
        InventorySlot inventorySlot)
    {
        if (inventorySlot == null ||
            inventorySlot.item == null)
        {
            itemImage.enabled = false;

            amountText.text = "";

            return;
        }

        itemImage.enabled = true;

        itemImage.sprite =
            inventorySlot.item.GetSprite();

        if (inventorySlot.item.amount > 1)
        {
            amountText.text =
                inventorySlot.item.amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }

    public void OnBeginDrag(
        PointerEventData eventData)
    {
        InventorySlot slot =
            uiInventory.GetSlot(
                gridX,
                gridY
            );

        // Can't drag empty slot
        if (slot == null ||
            slot.item == null)
        {
            isDragging = false;
            return;
        }

        isDragging = true;

        originalPosition =
            rectTransform.anchoredPosition;

        canvasGroup.alpha = 0.6f;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(
        PointerEventData eventData)
    {
        if (!isDragging)
            return;

        // Follow mouse
        rectTransform.position =
            eventData.position;
    }

    public void OnEndDrag(
        PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        canvasGroup.alpha = 1f;

        canvasGroup.blocksRaycasts = true;

        UI_InventorySlot targetSlot =
            GetTargetSlot(eventData);

        if (targetSlot != null)
        {
            uiInventory.MoveItem(
                gridX,
                gridY,
                targetSlot.gridX,
                targetSlot.gridY
            );
        }

        // UI will refresh automatically
        // because Inventory.MoveItem()
        // invokes OnInventoryChanged
    }

    private UI_InventorySlot GetTargetSlot(
        PointerEventData eventData)
    {
        GameObject target =
            eventData.pointerCurrentRaycast
                .gameObject;

        if (target == null)
            return null;

        UI_InventorySlot slot =
            target.GetComponent<
                UI_InventorySlot
            >();

        if (slot != null)
            return slot;

        slot =
            target.GetComponentInParent<
                UI_InventorySlot
            >();

        return slot;
    }
}