using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create a 'CharacterStats' comp if one not already attached to obj:
[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    const float combatCooldown = 5f;
    private float lastAttackTime; //world time when last attack happened

    public float attackSpeed = 1f;
    public float attackDelay = 0.6f; //delay the target taking damage
    public bool inCombat { get; private set; }

    //quick, easy way to create a 'delegate' w/ a return type of void and no args: (called 'Event method')
    public event System.Action OnAttackCallback;
    
    private float attackCooldown = 0f;
    private CharacterStats myStats;
    private CharacterStats opponentStats;

    private void Start()
    {
        myStats = GetComponent<CharacterStats>();

    }

    private void Update()
    {
        //incrementally decrease cooldown by 1 every second:
        attackCooldown -= Time.deltaTime;

        //not in combat when it's been longer than the cooldown since an attack:
        if(Time.time - lastAttackTime > combatCooldown) //'Time.time' = time since game started
        {
            inCombat = false;
        }
    }

    public void Attack(CharacterStats targetsStats)
    {
        //if cooldown is up, take damage + reset it:
        if(attackCooldown <= 0f)
        {
            //StartCoroutine(DoDamage(targetsStats, attackDelay));

            //update opponent stats every attack:
            opponentStats = targetsStats;

            //signal attack started:
            if(OnAttackCallback != null) //if any methods subscribed to this event
            {
                OnAttackCallback(); //dont have to 'Invoke()'
            }

            attackCooldown = 1 / attackSpeed; // cooldown and speed r inversely proportional (higher cooldown means lower attack speed, etc.)

            inCombat = true;
            lastAttackTime = Time.time;
        }

    }

    //delay damage to allow attack anim to play out partially:
    /*
    private IEnumerator DoDamage(CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);

        
        stats.TakeDamage(myStats.atk.GetValue());

        //not in combat if target dies:
        if(stats.currHealth <= 0)
        {
            inCombat = false;
        }
        
    }
    */

    //hit anim event passed on from 'CharacterAnimEventReceiver' script to deal damage:
    public void AttackHit_AnimEvent()
    {
        opponentStats.TakeDamage(myStats.atk.GetValue());

        //not in combat if target dies:
        if (opponentStats.currHealth <= 0)
        {
            inCombat = false;
        }
    }
}
