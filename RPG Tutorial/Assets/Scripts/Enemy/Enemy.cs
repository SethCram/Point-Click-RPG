using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{
    private PlayerManager playerManager; //to access player comps to get them to 'Attack()' and be attacked
    private CharacterStats myStats; //this enemy obj's stats to pass into player's 'Attack()'

    private void Start()
    {
        playerManager = PlayerManager.instance;
        

        myStats = GetComponent<CharacterStats>();
    }

    public override void Interact()
    {
        //say what we 'interacting' w/:
        //base.Interact();

        //player attack this enemy: 
        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        if(playerCombat != null)
        {
            playerCombat.Attack(myStats);
        }

        

    }
}
