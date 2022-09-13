using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Animator animator; // animator

    public void Start()
    {
        animator = GetComponentInChildren<Animator>(); // ref to animator component
    }

    public CharacterController playercontrols; // calling the character controls that will be used for the player
    public float speed; // player walking/ jogging speed (initialised)
    public float walking = 7f;
    public float sprintspeed = 14f;
   
    Vector3 v; // velocity

    [HideInInspector]
    public float g = -20f; // gravity (not 9.81 as it doesn't simulate a realisitc gravity in this case).

    public AudioSource running;

    void FixedUpdate() // good for physics/ maths calculation (improves optimisation)
    { 
        if ((Input.GetMouseButtonDown(0))) // if player clicks left mouse down...
        {
          animator.SetTrigger("Attack"); // we call the animation
        }

        float x = Input.GetAxis("Horizontal"); // inbuilt functionality for the "a - d" in unity
        float z = Input.GetAxis("Vertical"); // and "w - s" keyboard directions 

        Vector3 move = (transform.right * x) + (transform.forward * z);
        playercontrols.Move(move * (speed * Time.deltaTime)); // Move allows us to pass the move variable and is called

        v.y += g * Time.deltaTime; // creation of gravity (y coordinates); we call delta time twice
        playercontrols.Move(v * Time.deltaTime); // applying the formula y = v * (0.5*t^2) we need to call delta time twice to find the vertical velocity when the player is on a declined slope to apply gravity

        if (Input.GetKeyDown(KeyCode.Space))
        {
           Invoke("Breathe", 5f); // so heavy breathing sound isn't instant upon running
        }

        if (Input.GetKey(KeyCode.Space) && z == 1) // if the player holds "space" whilst moving, they sprint (only if simultanously holding "w" (z calls the vertical controls where 0 is "s" and 1 is "w", therefore we use 1)).
        {
           speed = sprintspeed; // speed increases  
        }
        else
        {
           speed = walking; // returns to walking speed
           running.Stop(); // returns to walking/ jogging breathe
        }
    }

    void Breathe()
    {
        running.Play();
    }
}
