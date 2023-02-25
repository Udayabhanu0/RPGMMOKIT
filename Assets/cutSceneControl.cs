using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
namespace MultiplayerARPG
{

    public class cutSceneControl : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        public Camera cam;
        public GameObject CamObject;
        public GameObject Cutsence;
        public GameObject player;

        public bool checkend = false;
        // Start is called before the first frame update
        void Start()
        {
            playableDirector.Stop();
            CamObject.SetActive(false);
            Cutsence.SetActive(false);

            cam = GetComponent<Camera>();

        }

        // Update is called once per frame
        void Update()
        {
           /* Debug.Log(player);*/
            if(player == null)
            {
            player = GameInstance.PlayingCharacterEntity.gameObject;

            }

            StartCoroutine(WaitForOneMinute());

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == player)
            {
                Cutsence.SetActive(true);
                CamObject.SetActive(true);
               /* Debug.Log("Name Of Player" + other.gameObject.name);*/

                cam.depth = 10;
                playableDirector.Play();
                checkend = true;

            }
        }
        IEnumerator WaitForOneMinute()
        {
            yield return new WaitForSeconds(10f);
            playableDirector.Stop();
            CamObject.SetActive(false);
            Cutsence.SetActive(false);
        }

    }
}
