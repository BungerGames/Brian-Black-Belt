using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private UI_Inventory uiInventory;

    [Header("Hover/Click Feel")]
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float scaleSpeed = 10f;

    private int gridX;
    private int gridY;

    private Image itemImage;
    private TextMeshProUGUI amountText;

    private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private RectTransform itemDisplayRect;

    private Vector2 originalPosition;

    private bool isDragging;

    private Coroutine scaleRoutine;

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

        itemDisplayRect =
            transform
                .Find("ItemDisplay")
                .GetComponent<RectTransform>();

        itemDisplayRect.SetAsLastSibling();

        itemImage =
            itemDisplayRect
                .Find("image")
                .GetComponent<Image>();

        amountText =
            itemDisplayRect
                .Find("text")
                .GetComponent<TextMeshProUGUI>();

        canvasGroup =
            itemDisplayRect.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup =
                itemDisplayRect.gameObject.AddComponent<CanvasGroup>();
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
            itemDisplayRect.anchoredPosition;

        

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(
        PointerEventData eventData)
    {
        if (!isDragging)
            return;

        // Follow mouse
        itemDisplayRect.position =
            eventData.position;
    }

    public void OnEndDrag(
        PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        itemDisplayRect.anchoredPosition = originalPosition;

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
            target.GetComponent<UI_InventorySlot>();

        if (slot != null)
            return slot;

        slot =
            target.GetComponentInParent<UI_InventorySlot>();

        return slot;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Pulse());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
            AnimateScale(1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Pulse());
    }

    private void AnimateScale(float targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleTo(targetScale));
    }

    private IEnumerator Pulse()
    {
        AnimateScale(pulseScale);
        yield return new WaitForSeconds(0.08f);
        AnimateScale(1f);
    }

    private IEnumerator ScaleTo(float targetScale)
    {
        Vector3 target = Vector3.one * targetScale;

        while (Vector3.Distance(rectTransform.localScale, target) > 0.001f)
        {
            rectTransform.localScale = Vector3.Lerp(
                rectTransform.localScale,
                target,
                Time.deltaTime * scaleSpeed
            );
            yield return null;
        }

        rectTransform.localScale = target;
    }
}