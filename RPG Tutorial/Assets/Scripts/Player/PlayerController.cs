using UnityEngine.EventSystems; //used to access 'EventSystem' that comes with a canvas UI
using UnityEngine;


[RequireComponent(typeof(PlayerMotor))]
//Tells us where we want to move the player by casting a ray:
public class PlayerController : MonoBehaviour
{
    public LayerMask movementMask;
    public Interactable focus;
    
    private Camera cam;
    private PlayerMotor motor;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()) //don't need to reference specific inventory gameobj bc an 'EventSystem' is attached to it on creation of a canvas
        {
            return;
        }

        if(Input.GetMouseButtonDown(0)) //if left mouse click
        {
            //cast out ray from camera to whatever we clicked on:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); //'ScreenPointToRay' = shoots out an array FROM a pnt (the mousePosition here) on the screen
            RaycastHit hit;

            //cast out the ray itself:
            if(Physics.Raycast(ray, out hit, 100, movementMask)) //casts out 'ray' that only hits objs on layer 'movementMask' within '100' distance and stores hitInfo in 'hit'
            {
                //Debug.Log("We hit " + hit.collider.name + " " + hit.point); //prints out name of what we hit and coords of where we hit

                //Move our player to what we hit:
                motor.MoveToPoint(hit.point); //passes hit pnt to motor script and that sets the agent's destination as the pnt

                //Stop 'focusing' any objects (such as a UI interactable or attacking something):
                RemoveFocus();
            }
        }

        if (Input.GetMouseButtonDown(1)) //if right mouse click
        {
            //cast out ray from camera to whatever we clicked on:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); //'ScreenPointToRay' = shoots out an array FROM a pnt (the mousePosition here) on the screen
            RaycastHit hit;

            //cast out the ray itself:
            if (Physics.Raycast(ray, out hit, 100)) //casts out 'ray' that only hits objs on layer 'movementMask' within '100' distance and stores hitInfo in 'hit'
            {
                // Check if we hit an interactable, if so set as new focus of agent:
                Interactable interactable = hit.collider.GetComponent<Interactable>(); //get interactable scrip comp if there is one
                if(interactable != null) //if found an interactable comp on what we hit
                {
                    SetFocus(interactable); //set focus to it
                }
            }
        }
    }

    //set focus for motor and interactable + follow target:
    private void SetFocus(Interactable newFocus)
    {
        if(newFocus != focus) //if setting a new focus
        {
            if (focus != null) //if transitioning from one focus to another (otherwise it'd try to defocus a null focus)
            {
                focus.OnDefocused(); //defocus old focus 
            }

            focus = newFocus; //set as new focus

            //motor.MoveToPoint(newFocus.transform.position); //doesn't work for moving to enemies or anything that isn't still
            motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform); //notifies interactable everytime we click on it (even if mult times in a row)
    }

    //remove focus from motor and interactable + stop following target:
    private void RemoveFocus()
    {
        //only defocus if there is a current focus:
        if (focus != null)
        {
            focus.OnDefocused();
        }

        motor.StopFollowingTarget(); //stop following target

        focus = null; //clear focus
    }
}
