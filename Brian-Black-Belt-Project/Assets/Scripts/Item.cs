using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item{
    public enum ItemType
    {
        Stone,
        Metal,
        Wood,
        RockTool,
        Torch,
    }

    public ItemType itemType;
    public int amount;


    public Sprite GetSprite()
    {
        switch (itemType){
            default:

            case ItemType.Stone:      return ItemAssets.Instance.StoneSprite;
            case ItemType.Metal:      return ItemAssets.Instance.MetalSprite;
            case ItemType.Wood:       return ItemAssets.Instance.WoodSprite;
            case ItemType.RockTool:   return ItemAssets.Instance.RockToolSprite;
            case ItemType.Torch:      return ItemAssets.Instance.TorchSprite;
                    
        }
    }
}
