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
        [Category("Gather System Settings")]
        public GameObject PopUpObject;
        public int DetectionRange = 5;
        public bool playerDetected = false;

        public void Awake()
        {
            transform.gameObject.AddComponent(typeof(SphereCollider));
            SphereCollider sphereCollider = (SphereCollider)GetComponent(typeof(SphereCollider));
            sphereCollider.center = Vector3.zero;
            sphereCollider.isTrigger = true;
            sphereCollider.radius = DetectionRange;
        }


        private void Start()
        {
            /*player = GameInstance.PlayingCharacterEntity.gameObject;*/
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("hiii its the npc");
            /*Debug.Log("Checked");
            Debug.Log(other.gameObject.tag);
            int layer = other.gameObject.layer;
            string layerName = LayerMask.LayerToName(layer);
            Debug.Log(layerName);*/
            if(other.gameObject.GetComponent<PlayerCharacterEntity>() != null)
            {
                Debug.Log("hiiii");
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
            if (other.gameObject.CompareTag("PlayerTag"))
            {
            CollisionChecked?.Invoke(false);
                Debug.Log("Hiexit");
            }
        }
    }
}
