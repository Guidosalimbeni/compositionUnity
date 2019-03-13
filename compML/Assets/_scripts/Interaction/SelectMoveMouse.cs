using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectMoveMouse : MonoBehaviour
{
    
    public GameObject primitivePrefab; // the prefab object you will create and control must have a rigidbody component!
    public float speed = 10f; // the speed w a s d will move the rigidbody
    public float gravity = 10f; // the speed the rigid body will be pulled down
    public float maxVelocityChange = 10; // for clamping rigidbodys new velocity
    private GameObject currentlyControlled; // the rigidbody w a s d will move
    private bool isFire1Held = false; // without these two Fire1 and Fire2 actions would be done every frame
    private bool isFire2Held = false;
    private RaycastHit Fire1hitinfo; // these store the collision data of the latest raycast
    private RaycastHit Fire2hitinfo;
    public Rigidbody rb;

    private void Start()
    {
        rb = primitivePrefab.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isFire1Held)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // casts a ray from the center of the camer to the mouse pointer
            if (Physics.Raycast(ray, out Fire1hitinfo))
                Instantiate(primitivePrefab, Fire1hitinfo.point, transform.rotation); // create the object where the ray collided
            isFire1Held = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isFire1Held = false;
        }
        if (Input.GetButtonDown("Fire2") && !isFire2Held)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // casts a ray from the center of the camer to the mouse pointer
            if (Physics.Raycast(ray, out Fire2hitinfo))
            {
                currentlyControlled = Fire2hitinfo.collider.gameObject; // set the currently controlled rigid body to the object the raycast hit
                currentlyControlled.transform.rotation = new Quaternion(0, 0, 0, 0); // adjust the rigidbodys rotation so w a s d will properly control it
                                                                                     //currentlyControlled.rigidbody.constraints = RigidbodyConstraints.FreezeRotation; // freeze the rotation so moving it will not mess up proper control
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            isFire2Held = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isFire2Held = false;
        }
    }
    void FixedUpdate()
    {
        if (currentlyControlled != null)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); // this vector is made from axis(s) held
            targetVelocity = currentlyControlled.transform.TransformDirection(targetVelocity);
            targetVelocity *= speed; // multiple our target velocity by speed (as it is only 0 1 or -1 right now)
                                     //Vector3 velocity = currentlyControlled.rigidbody.velocity; // add our velocity to controlled rigidbody
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity); // this is for acceleration
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            //currentlyControlled.rigidbody.AddForce(velocityChange, ForceMode.VelocityChange); // move the rigidbody in game space
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}
