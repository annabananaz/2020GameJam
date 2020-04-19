using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public GameObject FirstPersonCamera;
    static float sightDistance;                                //Raycast length
    public static bool isHolding;                       //Checks if the player is already holding an object
    public static Transform playerHoldingPosition;      //Transform where the object will be held
    public static RaycastHit hit;                       //Raycast hit

    // Start is called before the first frame update
    void Start()
    {
        FirstPersonCamera = GameObject.Find("Main Camera").gameObject;
        playerHoldingPosition = GameObject.Find("HoldingPosition").transform;
        sightDistance = 4.5f;
        isHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = FirstPersonCamera.transform.TransformDirection(Vector3.forward);                  //Uses the FirstPersonCamera as the Ray
        Debug.DrawRay(FirstPersonCamera.transform.position, forward * sightDistance, Color.yellow);         //Debug draws the raycast lines

        if (Physics.Raycast(FirstPersonCamera.transform.position, forward, out hit, sightDistance))
        {
            float rayDistance = hit.distance;
            if (rayDistance <= sightDistance)
            {
                if (hit.transform.tag != "Player")
                {
                    //Pick Up physics Object
                    if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        //Physically Carry the Object
                        if (!isHolding)
                        {
                            //If wanting to pick up & move object
                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                CarryObject(hit.transform.gameObject);
                            }
                        }
                        //Physically Drop the Object
                        else if (isHolding && (playerHoldingPosition.childCount > 0))
                        {
                            //if wanting to release object
                            if (Input.GetKeyUp(KeyCode.Mouse0))
                            {
                                //print("Right released");
                                DropObject(hit.transform.gameObject);
                            }

                            if (Input.GetKey(KeyCode.Mouse1))
                            {

                                ThrowObject(hit.transform.gameObject, hit);
                            }
                        }
                    }
                    // Press button
                    if (hit.collider.tag.Equals("Button"))
                    {
                        // Buttons for obstacles
                    }
                }
            }
        }
    }

    //==========================
    //CARRY OBJECT
    //==========================
    public void CarryObject(GameObject hitObject)
    {

        if (hitObject.transform.parent != null && !(hitObject.transform.parent.name.Equals(playerHoldingPosition.name)) && !(hitObject.transform.parent.tag.Equals("Untagged")))
        {
            hitObject = hitObject.transform.parent.gameObject;
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = true;
        hitObject.GetComponent<Rigidbody>().useGravity = false;
        hitObject.transform.position = playerHoldingPosition.position;
        hitObject.transform.parent = GameObject.Find("HoldingPosition").transform;
        sightDistance = 3.5f;
        isHolding = true;
    }

    //==========================
    //DROP OBJECT
    //==========================
    public static void DropObject(GameObject hitObject)
    {
        print("PSS says DROPPING");
        if (hitObject.transform.parent != null && !(hitObject.transform.parent.name.Equals(playerHoldingPosition.name)))
        {
            hitObject = hitObject.transform.parent.gameObject;
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = false;
        hitObject.GetComponent<Rigidbody>().useGravity = true;
        hitObject.transform.parent = null;
        sightDistance = 7.0f;
        isHolding = false;
    }

    //==========================
    //THROW OBJECT
    //==========================
    public void ThrowObject(GameObject hitObject, RaycastHit hit)
    {
        if (hitObject.transform.parent != null && (!(hitObject.transform.parent.name.Equals(playerHoldingPosition.name)) && !(hitObject.transform.parent.name.Equals("GravPos"))))
        {
            hitObject = hitObject.transform.parent.gameObject;
        }

        hitObject.GetComponent<Rigidbody>().isKinematic = false;
        hitObject.GetComponent<Rigidbody>().useGravity = true;
        hitObject.transform.parent = null;
        hit.rigidbody.AddForce(-hit.normal * 600);
        isHolding = false;
    }
}
