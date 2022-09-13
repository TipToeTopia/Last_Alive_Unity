using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWonTrigger : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        // Find the player
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the other gameobject is not the player, return
        if (other.gameObject != player)
        {
            return;
        }
        else
        {
            Invoke("GameWon", 1.0f);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it is not the player, return
        if (other.gameObject != player)
        {
            return;
        }
        else
        {
            CancelInvoke("GameWon");
        }
    }

    public void GameWon()
    {
        GameManager.Instance.GameWonScene();
    }
}
