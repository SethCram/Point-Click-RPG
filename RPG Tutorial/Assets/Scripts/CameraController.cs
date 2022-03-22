using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; //'target' = transform to follow

    public Vector3 offset; //cam 'offset' from target
    public float pitch = 2f; //2 bc that moves focus from our player's feet to his head
    public float zoomSpeed = 4f;
    public float minZoom = 5f; //needed bigger than 0 so cam can't be at same position as player
    public float maxZoom = 15f; //needed so can still see player
    public float yawSpeed = 100; //cam rot speed


    private float currZoom = 10f;
    private float currYaw = 0f; //angle of cam's rot around target

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //storing zoom tween min and max w/ scrollwheel:
        currZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; //subtracted bc otherwise it's inverted
        currZoom = Mathf.Clamp(currZoom, minZoom, maxZoom);

        //stored for rotting cam around target:
        currYaw -= Input.GetAxis("Horizontal") * yawSpeed *Time.deltaTime; //'Horizontal' = left/right arrow keys or left/right on joystick or a and d
    }

    //called right after update: (little later order of execution) (so currZoom is set beforehand)
    private void LateUpdate()
    {
        //cam zooms closer/farther from target:
        transform.position = target.position - offset * currZoom; // set/change cam's curr position (more zoom = closer)

        //cam looks at 'pitch' above target:
        transform.LookAt(target.position + Vector3.up * pitch); //need pitch bc 'target' is at the base of player's feet

        //rot cam around the target:
        transform.RotateAround(target.position, Vector3.up, currYaw); //rot cam around the target's 'up'/y axis at 'currYaw' angle
    }
}
