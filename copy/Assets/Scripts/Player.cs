using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;

    public GameObject cam;
    public GameObject InvCanv;

    private Inventory inventory;

    private void Start()
    {
        inventory = new Inventory(5, 4);

        uiInventory.SetInventory(inventory);

        Debug.Log("Inventory");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
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