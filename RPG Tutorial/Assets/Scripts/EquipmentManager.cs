using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    //event setup:
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem); //any methods subscribed to the below def'd 'callback' take these args and need to have this name?
    public OnEquipmentChanged onEquipmentChangedCallback; //other methods can subscribe to this

    public Equipment[] currEquipment; //array of equipment slots
    public SkinnedMeshRenderer[] currMeshes; //array of currently equipt meshes
    public Equipment[] defaultEquipment;

    public SkinnedMeshRenderer targetMesh; //mesh of player

    private Inventory inventory; //for caching inventory

    private void Start()
    {
        inventory = Inventory.inventoryInstance; //cache inventory instance

        //System.Enum.GetNames(typeof(EquipmentSlot)); //gets all enums of specified enum type
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; //number of slots = length of 'EquipmentSlot' enum

        //initialize array sizes:
        currEquipment = new Equipment[numSlots];
        currMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();
    }
    
    public void Equip(Equipment newItem) //equip passed in 'newItem', but placement matters
    {
        int slotIndex = (int)newItem.equipSlot; //enum converted to int to find its correct placement slot

        Equipment oldItem = Unequip(slotIndex); //catch for if no item to uneuip in the 'Unequip' method

        //swapping equipment: (now done w/ 'Unequip')
        /*
        if (currEquipment[slotIndex] != null) //if equipment slot not empty
        {
            //add old item back into inventory:
            oldItem = currEquipment[slotIndex];
            inventory.AddItem(oldItem);
        }
        */

        // w/ equipment changes call methods:
        if (onEquipmentChangedCallback != null) //if any methods subscribed to call back
        {
            onEquipmentChangedCallback.Invoke(newItem, oldItem); //run methods subscribed
        }

        //deform body for new item:
        SetEquipmentBlendShapes(newItem, 100);

        //equip:
        currEquipment[slotIndex] = newItem;

        //puts equipt item into game world:
        //tutorial did: SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        SkinnedMeshRenderer newMesh = newItem.mesh; //my replacement

        //tutorial did: newMesh.transform.parent = targetMesh.transform; //newMesh moved to child of player mesh

        //link newly equipt item to player bones:
        //tutorial did: newMesh.bones = targetMesh.bones;
        //tutorial did: newMesh.rootBone = targetMesh.rootBone; //doesn't work correctly?

        //activate specified equipment obj (my replacement):
        /*
        foreach (Transform child in targetMesh.transform.parent)
        {
            //debug: print("child name: " + child.name + " and new mesh name: " + newMesh.name);
            
            if(child.name == newMesh.name)
            {
                child.gameObject.SetActive(true);

                print(child.name + " set active: " + child.gameObject.activeSelf);
            }
        }
        */
        ChangeMeshActive(newMesh, targetMesh.transform.parent);

        currMeshes[slotIndex] = newMesh; //fill mesh array w/ equipt meshes
    }

    public Equipment Unequip(int slotIndex) //unequip item at 'slotIndex'
    {
        if( currEquipment[slotIndex] != null) //if specified equipment slot isn't empty
        {
            if(currMeshes[slotIndex] != null) //if specified slot's mesh isnt empty
            {
                //tutorial did: Destroy(currMeshes[slotIndex].gameObject); //remove mesh from scene

                //deactivate specified equipment obj (my replacement):
                /*
                foreach (Transform child in targetMesh.transform.parent)
                {
                    //debug: print("child name: " + child.name + " and new mesh name: " + currMeshes[slotIndex].name);
                    
                    if (child.name == currMeshes[slotIndex].name)
                    {
                        child.gameObject.SetActive(false);

                        print(child.name + "set active: " + child.gameObject.activeSelf);
                    }
                    
                }
                */
                ChangeMeshActive(currMeshes[slotIndex], targetMesh.transform.parent);

            }

            Equipment oldItem = currEquipment[slotIndex]; //store old item 

            //reset body deformation:
            SetEquipmentBlendShapes(oldItem, 0);

            bool itemAddedtoInventory;

            itemAddedtoInventory = inventory.AddItem(oldItem); //add item back into inventory + store if successful

            if (itemAddedtoInventory) //if item added to inventory (included incase can't unequip item since full inventory)
            {
                currEquipment[slotIndex] = null; //clear item from 'equipment' array (nothing in a class type, then it is null)
            }

            if (onEquipmentChangedCallback != null) //if any methods subscribed to call back
            {
                onEquipmentChangedCallback.Invoke(null, oldItem); //run methods subscribe (no newItem bc unequiping an item)
            }

            return oldItem;
        }

        return null;
    }

    //inverts the active state of the desired mesh found under the passed in 'parent':
    private void ChangeMeshActive(SkinnedMeshRenderer newMesh, Transform parent) //'parent' used for root, then branches
    {
        //debug: print("Currently searching through the parent obj: " + parent.name);

        foreach (Transform child in parent)
        {
            //debug: print("child name: " + child.name + " and new mesh name: " + newMesh.name);

            if (child.name == newMesh.name)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);

                print(child.name + " set active: " + child.gameObject.activeSelf);

                break; //stop looking after mesh found
            }
            ChangeMeshActive(newMesh, child); //uses recursion for each item, so seems pretty unnecessarily intensive 
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currEquipment.Length; i++) //loop thru all equipment slots
        {
          Unequip(i);
        }

        EquipDefaultItems();
    }

    //deform body to move smoothly with armor/clothes:
    private void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions) //loops thru each blendshape region covered by passed in item
        {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight); //set that blendshape weight to desired (usually 100 w/ equiping, and 0 w/ unequiping)
        }
    }

    void EquipDefaultItems()
    {
        foreach (Equipment equipmentPiece in defaultEquipment)
        {
            Equip(equipmentPiece);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }
}
