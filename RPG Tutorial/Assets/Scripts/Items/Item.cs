using UnityEngine;

//makes rlly easy to create new items/instances of this class:
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")] //when created from asset folder, auto adds item w/ filename 'New Item' from the 'Inventory' tab as the 'Item' option

//blueprint for all our 'scriptable' objs:
public class Item : ScriptableObject
{
    //all items need the properties of: a name, icon
    new public string name = "New Item"; //'new' makes this variable overwrite the old value of 'name' for this game obj

    public Sprite icon = null; // picture representation of item

    public bool isDefaultItem = false; //bc when replace default equipment don't want it cluttering up our inventory? 

    public virtual void Use() //use the item, planned to be overwritten by child classes
    {

        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.inventoryInstance.RemoveItem(this);
    }
}

//add: equipment type, armour modifiers, damage modifiers, etc. to children of this class