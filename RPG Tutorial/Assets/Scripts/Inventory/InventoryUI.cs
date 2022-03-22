using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent; //stores parent transform of all inventory slots
    public GameObject inventoryUI; //gameobj of displayed inventory panel

    private Inventory playerInventory;
    private InventorySlot[] slots; //to store refs to slots

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = Inventory.inventoryInstance; //cache the inventory instance

        playerInventory.onItemChangedCallback += UpdateUI; //subscribe 'UpdateUI' method to the event 'onItemChangedCallback'

        slots = itemsParent.GetComponentsInChildren<InventorySlot>(); //fill 'slots' array w/ all inventory slots (would have to do this in the 'update' method if inventory slots are changing)
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!(inventoryUI.activeSelf)); //shows inventory if hiding, hides inventory if showing (sets it to it's inverse)
        }
    }

    private void UpdateUI()
    {
        Debug.Log("Updating UI");

        for (int i = 0; i < slots.Length; i++) //loop thru all inventory slots
        {
            //fill slots w/ items:
            if (i < playerInventory.itemsList.Count) //checking bc the item list is the "actual" number of items in the inventory
            {
                slots[i].AddItem(playerInventory.itemsList[i]);
            }
            else //fill slots w/ empties
            {
                slots[i].clearSlot();
            }
        }

        //code for updating all items in inventory
    }
}
