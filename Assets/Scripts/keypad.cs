using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class keypad : MonoBehaviour
{
    public GameObject door; // ref to door
    public GameObject player; // ref to player
    public GameObject axe; // ref to axe assigned on player
    public GameObject disabler; // dissabled the collider object after completion

    public Animator animation = null; // door animation set to null

    public GameObject objectToEnable; // keypad enabler

    public string curPassword = "222"; // password string
    public string input; // input
    public Text displayText; // text

    public AudioSource audioincorrectcombo; // incorrect audio
    public AudioSource audiorightcombo; // right audio

    // local private variables

    private bool keypadScreen;
    private float btnClicked = 0;
    private float numOfGuesses;

    void Start()
    {
        btnClicked = 0; // times the button was clicked
        numOfGuesses = curPassword.Length; // password length.
        animation = door.GetComponent<Animator>(); // door animator
    }

    void Update()
    {
        if (btnClicked == numOfGuesses) // if equals 3?
        {
            if (input == curPassword) // if 3 digit number equals password?
            {
                Debug.Log("Correct Password!");
                input = ""; // clear Password
                btnClicked = 0; // reset
                disabler.SetActive(false); // disable keycode collider
                keypadScreen = false; // remove keypad from screen by setting the "if" to false
                objectToEnable.SetActive(false); // disable keypad on screen
                player.GetComponent<Move>().enabled = true; // can now move
                player.GetComponentInChildren<Look>().enabled = true; // can now look
                axe.SetActive(true); // axe reappears
                audiorightcombo.Play(); // correct passcode audio
                animation.SetBool("open", true); // door opens
            }
            else
            {
                input = "";
                displayText.text = input.ToString();
                audioincorrectcombo.Play();
                btnClicked = 0;
                disabler.SetActive(true);
            }
        }
    }

    void OnGUI()
    {
        if (Input.GetMouseButtonDown(1)) // if player clicks right on mouse...
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
               var selection = hit.transform;

                if (selection.CompareTag("keypad")) // if it hit the keypad?
                {
                    keypadScreen = true; // enable the "if" to true
                    player.GetComponent<Move>().enabled = false; // cannot move
                    player.GetComponentInChildren<Look>().enabled = false; // cannot look
                    axe.SetActive(false); // removes axe

                   var selectionRender = selection.GetComponent<Renderer>();
                    if (selectionRender != null) // render exception
                    {
                        keypadScreen = true;
                    }
                }
            }
        }

        if (keypadScreen) // "if" keypad true or false?
        {
            objectToEnable.SetActive(true);
        }
    }

    public void ValueEntered(string valueEntered) // reset, quit and default scenarios on the keypad...
    {
        switch (valueEntered)
        {
            case "Q": // QUIT
           
                objectToEnable.SetActive(false);
                btnClicked = 0;
                keypadScreen = false;
                input = "";
                displayText.text = input.ToString();
                player.GetComponent<Move>().enabled = true;
                player.GetComponentInChildren<Look>().enabled = true;
                axe.SetActive(true);
                break;

            case "C": // CLEAR

                input = "";
                btnClicked = 0;// Clear Guess Count
                displayText.text = input.ToString();
                break;

            default: // Buton clicked add a variable

                btnClicked++; // Add a guess
                input += valueEntered;
                displayText.text = input.ToString();
                break;
        }
    }
}
