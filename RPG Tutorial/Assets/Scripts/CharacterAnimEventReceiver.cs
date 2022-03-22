using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimEventReceiver : MonoBehaviour
{
    private CharacterCombat combat;

    private void Start()
    {
        combat = GetComponentInParent<CharacterCombat>();
    }

    //animation event activated w/ hit occurs:
    public void AttackHitEvent() //automatically called if anims setup with event w/ funct named this:
    {
        //call method on character combat script to pass along 'anim event':
        combat.AttackHit_AnimEvent();
    }
}
