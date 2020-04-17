using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//This script handles the battery and the light of the torchlight
//The torchlight can be recharged by being shaked and has 4 possible states :
// - Recharge : The player is shaking the controller and battery increases, the light turns blue for 2 sec when the battery is full
// - Overload : The player is pressing the overload button, the light becomes red and kill zombies in its range, battery consumtion increased
// - Empty : The battery is empty, no more light
// - Normal : Regular white light

public class FlashlightBattery : MonoBehaviour
{
    public float Battery;
    public float MaxBattery;

    public Light Light;
    public SteamVR_Action_Boolean IsOverload = null;

    private float timerMaxBatteriy;//Time during which light turns blue
    private Vector3 currentPos;
    private Vector3 previousPos;


    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = this.transform.position;

	//If overload button is pressed, the battery is consumed and light is red
        if (IsOverload.state && Battery>0)
        {
            Light.color = Color.red;
            Light.intensity = 3;
            Battery-=0.5f;
        }
        //Test if the flashlight is shaked based on its movement value
        else if (Vector3.Distance(currentPos, previousPos)>0.2)
        {
            //Make sure maxBattery is not exceeded
            if (Battery < MaxBattery)
            {
                Battery+=10;
            } 
            //If battery is full, light turns blue
            else if (timerMaxBatteriy <= 0)
            {
                timerMaxBatteriy = 2;
                Light.color = Color.cyan;
                Light.intensity = 1;
            }
        } 
        else
        //Otherwise, regular white light
        {
            Light.color = Color.white;
            Light.intensity = 1;
        }

        //Battery is consumed at each update
        Battery -= Time.deltaTime;
        //The consumption is random so the player cant predict the battery duration
        if (Random.value > 0.995)
        {
            Battery--;
        }

        //No more light if no battery
        if (Battery < 0)
        {
            Battery = 0;
            Light.enabled = false;
            Light.intensity = 1;

        } else
        {
            Light.enabled = true;
        }

        //Update values for the next call
        previousPos = currentPos;
        timerMaxBatteriy -= Time.deltaTime;
    }
}
