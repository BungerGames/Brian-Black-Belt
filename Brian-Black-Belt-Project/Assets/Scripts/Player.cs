using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;

    public GameObject cam;
    public GameObject InvCanv;
    public Inventory GetInventory() => inventory;
    private Inventory inventory;
    [Header("Dropping")]
    [SerializeField] private Transform dropPoint; // empty object slightly in front of camera

    
    private void Start()
    {
        inventory = new Inventory(5, 5);

        uiInventory.SetInventory(inventory);

        Debug.Log("Inventory");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            DropSelectedHotbarItem();


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }

        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Debug.Log("Pressed hotbar key: " + i);
                uiInventory.SelectHotbarSlot(i);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            uiInventory.SelectHotbarSlot((uiInventory.GetSelectedHotbarX() - 1 + 5) % 5);
        else if (scroll < 0f)
            uiInventory.SelectHotbarSlot((uiInventory.GetSelectedHotbarX() + 1) % 5);

    }

    public void OpenInventory()
    {
        InvCanv.SetActive(true);
    }

    public void CloseInventory()
    {
        InvCanv.SetActive(false);
    }
    public void DropSelectedHotbarItem()
    {
        InventorySlot slot = uiInventory.GetSelectedHotbarSlot();
        if (slot == null || slot.item == null) return;

        Item droppedItem = new Item { itemType = slot.item.itemType, amount = slot.item.amount };

        ItemWorld.SpawnItemWorld(dropPoint.position, droppedItem);

        uiInventory.RemoveSelectedHotbarItem();
    }
    public void ToggleUI()
    {
        bool currentState = InvCanv.activeSelf;

        InvCanv.SetActive(!currentState);

        Debug.Log("Toggled. New state: " + InvCanv.activeSelf + " | Cursor lock: " + (!currentState ? "None" : "Locked"));

        if (!currentState)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}