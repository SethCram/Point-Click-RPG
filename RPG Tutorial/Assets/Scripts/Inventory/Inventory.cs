using System.Collections;
using System.Collections.Generic; //needed for 'list' type
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region InventoryInstanceSingleton
    //singleton pattern: (so 'ItemPickup' script can add an item to our list)
    public static Inventory inventoryInstance; //static vars can be accessed by any other class and are dynamically updated

    private void Awake()
    {
        //should only ever have one 'Inventory':
        if (inventoryInstance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");

            return; //so dont set instance
        }

        inventoryInstance = this;
    }
    #endregion


    public delegate void OnItemChanged(); //def delegate 'type'
    public OnItemChanged onItemChangedCallback; //implement delegate w/ a var (triggered everytime anything changes in our inventory) (now referred to as an 'event'?)

    public List<Item> itemsList = new List<Item>(); //creates new list of items that'll hold all items in our inventory

    public int maxInventorySlots = 20;

    //add item to list:
    public bool AddItem(Item item)
    {
        //only add to list of items that reps inventory if not a default item:
        if (!(item.isDefaultItem))
        {
            if (itemsList.Count >= maxInventorySlots)
            {
                Debug.Log("Not enough room.");

                return false;
            }

            itemsList.Add(item);

            if (onItemChangedCallback != null) //if has any methods subscribed to it
            {
                onItemChangedCallback.Invoke(); //executes all methods subscribed to this callback by invoking it
            }
        }
        return true; //also return true when item is a default item and we don't add it to our inventory?
    }

    //remove item from list:
    public void RemoveItem(Item item)
    {
        itemsList.Remove(item);

        if (onItemChangedCallback != null) //if has any methods subscribed to it
        {
            onItemChangedCallback.Invoke(); //executes all methods subscribed to this callback by invoking it
        }
    }
}
