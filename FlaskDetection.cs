using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FlaskAttributs;

//Le but du jeu et de ramener des fioles de la couleur demandee
//Cette classe permet de detecter le placement d une dans le recipient dediee 
// A chaque fois que le joueur amene la bonne fiole, le score augmente de 1, la carte s' assombrit et des zombies apparaissent pour compliquer le jeu
public class FlaskDetection : MonoBehaviour
{
    private Color targetColor;
    private int score = 0; //nb de fioles ramenées

    public ZombieSpawn ZombieSpawn;

    private Collider otherObj;//Collider de l objet place dans le recipient

    public GameObject HUD = null;
    private UnityEngine.UI.Text hudText = null;

    public GameObject corridorLights; //Collection de GroupeLampes de chaque couloir
    private List<GroupLamps> listCorridorLights = new List<GroupLamps>();
    public GroupLamps lobby;
    public GroupLamps room1;
    public GroupLampsEmergency emergencyLobby;
    public GroupLampsEmergency secoursRoom1;

    void Start()
    {
        hudText = HUD.GetComponentInChildren<Text>();
        SetRandomTarget();
        ZombieSpawn = this.GetComponent<ZombieSpawn>();

        //Place les groupes de lumieres dans une liste
        foreach (GroupLamps groupL in corridorLights.GetComponentsInChildren<GroupLamps>())
        {
            listCorridorLights.Add(groupL);
        }
    }

    //Appelee quand un objet est placee dans le recipient
    private void OnTriggerEnter(Collider collider)
    {
        otherObj = collider;
        Color otherColor = otherObj.gameObject.GetComponent<FlaskAttributs>().CouleurFiole;

        //Compare la couleur cible a celle de l'objet place
        if (targetColor == otherColor)
        {
            OnDetection();
        }
        //On detruit l'objet dans tous les cas
        Destroy(otherObj.gameObject);
    }

    void Update()
    {
        //On peut passer a la phase suivante en appuyant sur p pour tester
        if (Input.GetKeyDown("p"))
        {
            OnDetection();
        }
    }

    //On passe a la phase suivante quand la bonne fiole est ammenee
    private void OnDetection()
    {
        score++;
        switch (score)
        {
            //Chaque phase applique des modifications sur la map
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

    //A chaque phase, on eteind plus de lumieres et on fait apparaitre des zombies
    private void Phase1()
    {
        //Fait clignoter 3 couloirs
        listCorridorLights[2].setBlink(true);
        listCorridorLights[3].setBlink(true);
        listCorridorLights[4].setBlink(true);

        //spawn 10 zombies
        ZombieSpawn.zombieSpawn(10);
    }

    private void Phase2()
    {
        //On eteint la salle 1
        secoursRoom1.SetIntensity(1);
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

    //Apres avoir ramene 3 fioles le joueur gagne
    private void Phase4()
    {
        hudText.text = "YOU WIN";
    }

    private void SetTarget(Color color)
    {
        targetColor = color;
        hudText.text = "Objective : \n Bring a " + targetColor.ToString() + " Flask";
    }

    //Choisi aleatoirement une couleur cible et l affiche
    private void SetRandomTarget()
    {
        targetColor = GenerateRandomColor();
        hudText.text = "Objective : \n Bring a " + targetColor.ToString() + " Flask";
    }

    //Permet de generer une couleur aléatoire
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
