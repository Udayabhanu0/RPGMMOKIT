using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MultiplayerARPG
{

    public class NPCCollider : MonoBehaviour
    {
        public delegate void NpcColliderCheckEvent(bool isColliding);

        public static event NpcColliderCheckEvent CollisionChecked;
        public GameObject player;
        private void Start()
        {
        }
        private void Update()
        {
            if (player == null) { 
            player = GameInstance.PlayingCharacterEntity.gameObject;
            
            }
        }
        private void OnTriggerEnter(Collider other)
        {
          
            
            if(other.gameObject == player)
            {
               
                CollisionChecked?.Invoke(true);

            }

           /* if (layerName == "Ignore Raycast")
            {
                Debug.Log("Hi");
                CollisionChecked?.Invoke(true);

            }*/
           
        }

        private void OnTriggerExit(Collider other)
        {
            CollisionChecked?.Invoke(false);
            if (other.gameObject == player)
            {
                Debug.Log("Hiexit");
            }
        }
    }
}
