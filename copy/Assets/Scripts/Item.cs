using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Stone,
        Metal,
        Wood,
        RockTool,
        Torch
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Stone:
                return ItemAssets.Instance.StoneSprite;

            case ItemType.Metal:
                return ItemAssets.Instance.MetalSprite;

            case ItemType.Wood:
                return ItemAssets.Instance.WoodSprite;

            case ItemType.RockTool:
                return ItemAssets.Instance.RockToolSprite;

            case ItemType.Torch:
                return ItemAssets.Instance.TorchSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            case ItemType.Stone:
            case ItemType.Metal:
            case ItemType.Wood:
            case ItemType.Torch:
                return true;

            case ItemType.RockTool:
                return false;

            default:
                return false;
        }
    }

    public int GetMaxStackSize()
    {
        switch (itemType)
        {
            case ItemType.Stone:
            case ItemType.Metal:
            case ItemType.Wood:
                return 1000;

            case ItemType.Torch:
                return 1;

            case ItemType.RockTool:
                return 1;

            default:
                return 1;
        }
    }
}