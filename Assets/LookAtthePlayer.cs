using MultiplayerARPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtthePlayer : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameInstance.PlayingCharacterEntity.transform;
        }
        transform.LookAt(player.position);
    }
}
