using UnityEngine;
using UnityEngine.UI; //needed for access to image comp on this slot

public class InventorySlot : MonoBehaviour
{
    public Image icon; //to update our icon w/ add or remove item (must drag icon onto this in unity)
    public Button removeButton; //to show/hide remove button on each slot

    private Item item; //keeps track of curr item in slot

    public void AddItem(Item newItem)
    {
        //set item:
        item = newItem;

        //update our icon:
        icon.enabled = true; //show inventory icon
        icon.sprite = item.icon; //set item icon to inventory icon

        //show remove button:
        removeButton.interactable = true;
    }

    public void clearSlot()
    {
        //clear item:
        item = null;

        //clear icon:
        icon.sprite = null;
        icon.enabled = false;

        //clear remove button:
        removeButton.interactable = false;
    }

    public void OnRemoveButton() // w/ 'removeButton' clicked
    {
        Inventory.inventoryInstance.RemoveItem(item);
    }

    public void UseItem()
    {
        if(item != null) //if have an item
        {
            item.Use();
        }
    }
}
