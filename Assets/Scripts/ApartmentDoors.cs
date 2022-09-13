using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentDoors : MonoBehaviour
{
    private Animator animator;
    GameObject player; // player

    public AudioSource dooropen;
    public AudioSource doorclose;

    private int y;

    private void Start()
    {
        animator = GetComponent<Animator>(); // ref to animator component
        player = GameObject.Find("Player");
        y = 1;
    }

    private void Update()
    {
        GameObject[] Doors = GameObject.FindGameObjectsWithTag("ApartmentDoors");

        foreach (GameObject Door in Doors)
        {
            if ((Vector3.Distance(Door.transform.position, player.transform.position) < 5))
            {
                if (Input.GetKeyDown(KeyCode.E) && (y == 1))
                {
                    Door.GetComponent<Animator>().SetBool("open", true);
                    dooropen.Play();
                    y = 2;
                }

                if (Input.GetKeyDown(KeyCode.R) && (y == 2))
                {
                    Door.GetComponent<Animator>().SetBool("open", false);
                    doorclose.Play();
                    y = 1;
                }
            }
        }
    }
}
