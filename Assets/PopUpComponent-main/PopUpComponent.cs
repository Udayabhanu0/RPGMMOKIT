// PopUpComponent
// By Fallen
// Intended use create simple messages in game to guide your player
// Example "Press E to get inside the mine cart"
// PopUpObject was intended to be a text mesh object
// PopUp Occurs when player is in range

using Cysharp.Threading.Tasks;
using LiteNetLibManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace MultiplayerARPG
{
    public class PopUpComponent : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        public Camera cam;
        public GameObject CamObject;
        public GameObject Cutsence;
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
        void Start()
        {
            CamObject.SetActive(false);
            Cutsence.SetActive(false);
/*
            cam = GetComponent<Camera>();
*/
        }

        void Update()
        {
            if (playerDetected == true)
            {
                StartCoroutine(WaitForOneMinute());
                PopUpObject.transform.rotation = Quaternion.LookRotation(PopUpObject.transform.position - Camera.main.transform.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
          /*  Debug.Log("hiii");*/
            if (other.gameObject.GetComponent<PlayerCharacterEntity>() != null)
            {
                playerDetected = true;

                if (playerDetected == true)
                {

                    Debug.Log("hii its active now");
                    Cutsence.SetActive(true);
                    CamObject.SetActive(true);
                    PopUpObject.SetActive(true);
                    playableDirector.Play();
                    playerDetected = true;
                    
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
        IEnumerator WaitForOneMinute()
        {
            yield return new WaitForSeconds(8f);
            playableDirector.Stop();
            /*  cam.depth = -1;*/
            CamObject.SetActive(false);
            Cutsence.SetActive(false);
        }
    }
}
