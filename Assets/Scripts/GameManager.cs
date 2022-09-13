using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static float healthdecrease;

    [SerializeField]
    private Image healthbar; // health image bar 

    private float health = 100; // player health

    private float HealthBar; //  health value

    private float healthlimit = 100; // if health is already above 75 and healthpack are obtained, it shouldnt exceed its max health charge, therefore it reaches max 100

    public AudioSource pickuphealth; // pickup health sound

    private int x; // one time call

    GameObject player; // player

    public TMP_Text healthtext; // Health

    void Start()
    {
        player = GameObject.Find("Player");
        x = 1;
        health = 100;
        healthtext.text = "";
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance) // if already an instance?
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // GameManager instance
    private static GameManager instance = null;

    public void PlayerHealth() // player health function 
    {
        health = health - healthdecrease; // health constant decline, given zombie is within range
        HealthBar = health / 100f; // to translate the health as a value between 0 and 1 that can move the health bar left to right
        healthbar.fillAmount = HealthBar; // to de-fill the image

        PlayerDead();
    }

    public void PlayerDead() // Player Death
    {
        if (health <= 0) // player dead condition
        {
            health = 0;
            SceneManager.LoadScene(5);
        }
    }

    public void GameWonScene() // Game Won Screen
    {
        SceneManager.LoadScene(4);
    }

    public void Update()
    {

        GameObject[] HealthPacks = GameObject.FindGameObjectsWithTag("HealthKit");

        if (health == 100)
        {
            healthtext.text = "MAX";
            return;
        }
        else
        {
            healthtext.text = "";

            foreach (GameObject HealthPack in HealthPacks) // collectables that will increase the health of the health by 25%, unless heath is greater then 75% (will max too 100%)
            {
                if ((Vector3.Distance(HealthPack.transform.position, player.transform.position) < 5)) // if close to healthpack?
                {

                    if ((Input.GetKeyDown(KeyCode.E) && (x == 1))) // if player presses E (one time call)?
                    {
                        Destroy(HealthPack);
                        pickuphealth.Play();

                        if (health > 75) // if health is already above 75 we will just increase is to max 100 (no further) e.g if health was 81% we would increase by 19%
                        {
                            healthlimit = 100 - health;
                            health = health + healthlimit;
                            HealthBar = health / 100f; // to translate the healthpack health as a value between 0 and 1 that can move the health bar left to right
                            healthbar.fillAmount = HealthBar; // to de-fill the image
                        }
                        else // else we increase by 25%
                        {
                            health = health + 25f;
                            HealthBar = health / 100f; // to translate the healthpack health as a value between 0 and 1 that can move the health bar left to right
                            healthbar.fillAmount = HealthBar; // to de-fill the image
                        }
                    }
                }
            }
        }
    }
}
