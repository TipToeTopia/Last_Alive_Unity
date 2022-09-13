using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerSwitch : MonoBehaviour
{
    public GameObject player; // player ref
    public GameObject keypadref; // keypad ref

    public TMP_Text powerswitchtext; // power

    private int x; // one time call

    public AudioSource power;

    void Start()
    {
        x = 1; // set at 1
        keypadref.SetActive(false); // power default switched off
        powerswitchtext.text = "";
    }

    void Update()
    {
        if (x == 1)
        {
            if ((Vector3.Distance(player.transform.position, keypadref.transform.position) < 5))
            {
                powerswitchtext.text = "NEEDS POWER";
            }
            else
            {
                powerswitchtext.text = "";
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && (Vector3.Distance(player.transform.position, transform.position) < 5) && (x==1)) // if player close and presses E (one time call)?
        {
            power.Play(); // power sound
            x = 2; // one time call
            keypadref.SetActive(true); // keypad has power therefore is now enabled
        }
    }
}
