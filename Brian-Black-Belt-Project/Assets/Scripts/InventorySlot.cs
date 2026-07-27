using System;

[Serializable]
public class InventorySlot
{
    public Item item;

    public int x;
    public int y;

    public InventorySlot(int x, int y)
    {
        this.x = x;
        this.y = y;
        item = null;
    }

    public bool IsEmpty()
    {
        return item == null;
    }
}