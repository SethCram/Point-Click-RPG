using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    public override void Die()
    {
        //says who died in console:
        base.Die();

        //Kill the player (death anim done in 'CharacterAnimator'; could add gameover screen after player killed, then prompt to respawn w/ penalty) (we just restart lvl for now):

        PlayerManager.instance.Invoke("ResetScene", deathAnimDelay); //delay scene reset by _ secs so player death anim can play out
    }

    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChangedCallback += OnEquipmentChanged;
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem) //any methods subscribed to the callback have to take these args and have this name (so only 1 method can subscribe to the callback per class?)
    {
        //add new mods:
        if (newItem != null) //bc 'Unequip' method invokes the callback with a 'null' newItem, cant add mods to list w/ no new item
        {
            def.AddModifier(newItem.armorModifier);
            atk.AddModifier(newItem.damageModifier);
            stealth.AddModifier(newItem.stealthModifier);
            bulk.AddModifier(newItem.bulkModifier);
        }

        //remove old mods:
        if(oldItem != null)
        {
            def.RemoveModifier(oldItem.armorModifier);
            atk.RemoveModifier(oldItem.damageModifier);
            stealth.RemoveModifier(oldItem.stealthModifier);
            bulk.RemoveModifier(oldItem.bulkModifier);
        }
    }
}
