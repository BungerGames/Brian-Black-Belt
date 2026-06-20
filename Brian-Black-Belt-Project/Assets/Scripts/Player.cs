using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;
    public Camera cam;
    public GameObject InvCanv;
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        Debug.Log("Inventory");

        cam = GetComponent<Camera>();

        


    }
    
    
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
            print("guh");
            uiInventory.RefreshInventoryItems();
        }
    }

    public void OpenInventory()
    {
        InvCanv.SetActive(true);
    }

    public void CloseInventory()
    {
        InvCanv.SetActive(false);
    }

    public void ToggleUI()
    {
        bool currentState = InvCanv.activeSelf;
        InvCanv.SetActive(!currentState);

    }
   
}
