using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    CharacterController cc; // refer to animator controller

    void Start()
    {
        cc = GetComponent<CharacterController>(); // get component
    }

    void Update()
    {
        if ((cc.velocity.magnitude > 0.1f) && (GetComponent<AudioSource>().isPlaying == false)) // if player is on the ground, and its directional velocity is above 0, given the audio isn't currently playing...
        {
            GetComponent<AudioSource>().volume = Random.Range(0.3f, 0.5f); // randomised for realism (variation)
            GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.95f); // randomised for realism (variation)
            GetComponent<AudioSource>().Play(); // we play
        }
    }
}
