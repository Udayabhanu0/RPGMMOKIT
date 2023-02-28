// PopUpComponent
// By Fallen
// Intended use create simple messages in game to guide your player
// Example "Press E to get inside the mine cart"
// PopUpObject was intended to be a text mesh object
// PopUp Occurs when player is in range

using Cysharp.Threading.Tasks;
using LiteNetLibManager;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    public class PopUpComponentonly : MonoBehaviour
    {
        [Category("Gather System Settings")]
        public GameObject PopUpObject;
        public int DetectionRange = 3;
        public bool playerDetected = false;

        public void Awake()
        {
            transform.gameObject.AddComponent(typeof(SphereCollider));
            SphereCollider sphereCollider = (SphereCollider)GetComponent(typeof(SphereCollider));
            sphereCollider.center = Vector3.zero;
            sphereCollider.isTrigger = true;
            sphereCollider.radius = DetectionRange;
        }

        void Update()
        {
            if (playerDetected == true)
            {
                PopUpObject.transform.rotation = Quaternion.LookRotation(PopUpObject.transform.position - Camera.main.transform.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerCharacterEntity>() != null)
            {
                playerDetected = true;

                if (playerDetected == true)
                {
                    PopUpObject.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            playerDetected = false;

            if (playerDetected == false)
            {
                PopUpObject.SetActive(false);
            }
        }
    }
}
