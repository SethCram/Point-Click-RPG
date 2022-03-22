using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //to access 'Nav Mesh Agent'

public class EnemyController : MonoBehaviour
{
    //attack player radius and its calculation:
    public float baseAttackRadius = 10f;
    private float currAttackRadius; //biggest this will get is base atk radius size
    private float maxStealth = 20f;
    private int currPlayerStealth;
    private float atkRadiusPercent;

    //target to chase + atk:
    private Transform target;

    //to move enemy:
    private NavMeshAgent agent;

    private CharacterCombat myCombat; // need to call 'Attack()' for enemy

    // Start is called before the first frame update
    void Start()
    {
        //set target to player's transform:
        target = PlayerManager.instance.player.transform;

        //init vars:
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 1.5f; //perfect distance for hand to hand combat (should change based on enemy weapon/reach)

        myCombat = GetComponent<CharacterCombat>(); //dont needa requirecomp for this bc done in 'CharacterCombat'

        //set curr atk radius according to player's base stealth:
        ChangeCurrAttackRadius();

        //subscribe method to callback:
        EquipmentManager.instance.onEquipmentChangedCallback += OnEquipmentChanged;

    }

    // Update is called once per frame
    void Update()
    {
        //find dist tween player + enemy to see if need to chase:
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= currAttackRadius)
        {
            agent.SetDestination(target.position); //even if player moving, this constantly updating to account for that

            if(distance <= agent.stoppingDistance)
            {

                //attack target:
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                if(targetStats != null)
                {
                    myCombat.Attack(targetStats);
                }

                //face the target:
                FaceTarget();
            }
        }
    }

    //update curr attack radius everytime equip changes:
    public void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if( newItem != null)
        {
            if(newItem.stealthModifier != 0)
            {
                Invoke("ChangeCurrAttackRadius", 0.1f); //wait for 0.1 secs to let mods be applied
            }
        }
        
        if(oldItem != null)
        {
            if(oldItem.stealthModifier != 0)
            {
                Invoke("ChangeCurrAttackRadius", 0.1f);
            }
        }
    }

    //set curr atk radius using player stealth stat:
    private void ChangeCurrAttackRadius()
    {
        currPlayerStealth = target.GetComponent<CharacterStats>().stealth.GetValue();
        atkRadiusPercent = 1 - (currPlayerStealth / maxStealth);
        if (atkRadiusPercent <= 0)
        {
            //smallest atk radius:
            currAttackRadius = baseAttackRadius * 0.1f;
        }
        else
        {
            currAttackRadius = atkRadiusPercent * baseAttackRadius;
        }
    }

    private void FaceTarget()
    {
        //dir towards target:
        Vector3 direction = (target.position - transform.position).normalized; //difference tween target and agent position (normalize bc only care ab dir not length)

        //rotation to look in target's dir:
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)); //don't want player to look in y direction (Quaternion's used for rot)

        //smoothly interpolate towards new rot:
        float rotationSpeed = 5f;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); //smoothly change agent's rot from it's curr rot to 'lookRotation' at 'rotationSpeed'
    }

    //draws red gizmo for attack zone w/ attached obj selected (like for item pickups):
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currAttackRadius);
    }
}
