using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public Sprite StoneSprite;
    public Sprite MetalSprite;
    public Sprite WoodSprite;
    public Sprite RockToolSprite;
    public Sprite TorchSprite;
}
