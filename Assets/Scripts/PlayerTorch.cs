using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTorch : MonoBehaviour
{
    public GameObject Torch; // calling the spotlight object for the torch in Unity
    public GameObject BatteryConstant; // cube for a const calling of deducting battery health
    public GameObject Panal;

    [SerializeField]
    private Image batterybar; // battery image

    private float battery = 100; // battery health

    private float BatteryBar; // battery value

    private bool lightactive; // to use the same F button down on both torch on/ off instances

    private float Dist; // dist between torch and cube for battery health

    private float healthlimit = 100; // if health is already above 75 and batteries are obtained, it shouldnt exceed its max battery charge, therefore it reaches max 100

    public AudioSource torchonoroff; // torch sound
    public AudioSource pickupbatteries; // pickup batteries sound

    private int x; // one time call
    private int y;

    public TMP_Text healthtext; // Battery Health

    private void Start()
    {
       healthtext.text = "";
       Torch.SetActive(false); // initially, the torch is assumed to be off
       BatteryConstant.SetActive(false); // cube off
       x = 1;
       y = 1;
    }

    private void Update()
    {
        GameObject[] Batteries = GameObject.FindGameObjectsWithTag("Batteries");

        if (battery == 100)
        {
            y = 1;
            healthtext.text = "MAX";
        }
        else
        {
            y = 2;
            healthtext.text = "";
        }

        if (y == 2)
        {

            foreach (GameObject Battery in Batteries) // collectables that will increase the health of the battery by 25%, unless heath is greater then 75% (will max too 100%)
            {
                if ((Vector3.Distance(Battery.transform.position, transform.position) < 5)) // if close to batteries?
                {
                    if ((Input.GetKeyDown(KeyCode.E)) && (x == 1)) // if player presses E (one time call)?
                    {
                        Destroy(Battery);
                        Torch.SetActive(false);
                        BatteryConstant.SetActive(false);
                        pickupbatteries.Play();

                        if (battery > 75) // if health is already above 75 we will just increase is to max 100 (no further) e.g if health was 81% we would increase by 19%
                        {
                            healthlimit = 100 - battery;
                            battery = battery + healthlimit;
                            BatteryBar = battery / 100f; // to translate the battery health as a value between 0 and 1 that can move the health bar left to right
                            batterybar.fillAmount = BatteryBar; // to de-fill the image
                        }
                        else // else we increase by 25%
                        {
                            battery = battery + 25f;
                            BatteryBar = battery / 100f; // to translate the battery health as a value between 0 and 1 that can move the health bar left to right
                            batterybar.fillAmount = BatteryBar; // to de-fill the image
                        }
                    }
                }
            }
        }
           
        Dist = Vector3.Distance(BatteryConstant.transform.position, transform.position);

        if (battery <= 0) // if torch runs out of battery?
        {
            battery = 0;
            Torch.SetActive(false);
            return;
        }
        else
        {
            if (Panal.activeInHierarchy == true) // if currently reading note?
            {
                return;
            }
            else // torch can be enabled and disabled then...
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    lightactive = !lightactive;

                    if (lightactive)
                    {
                        BatteryConstant.SetActive(true);
                        Torch.SetActive(true);
                        torchonoroff.Play();
                    }

                    if (!lightactive)
                    {
                        BatteryConstant.SetActive(false);
                        Torch.SetActive(false);
                        torchonoroff.Play();
                    }
                }

                if (BatteryConstant.activeInHierarchy == true) // if torch is on?
                {
                    if (Dist < 10)
                    {
                        battery = battery - 0.02f; // battery health constant decline
                        BatteryBar = battery / 100f; // to translate the battery health as a value between 0 and 1 that can move the health bar left to right
                        batterybar.fillAmount = BatteryBar; // to de-fill the image
                    }
                }
            }
        }
    }
}
