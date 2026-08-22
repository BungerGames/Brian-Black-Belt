using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [Header("Inventory Size")]
    [SerializeField] private int inventoryWidth = 5;
    [SerializeField] private int inventoryHeight = 5; // includes hotbar row

    [Header("UI")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;       // storage grid only
    [SerializeField] private Transform hotbarRowContainer;   // hotbar row, inside inventory panel

    [Header("Hotbar Selection")]
    [SerializeField] private int hotbarWidth = 5;

    private int selectedHotbarX = 0;
    private UI_InventorySlot[] hotbarSlotsCache;

    private Inventory inventory;
    private Dictionary<Vector2Int, UI_InventorySlot> slotUI = new Dictionary<Vector2Int, UI_InventorySlot>();

    private int HotbarRow => inventoryHeight - 1;

    public void SetInventory(Inventory inventory)
    {
        if (this.inventory != null)
            this.inventory.OnInventoryChanged -= Inventory_OnInventoryChanged;

        this.inventory = inventory;
        this.inventory.OnInventoryChanged += Inventory_OnInventoryChanged;

        CreateInventoryUI();
        RefreshInventoryUI();
    }
    public void RemoveSelectedHotbarItem()
    {
        inventory.RemoveItem(selectedHotbarX, HotbarRow);
    }
    private void CreateInventoryUI()
    {
        if (slotPrefab == null || slotContainer == null || hotbarRowContainer == null)
        {
            Debug.LogError("UI_Inventory: a required field is not assigned in the Inspector!");
            return;
        }

        foreach (Transform child in slotContainer) Destroy(child.gameObject);
        foreach (Transform child in hotbarRowContainer) Destroy(child.gameObject);
        slotUI.Clear();

        for (int y = 0; y < HotbarRow; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                GameObject slotObject = Instantiate(slotPrefab, slotContainer);
                UI_InventorySlot slot = slotObject.GetComponent<UI_InventorySlot>();
                slot.Initialize(this, x, y);
                slotUI.Add(new Vector2Int(x, y), slot);
            }
        }

        for (int x = 0; x < inventoryWidth; x++)
        {
            GameObject slotObject = Instantiate(slotPrefab, hotbarRowContainer);
            UI_InventorySlot slot = slotObject.GetComponent<UI_InventorySlot>();
            slot.Initialize(this, x, HotbarRow);
            slotUI.Add(new Vector2Int(x, HotbarRow), slot);
        }
        // Cache hotbar slots in x order, then re-apply current selection
        hotbarSlotsCache = new UI_InventorySlot[hotbarWidth];
        for (int x = 0; x < hotbarWidth; x++)
            hotbarSlotsCache[x] = slotUI[new Vector2Int(x, HotbarRow)];

        SelectHotbarSlot(selectedHotbarX);
    }

    private void Inventory_OnInventoryChanged(object sender, EventArgs e) => RefreshInventoryUI();

    public void RefreshInventoryUI()
    {
        foreach (var pair in slotUI)
            pair.Value.Refresh(inventory.GetSlot(pair.Key.x, pair.Key.y));
    }


    public void SelectHotbarSlot(int x)
    {
        Debug.Log($"SelectHotbarSlot called with x={x}, cache null? {hotbarSlotsCache == null}");
        if (hotbarSlotsCache == null || x < 0 || x >= hotbarWidth) return;

        if (hotbarSlotsCache[selectedHotbarX] != null)
            hotbarSlotsCache[selectedHotbarX].SetSelected(false);

        selectedHotbarX = x;
        hotbarSlotsCache[selectedHotbarX].SetSelected(true);
    }

    public int GetSelectedHotbarX() => selectedHotbarX;

    public InventorySlot GetSelectedHotbarSlot() => inventory.GetSlot(selectedHotbarX, HotbarRow);
    public void RefreshInventoryItems() => RefreshInventoryUI();

    public void MoveItem(int fromX, int fromY, int toX, int toY)
    {

        Debug.Log($"MoveItem called: from ({fromX},{fromY}) to ({toX},{toY})");
        inventory.MoveItem(fromX, fromY, toX, toY);

    }
    public InventorySlot GetSlot(int x, int y) => inventory.GetSlot(x, y);
    public Inventory GetInventory() => inventory;
}