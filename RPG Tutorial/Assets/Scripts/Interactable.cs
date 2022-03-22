using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 3f; // distance player has to be from obj to interact with it

    public Transform interactionTransform;

    private bool isFocus = false;
    private bool hasInteracted = false;

    private Transform player;


    public virtual void Interact() //'virtual' so this method can be overwritten in any children classes of this class (dif for each child class)
    {
        //this method meant to be overwritten

        Debug.Log("Interacting w/: " + transform.name);
    }

    private void Update()
    {
        if(isFocus == true && !(hasInteracted)) //if this interactable is the focus and it hasn't already been interacted with
        {
            //check distance from this interactable/interactable pnt to player:
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(distance <= interactionRadius) //if player within interactionRadius, interact
            {
                //Debug.Log("Interact");
                Interact();

                hasInteracted = true; //so only interacts once per right click
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;

        hasInteracted = false; //so everytime focus obj, interact once

        player = playerTransform;
    }

    public void OnDefocused()
    {
        isFocus = false;

        hasInteracted = false;

        player = null;
    }

    //draw yellow wire sphere around 'interactionTransform' obj when interactable obj it's selected:
    private void OnDrawGizmosSelected() //lets us draw graphics in the scene
    {
        //if no interactable point set, set it to this obj:
        if(interactionTransform == null)
        {
            interactionTransform = transform;
        }

        //draw yellow wire sphere around this obj:
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }
}
