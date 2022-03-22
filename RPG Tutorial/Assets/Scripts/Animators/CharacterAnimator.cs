using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    const float locomotionAnimationSmoothTime = 0.1f; //const bc it never changes
    private NavMeshAgent agent; //can get current speed of character with this

    protected Animator animator; //'protected' so accessable to inherited classes
    protected CharacterCombat combat;
    protected AnimatorOverrideController animatorOverrideController; //to override anims and replace them
    
    //to keep track of all our atk anims:
    protected AnimationClip[] currAtkAnimSet;
    public AnimationClip[] defaultAtkAnimSet; //drag+dropped into
    public AnimationClip replaceableAtkAnim; //drag+dropped into

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        combat = GetComponent<CharacterCombat>();

        //override controller linked to scene animator controller:
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController); //create a new instance of the animator constroller used during runtime 
        animator.runtimeAnimatorController = animatorOverrideController;

        //initialize current atk anim set:
        currAtkAnimSet = defaultAtkAnimSet;

        //subscribe atttack method to it's event:
        combat.OnAttackCallback += OnAttack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //set rate of transitioning tween states in blendtrees and pass our current speed (0 to 1):
        float speedPercent = agent.velocity.magnitude / agent.speed; // agent's curr speed / max speed
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime); //dampTime = time taken to smooth tween two values ('locomotionAnimationSmoothTime' here)

        //update whether in combat:
        animator.SetBool("inCombat", combat.inCombat);

    }

    //activated by the 'onAtkCallback':
    protected virtual void OnAttack()
    {

         //set 'attack' param to true:
         animator.SetTrigger("attack");

         //swap out default atk anim for rando anim in curr atk set:
         int attackIndex = Random.Range(0, currAtkAnimSet.Length);
         animatorOverrideController[replaceableAtkAnim.name] = currAtkAnimSet[attackIndex];

    }

    //called by 'CharacterStats' to activate death anim:
    public void Die()
    {
        animator.applyRootMotion = true;
        animator.SetTrigger("dead");
    }
}
