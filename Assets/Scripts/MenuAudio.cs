using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    private static MenuAudio audio;

    void Awake()
    {
        if (audio != null)
        {
            Destroy(gameObject); // destroy existing audio so they aren't playing simultaneously
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject); // carry audio source over (intended for menu to controls and vice versa)
            audio = this; // and set the audio to the audio from the previous scene
        }
    }
}
