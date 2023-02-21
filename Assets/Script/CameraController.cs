using Cinemachine;
using MultiplayerARPG;
using MultiplayerARPG.Cinemachine;
using UnityEngine;

namespace MultiplayerARPG
{


    public class CameraController : MonoBehaviour
    {
        public CMGameplayCameraController CM;
        public Transform player;
        public Transform NPC;
        public float height = 2f;
        public float distance = 5f;
        public float transitionSpeed = 2f;
        public GameObject TargetPos;

 
      
        

        private bool isFocused = false;
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        void Start()
        {


            NPCCollider.CollisionChecked += CheckedFunction;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }
        private void CheckedFunction(bool isColliding)
        {

            isFocused = isColliding;
        }

        void Update()
        {
            if(player == null)
            {
            player = GameInstance.PlayingCharacterEntity.EntityTransform;

            }
            if(NPC== null)
            {
                NPC = GameObject.FindGameObjectWithTag("NpcTag").transform;
            }
           
            Debug.Log(isFocused);
            if (isFocused)
            {
                Debug.Log("Changed pisition");
                Vector3 midPoint = player.transform.position;

                Vector3 targetPosition = new Vector3(midPoint.x, midPoint.y +1.5f+ height, midPoint.z - distance);
                TargetPos.transform.position = midPoint;

                transform.position = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);

                transform.LookAt(NPC);
                CM.virtualCamera1.LookAt = NPC.transform;
                
                CM.virtualCamera1.m_Follow = NPC.transform;
                CM.virtualCamera1.transform.position = targetPosition;
                CM.virtualCamera1.transform.position = targetPosition;
                CM.virtualCamera1.transform.rotation = Quaternion.LookRotation(CM.virtualCamera1.transform.position - player.position);
               /*
                CM.virtualCamera.m_UpdateMethod = CinemachineVirtualCameraBase.UpdateMethod.LateUpdate;*/
                CM.virtualCamera1.m_Priority = 10;
                CM.virtualCamera.m_Priority = 1; 
            }
            else
            {
                CM.virtualCamera1.m_Priority = 1;
                CM.virtualCamera.m_Priority = 10;
                transform.position = Vector3.Lerp(transform.position, initialPosition, transitionSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, transitionSpeed * Time.deltaTime);
            }
        }
    }
}