using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public GameObject panal; // fade background 
    public GameObject player; // ref to player
    public GameObject axe; // ref to axe object on player

    public bool note; // use E for on and off note

    public void Start() // initialised to false
    {
      panal.SetActive(false);
      axe.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (Vector3.Distance(player.transform.position, transform.position) < 5))
        {
            note = !note;

            if (note)
            {
                panal.SetActive(true); // fade active
                player.GetComponent<Move>().enabled = false; // cannot move
                player.GetComponentInChildren<Look>().enabled = false; // cannot look around
                axe.SetActive(false); // axe dissapears
            }

            if (!note)
            {
               panal.SetActive(false);
               player.GetComponent<Move>().enabled = true;
               player.GetComponentInChildren<Look>().enabled = true;
               axe.SetActive(true);
            }
        }
    }
}
