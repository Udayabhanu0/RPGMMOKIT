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
        public int DetectionRange = 3;
        public bool playerDetected = false;
        public bool checkend = false;
        // Start is called before the first frame update
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

            cam = GetComponent<Camera>();

        }

        // Update is called once per frame
        void Update()
        {
            StartCoroutine(WaitForOneMinute());

        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("other object" + other.gameObject.GetComponent<PlayerCharacterEntity>());
            if (other.gameObject.GetComponent<PlayerCharacterEntity>() != null)
            {
                Debug.Log("hi test");
                Cutsence.SetActive(true);
                CamObject.SetActive(true);


                cam.depth = 10;
                playableDirector.Play();
                checkend = true;

            }
        }
        IEnumerator WaitForOneMinute()
        {
            yield return new WaitForSeconds(5f);
            playableDirector.Stop();
            /*  cam.depth = -1;*/
            CamObject.SetActive(false);
            Cutsence.SetActive(false);
        }

    }
}
