using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float maxPitch;                      //The maximum pitch to go down
    public float minPitch;                      //The minimum pitch to go up
    public float cameraSpeed;                   //The speed of the camera movement
    //public Transform holdingPosTransform;       //The transform of the object in player's hands
    public bool lockCursor = true;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float isPausedSpeed;
    private bool m_cursorIsLocked = true;

    // Update is called once per frame
    void Update()
    {
        // USE WHEN PAUSE MENU IMPLEMENTED
        /*if (PauseMenuScript.gameIsPaused)
        {
            isPausedSpeed = 0.0f;
        }
        else
        {
            isPausedSpeed = 1.0f;
        }*/

        // REMOVE WHEN PAUSE MENU IMPLEMENTED 
        isPausedSpeed = 1.0f;

        yaw += cameraSpeed * isPausedSpeed * Input.GetAxis("Mouse X");                                  //Moves from left and right
        pitch -= cameraSpeed * isPausedSpeed * Input.GetAxis("Mouse Y");                                //Moves from up and down
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);                                 //Gives limits/parameters to prevent revolving 

        //holdingPosTransform.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);      //Turns the object in hand from up, down, left and right

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);                          //Turns the transform of where this script is located from up, down, left, and right

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
        {
            InternalLockUpdate();
        }
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0)) //&& !PauseMenuScript.gameIsPaused) READD WHEN PAUSE MENU IS IMPLMENTED
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
