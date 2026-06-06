using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;

    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        Debug.Log("Inventory");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
