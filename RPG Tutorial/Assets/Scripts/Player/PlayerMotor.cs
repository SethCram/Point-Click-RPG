using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //needed to access nav mesh + agent

//adds a 'NavMeshAgent' to whatever obj this script is attached to if there isn't one:
[RequireComponent(typeof(NavMeshAgent))]
//Moves agent to passed in ponit:
public class PlayerMotor : MonoBehaviour
{
    private NavMeshAgent agent; //agent that's traversing nav mesh
    private Transform target; //target to follow

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPoint(Vector3 point) //'point' = desired pnt to move to 
    {
        agent.SetDestination(point); //agent always takes shortest path to destination
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);

            FaceTarget();
        }
    }

    //tracks a target:
    public void FollowTarget(Interactable newTarget)
    {
        //stops inside the newTarget's interactable raidus:
        agent.stoppingDistance = newTarget.interactionRadius * 0.8f; //multiplied by 0.8 to make sure we're 'IN' the newTarget's raidus
        
        agent.updateRotation = false; //don't rotate agent when following target

        target = newTarget.interactionTransform; //only point of this is to show in inspector what 'interactable pnt' should player be standing on?
    }

    //stops tracking any target:
    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f; //reset so walk ON, not just close to, anything selected w/ left click after
        
        agent.updateRotation = true; //only update agent rot when stop following target (so within target's radius or walking elsewhere)

        target = null;
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
}
