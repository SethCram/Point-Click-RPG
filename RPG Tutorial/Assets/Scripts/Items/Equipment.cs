using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//can create new 'equipment' from same area creating new items:
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")] 
public class Equipment : Item
{
    //public int equipSlot; //could be used to identify which equipment slot this piece is used in (head or chest, etc.), but not intuitive
    public EquipmentSlot equipSlot; //what slot this piece meant for

    public SkinnedMeshRenderer mesh; //each item's 3d obj
    public EquipmentMeshRegion[] coveredMeshRegions;

    //equipment modifiers: (use these w/ create player stats)
    public int armorModifier;
    public int damageModifier;
    public int stealthModifier;
    public int bulkModifier;

    public override void Use()
    {
        base.Use(); //placeholder, just call og funct

        //remove it from the inventory: (remove before equip)
        RemoveFromInventory(); // make sure to remove item from inventory before equip it so incase need slot to swap items(don't need to ref parent class bc this class derived from it)

        //equip the item:
        EquipmentManager.instance.Equip(this); //feed 'this' bc method needs arg of type 'Equipment'
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, OffHand, Feet} //defing our own variable type, can be used in any class (each possible one has own index val)

public enum EquipmentMeshRegion { Legs, Arms, Torso }; //corresponds to body blendshapes