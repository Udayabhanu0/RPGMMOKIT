using UnityEngine;
using UnityEngine.UI;

public class SpawnOnButtonClick : MonoBehaviour
{
    public GameObject[] objectToSpawn;
    public Button Power1;
    public Button Power2;
    public Button Power3;
    public Button Power4;
    public Button Power5;
     public GameObject player;
     public int posy =0;
    private void Start()
    {
        player  = GameObject.FindWithTag("PlayerTag");

        Power1.onClick.AddListener(Power1animation);
        Power2.onClick.AddListener(Power2animation);
        Power3.onClick.AddListener(Power3animation);
        Power4.onClick.AddListener(Power4animation);
        Power5.onClick.AddListener(Power5animation);
    }

    private void Power1animation()
    {
        Instantiate(objectToSpawn[0], player.transform.position, transform.rotation);
    }
    private void Power2animation()
    {
        Vector3 spawnPosition = player.transform.position;
        spawnPosition.y += posy;
     
        Instantiate(objectToSpawn[1], spawnPosition, transform.rotation);
    }
    private void Power3animation()
    {
        Instantiate(objectToSpawn[2], player.transform.position, transform.rotation);
    }
    private void Power4animation()
    {
        Instantiate(objectToSpawn[3], player.transform.position, transform.rotation);
    }
    private void Power5animation()
    {
        Instantiate(objectToSpawn[4], player.transform.position, transform.rotation);
    }
}

