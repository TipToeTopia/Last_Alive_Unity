using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public GameObject introaudio; // refer and find the audio in the menu

    public void ControlsMenu () // goes to the controls scene
    {
        SceneManager.LoadScene(1);
    }

    public void IntroTowardMain () // goes to the actual game
    { 
       SceneManager.LoadScene(6);
    }

    public void Easy()
    {
        AIZombie.healthreduction = 25;
        SceneManager.LoadScene(2);
        GameManager.healthdecrease = 0.125f;
        introaudio = GameObject.Find("IntroAudio"); // find the specific intro audio
        Destroy(introaudio); // and destroy
    }

    public void Medium()
    {
        AIZombie.healthreduction = 20;
        SceneManager.LoadScene(2);
        GameManager.healthdecrease = 0.25f;
        introaudio = GameObject.Find("IntroAudio"); // find the specific intro audio
        Destroy(introaudio); // and destroy
    }

    public void Grounded()
    {
        AIZombie.healthreduction = 10;
        SceneManager.LoadScene(2);
        GameManager.healthdecrease = 0.4f;
        introaudio = GameObject.Find("IntroAudio"); // find the specific intro audio
        Destroy(introaudio); // and destroy
    }

    public void Controlstomenu() // goes back to menu
    {
        SceneManager.LoadScene(0);
    }
}
