using System;
using UnityEngine;

public class ItemPickup : Interactable //this class is now derived from/a child of the 'Interactable' class (but since not a child of monobehavior, cant access any unity functs? (incorrect bc the class we derive from derives from monobehavior) )
{
    public Item item;

    public override void Interact()
    {
        base.Interact(); //calls 'Interactable' Interact() method

        Pickup();
    }

    //add item to inventory:
    private void Pickup()
    {
        Debug.Log("Picking up " + item.name);

        // Add to inventory:
        //FindObjectOfType<Inventory>().AddItem(item); //could do but pretty performance demanding, so we use a singleton
        bool wasPickedUp = Inventory.inventoryInstance.AddItem(item); //add item to list w/ pickup

        if (wasPickedUp)
        {
            Destroy(gameObject); //comment out to test full inventory behavior
        }
    }
}
