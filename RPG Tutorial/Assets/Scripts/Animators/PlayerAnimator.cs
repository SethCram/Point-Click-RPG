using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    public WeaponAnims[] weaponAnims; //'WeaponAnims' type def'd below (drag+dropped into)
    Dictionary<Equipment, AnimationClip[]> weaponAnimsDict; //index is 'Equipment' type and it's associated type is 'AnimationClip[]'

    private EquipmentManager equipmentManager; //caching equip manager
    private bool SwordAndShieldEquipt = false;
    //maybe: private bool OnlyWeaponEquipt = false;

    //link weapon to a set of attack animations:
    [System.Serializable] //so below method's local vars visible in inspector
    public struct WeaponAnims
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }

    protected override void Start() //syntax to override a protected method
    {
        base.Start();

        equipmentManager = EquipmentManager.instance;

        equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged; //subscribe method

        //initialize and populate attack anim dictionary:
        weaponAnimsDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach (WeaponAnims atkAnim in weaponAnims)
        {
            weaponAnimsDict.Add(atkAnim.weapon, atkAnim.clips);
        }
    }

    //Change active animator layer based on what equip changes:
    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem) //framework for subscribable method
    {
        //check if new weapon equipt (for w/ equipping):
        if(newItem != null && newItem.equipSlot == EquipmentSlot.Weapon )
        {
            //if new weapon has it's own set of atk anims, assign it to the curr atk anim set:
            if(weaponAnimsDict.ContainsKey(newItem)) 
            {
                currAtkAnimSet = weaponAnimsDict[newItem];
            }

            //check if offhand item equipt:
            foreach (Equipment equipPiece in equipmentManager.currEquipment)
            {
                if (equipPiece != null) //needed since an array is prefilled with empty slots
                {
                    if (equipPiece.equipSlot == EquipmentSlot.OffHand)
                    {
                        //change active animator layer:
                        //ChangeToSwordAndShieldLayer();
                    }
                }
            }
        } //check if new item not a weapon, but old one was (for w/ unequipping):
        else if(newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Weapon)
        {
            //reset attack animations:
            currAtkAnimSet = defaultAtkAnimSet;

            if (SwordAndShieldEquipt)
            {
                //change active animator layer:
                //ChangeToBaseLayer();
            }
        }

        //check if new offhand equipt:
        if (newItem != null && newItem.equipSlot == EquipmentSlot.OffHand)
        {
            //check if weapon item equipt:
            foreach (Equipment equipPiece in equipmentManager.currEquipment)
            {
                if (equipPiece != null) //needed since an array is prefilled with empty slots
                {
                    if (equipPiece.equipSlot == EquipmentSlot.Weapon)
                    {
                        //change active animator layer:
                        //ChangeToSwordAndShieldLayer();
                    }
                }
            }
        } //check if new item not an offhand, but old one was (for w/ unequipping):
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.OffHand && SwordAndShieldEquipt)
        {
            //change active animator layer:
            //ChangeToBaseLayer();
        }

    }

    private void ChangeToBaseLayer()
    {
        animator.SetLayerWeight(1, 0); //deactivate sword and shield layer
        animator.SetLayerWeight(0, 1); //activate base layer

        SwordAndShieldEquipt = false;
    }

    private void ChangeToSwordAndShieldLayer()
    {
        animator.SetLayerWeight(1, 1); //activate sword and shield layer
        animator.SetLayerWeight(0, 0); //deactivate base layer

        SwordAndShieldEquipt = true;
    }
}
