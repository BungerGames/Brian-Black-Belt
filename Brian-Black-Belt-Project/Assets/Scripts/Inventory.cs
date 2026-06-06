using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.RockTool, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Torch, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Stone, amount = 1 });
    }


    public void AddItem(Item item)
    {
        itemList.Add(item);
    }
    
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
