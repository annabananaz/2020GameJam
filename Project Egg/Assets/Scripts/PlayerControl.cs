using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float playerBaseSpeed;
    public float playerBaseJump;
    public float raycastJumpRange;

    private Rigidbody rb;
    private bool canJump;
    private bool isCrouching;
    private float playerHalfSpeed;
    private float playerOldSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOldSpeed = playerBaseSpeed;
        playerHalfSpeed = (playerBaseSpeed / 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checks if player can jump
        CheckGroundStatus();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = Camera.main.transform.forward * vertical * playerBaseSpeed * Time.deltaTime;
        Vector3 sidestep = Camera.main.transform.right * horizontal * playerBaseSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement + sidestep);

        if (Input.GetKeyDown("space") && canJump == true)
        {
            canJump = false;
            rb.velocity = new Vector3(0, (playerBaseJump * 2) * playerBaseJump * Time.deltaTime, 0);
        }

        // CROUCHING
        if (Input.GetKey(KeyCode.LeftControl) && !isCrouching)
        {
            Camera.main.transform.localPosition = new Vector3(0.0f, -0.25f, 0.0f);
            playerBaseSpeed = playerHalfSpeed;
            isCrouching = true;
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && isCrouching)
        {
            Camera.main.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            playerBaseSpeed = playerOldSpeed;
            isCrouching = false;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * raycastJumpRange, Color.green);

        if (Physics.Raycast(landingRay, out hit, raycastJumpRange))
        {
            float rayDistance = hit.distance;


            if (hit.collider == null)
            {
                canJump = false;
            }
            else
            {
                canJump = true;
            }

        }
    }
}
