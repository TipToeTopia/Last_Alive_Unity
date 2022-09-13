using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Look : MonoBehaviour
{
    public GameObject keypadref; // ref to keypad
    public GameObject listenmode; // player see zombie through walls mode
    public GameObject player; // ref to player
    public GameObject axe;

    public float sensititvity = 100f; // sensitivity of mouse movement
    public Transform playerbody;
    float rotation = 0f;

    public bool listenmodeactive; // set off
    public TMP_Text listenmodetext; // listen mode

    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked; // cursor is set to non visible
       listenmode.SetActive(false); // initialsed as false
       listenmodetext.text = "LISTEN MODE INACTIVE";
    }

    public void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensititvity * Time.deltaTime; // Unity has built in values for the mouse which we will use for looking
        float mouseY = Input.GetAxis("Mouse Y") * sensititvity * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -45f, 45f); // limited angle
        transform.localRotation = Quaternion.Euler(rotation, 0f, 0f); // Euler used to set 3 parameters as floats for the rotation
        playerbody.Rotate(Vector3.up * mouseX); // X rotation

        if (keypadref.activeInHierarchy == true) // cursor appears if close to keypad, given it hasnt already been completed
        {
            if (Vector3.Distance(keypadref.transform.position, transform.position) < 5)
            {
                Cursor.lockState = CursorLockMode.None; // cursor is visible
            }
            if (Vector3.Distance(keypadref.transform.position, transform.position) > 5)
            {
                Cursor.lockState = CursorLockMode.Locked; // cursor is invisible
            }
        }
        else // however, if complete its set to locked
        {
            Cursor.lockState = CursorLockMode.Locked; // cursor is invisible
        }

        if (Input.GetKeyDown(KeyCode.Q)) // controls for listen mode
        {
            listenmodeactive = !listenmodeactive;

            if (listenmodeactive)
            {
                listenmodetext.text = "LISTEN MODE ACTIVE";
                listenmode.SetActive(true);
                axe.SetActive(false);
                player.GetComponent<Move>().enabled = false; // cannot move
            }

            if (!listenmodeactive)
            {
                listenmodetext.text = "LISTEN MODE INACTIVE";
                listenmode.SetActive(false);
                axe.SetActive(true);
                player.GetComponent<Move>().enabled = true; // cannot move
            }
        }
    }
}
