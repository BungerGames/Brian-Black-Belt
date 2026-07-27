using System;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnInventoryChanged;

    private int width;
    private int height;

    private InventorySlot[,] slots;

    public Inventory(int width, int height)
    {
        this.width = width;
        this.height = height;

        slots = new InventorySlot[width, height];

        // Create all slots
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                slots[x, y] = new InventorySlot(x, y);
            }
        }

        // Test items
        AddItem(new Item
        {
            itemType = Item.ItemType.RockTool,
            amount = 1
        });

        AddItem(new Item
        {
            itemType = Item.ItemType.Torch,
            amount = 5
        });

        AddItem(new Item
        {
            itemType = Item.ItemType.Stone,
            amount = 50
        });

        AddItem(new Item
        {
            itemType = Item.ItemType.Stone,
            amount = 75
        });
    }

    public bool AddItem(Item item)
    {
        // First try stacking
        if (item.IsStackable())
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    InventorySlot slot = slots[x, y];

                    if (slot.item != null &&
                        slot.item.itemType == item.itemType)
                    {
                        int maxStack = slot.item.GetMaxStackSize();

                        int space = maxStack - slot.item.amount;

                        if (space > 0)
                        {
                            int amountToAdd = Mathf.Min(
                                space,
                                item.amount
                            );

                            slot.item.amount += amountToAdd;
                            item.amount -= amountToAdd;

                            if (item.amount <= 0)
                            {
                                OnInventoryChanged?.Invoke(
                                    this,
                                    EventArgs.Empty
                                );

                                return true;
                            }
                        }
                    }
                }
            }
        }

        // Find empty slots
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (slots[x, y].IsEmpty())
                {
                    slots[x, y].item = item;

                    OnInventoryChanged?.Invoke(
                        this,
                        EventArgs.Empty
                    );

                    return true;
                }
            }
        }

        // Inventory is full
        return false;
    }

    public void MoveItem(
        int fromX,
        int fromY,
        int toX,
        int toY)
    {
        if (!IsInsideGrid(fromX, fromY))
            return;

        if (!IsInsideGrid(toX, toY))
            return;

        InventorySlot fromSlot = slots[fromX, fromY];
        InventorySlot toSlot = slots[toX, toY];

        if (fromSlot.item == null)
            return;

        // If target is empty, simply move
        if (toSlot.item == null)
        {
            toSlot.item = fromSlot.item;
            fromSlot.item = null;
        }

        // If same item type and stackable, combine
        else if (
            toSlot.item.itemType ==
            fromSlot.item.itemType &&
            toSlot.item.IsStackable())
        {
            int maxStack =
                toSlot.item.GetMaxStackSize();

            int space =
                maxStack - toSlot.item.amount;

            int amountToMove =
                Mathf.Min(
                    space,
                    fromSlot.item.amount
                );

            toSlot.item.amount += amountToMove;

            fromSlot.item.amount -= amountToMove;

            if (fromSlot.item.amount <= 0)
            {
                fromSlot.item = null;
            }
        }

        // Otherwise swap
        else
        {
            Item temp = toSlot.item;

            toSlot.item = fromSlot.item;

            fromSlot.item = temp;
        }

        OnInventoryChanged?.Invoke(
            this,
            EventArgs.Empty
        );
    }

    public void RemoveItem(int x, int y)
    {
        if (!IsInsideGrid(x, y))
            return;

        slots[x, y].item = null;

        OnInventoryChanged?.Invoke(
            this,
            EventArgs.Empty
        );
    }

    public InventorySlot GetSlot(int x, int y)
    {
        if (!IsInsideGrid(x, y))
            return null;

        return slots[x, y];
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    private bool IsInsideGrid(int x, int y)
    {
        return
            x >= 0 &&
            x < width &&
            y >= 0 &&
            y < height;
    }
}