using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [Header("Inventory Size")]
    [SerializeField] private int inventoryWidth = 5;
    [SerializeField] private int inventoryHeight = 4;

    [Header("UI")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;

    private Inventory inventory;

    private Dictionary<Vector2Int, UI_InventorySlot> slotUI =
        new Dictionary<Vector2Int, UI_InventorySlot>();


    private void Start()
    {
        // We don't create the Inventory here.
        // Player.cs creates it and gives it to us.
    }


    public void SetInventory(Inventory inventory)
    {
        // If we already had an inventory,
        // stop listening to its events.
        if (this.inventory != null)
        {
            this.inventory.OnInventoryChanged -=
                Inventory_OnInventoryChanged;
        }

        // Store the new inventory
        this.inventory = inventory;

        // Listen for inventory changes
        this.inventory.OnInventoryChanged +=
            Inventory_OnInventoryChanged;

        // Create the visual slots
        CreateInventoryUI();

        // Display the items
        RefreshInventoryUI();
    }


    private void CreateInventoryUI()
    {
        if (slotPrefab == null)
        {
            Debug.LogError("slotPrefab is not assigned in the Inspector!");
            return;
        }

        if (slotContainer == null)
        {
            Debug.LogError("slotContainer is not assigned in the Inspector!");
            return;
        }
        // Delete any old slots
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        slotUI.Clear();


        // Create all inventory slots
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                GameObject slotObject =
                    Instantiate(
                        slotPrefab,
                        slotContainer
                    );

                UI_InventorySlot slot =
                    slotObject.GetComponent<UI_InventorySlot>();

                // Tell the slot which inventory
                // and coordinates it belongs to
                slot.Initialize(
                    this,
                    x,
                    y
                );

                // Store reference to the UI slot
                slotUI.Add(
                    new Vector2Int(x, y),
                    slot
                );
            }
        }
    }


    private void Inventory_OnInventoryChanged(
        object sender,
        EventArgs e)
    {
        RefreshInventoryUI();
    }


    public void RefreshInventoryUI()
    {
        // Update every visual slot
        foreach (
            KeyValuePair<Vector2Int, UI_InventorySlot> pair
            in slotUI
        )
        {
            int x = pair.Key.x;
            int y = pair.Key.y;

            // Get the actual inventory slot
            InventorySlot inventorySlot =
                inventory.GetSlot(x, y);

            // Update the UI
            pair.Value.Refresh(
                inventorySlot
            );
        }
    }


    // This is kept so your old Player.cs
    // won't have errors if you call it.
    public void RefreshInventoryItems()
    {
        RefreshInventoryUI();
    }


    public void MoveItem(
        int fromX,
        int fromY,
        int toX,
        int toY)
    {
        inventory.MoveItem(
            fromX,
            fromY,
            toX,
            toY
        );
    }


    public InventorySlot GetSlot(
        int x,
        int y)
    {
        return inventory.GetSlot(x, y);
    }


    public Inventory GetInventory()
    {
        return inventory;
    }
}