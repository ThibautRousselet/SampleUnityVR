using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FlaskAttributs;

// The goal of the game is to bring flask of the correct color to a recipient while being chased by zombies
// This script detects what kind of object the player is bringing back to the dedicated recipient
// When the good flask is brought, the map gets darker and more zombies spawn
{
    private Color targetColor;
    private int score = 0; //number of flasks brought

    public ZombieSpawn ZombieSpawn;

    private Collider otherObj; //Collider of the object in the recipient

    public GameObject HUD = null;
    private UnityEngine.UI.Text hudText = null;

    public GameObject corridorLights; //Collection of lamps from every corridors
    private List<GroupLamps> listCorridorLights = new List<GroupLamps>();
    public GroupLamps lobby;
    public GroupLamps room1;
    public GroupLampsEmergency emergencyLobby;
    public GroupLampsEmergency emergencyRoom1;

    void Start()
    {
        hudText = HUD.GetComponentInChildren<Text>();
        SetRandomTarget();
        ZombieSpawn = this.GetComponent<ZombieSpawn>();

        foreach (GroupLamps groupL in corridorLights.GetComponentsInChildren<GroupLamps>())
        {
            listCorridorLights.Add(groupL);
        }
    }

    //Called when an object is put in the recipient
    private void OnTriggerEnter(Collider collider)
    {
        otherObj = collider;
        Color otherColor = otherObj.gameObject.GetComponent<FlaskAttributs>().FlaskColor;

        //Check if the flask has the right color
        if (targetColor == otherColor)
        {
            OnDetection();
        }
        //Object is then destroyed
        Destroy(otherObj.gameObject);
    }

    void Update()
    {
        //Debug tool to automatically get to the next phase
        if (Input.GetKeyDown("p"))
        {
            OnDetection();
        }
    }

    //Called when the right flask is brought
    private void OnDetection()
    {
        score++; //Keep track of the number of flasks brought
        switch (score)
        {
            //Every phase changes the map and make it more dangerous
            case 1:
                Phase1();
                break;
            case 2:
                Phase2();
                break;
            case 3:
                Phase3();
                break;
            case 4:
                Phase4();
                break;
        }
    }

    private void Phase1()
    {
        //Light in 3 corridors are blinking
        listCorridorLights[2].setBlink(true);
        listCorridorLights[3].setBlink(true);
        listCorridorLights[4].setBlink(true);

        //spawn 10 zombies
        ZombieSpawn.zombieSpawn(10);
    }

    private void Phase2()
    {
        //Room 1 is shut down and emergency red lights are turned on
        emergencyRoom1.SetIntensity(1);
        room1.SetIntensity(0);

        ZombieSpawn.zombieSpawn(15);

        listCorridorLights[0].SetIntensity(0);
        listCorridorLights[3].SetIntensity(0);

        listCorridorLights[5].setBlink(true);
        listCorridorLights[6].setBlink(true);
    }

    private void Phase3()
    {
        listCorridorLights[2].SetIntensity(0);
        listCorridorLights[3].SetIntensity(0);
        listCorridorLights[4].SetIntensity(0);
        listCorridorLights[5].SetIntensity(0);
        listCorridorLights[6].SetIntensity(0);
        listCorridorLights[7].SetIntensity(0);

        lobby.SetIntensity(0);
        emergencyLobby.SetIntensity(1);

        ZombieSpawn.zombieSpawn(20);

    }

    //After bringing 3 flasks, the player wins
    private void Phase4()
    {
        hudText.text = "YOU WIN";
    }

    private void SetTarget(Color color)
    {
        targetColor = color;
        hudText.text = "Objective : \n Bring a " + targetColor.ToString() + " Flask";
    }

    //Set a random target color and print it on screen
    private void SetRandomTarget()
    {
        targetColor = GenerateRandomColor();
        hudText.text = "Objective : \n Bring a " + targetColor.ToString() + " Flask";
    }

    //Generate a random color
    private Color GenerateRandomColor()
    {
        Color col;
        float rand = Random.Range(0.0f, 5.0f);
        if (rand < 1.0f)
            coul = Color.White;
        else if (rand < 2.0f)
            coul = Color.Blue;
        else if (rand < 3.0f)
            coul = Color.Yellow;
        else if (rand < 4.0f)
            coul = Color.Red;
        else
            coul = Color.Green;
        return col;
    }
}
