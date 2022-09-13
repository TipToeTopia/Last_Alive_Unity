using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    // this script is called during an event in the axe attack animation

    public AudioSource axeswing;
    string enemyTag = "enemy";

    public void Attack()
    {
        axeswing.Play();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); // define all zombies pos through tags

        // We can now freely iterate through our array of zombies

        foreach (GameObject enemy in enemies)
        { 
            if (Vector3.Distance(enemy.transform.position, transform.position) < 5)
            {
                enemy.GetComponent<AIZombie>().ZombieHealth();
              
            }
        }
    }
}