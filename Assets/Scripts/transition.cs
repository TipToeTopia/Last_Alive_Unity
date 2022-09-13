using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transition : MonoBehaviour
{
    // loading screen timer...

    private float Loadtimer = 5f;

    void Update() // loading timer
    {
        Loadtimer = Loadtimer - Time.deltaTime;
        if (Loadtimer <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }
}
