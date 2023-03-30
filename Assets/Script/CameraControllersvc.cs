using MultiplayerARPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllersvc : MonoBehaviour
{
    public float minDistance;
    public string npcTag = "NpcTag";
    public Camera mainCamera;
    public Camera npcCamera;
    private GameObject currentPlayer;
    private GameObject currentTarget;
    private float defaultFOV;
    private bool usingNpcCamera = false;
    public List<GameObject> detectedNPCs = new List<GameObject>();
    public Vector3 npcCameraPositionOffset = new Vector3(0f, 2f, -5f);
    public Vector3 npcCameraRotationOffset = new Vector3(20f, 0f, 0f);
    public float cameraTransitionSpeed = 5f;

    void Start()
    {
        defaultFOV = mainCamera.fieldOfView;
        npcCamera.enabled = false;
    }

    void Update()
    {
        if (currentPlayer == null)
        {
            currentPlayer = GameInstance.PlayingCharacterEntity.gameObject;
        }

        // Find closest NPC to player based on tag
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        float closestDistance = Mathf.Infinity;
        foreach (GameObject npc in npcs)
        {
            float distance = Vector3.Distance(currentPlayer.transform.position, npc.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = npc;
            }

            if (!detectedNPCs.Contains(npc) && distance < minDistance)
            {
                detectedNPCs.Add(npc);
            }
            else if (detectedNPCs.Contains(npc) && distance >= minDistance)
            {
                detectedNPCs.Remove(npc);
            }
        }

        // Smoothly switch to NPC camera if close enough
        if (currentTarget != null && closestDistance < minDistance && !usingNpcCamera)
        {
            usingNpcCamera = true;
            mainCamera.enabled = false;
            npcCamera.enabled = true;
        }
        // Smoothly switch back to main camera if too far away
        else if ((currentTarget == null || closestDistance >= minDistance) && usingNpcCamera)
        {
            usingNpcCamera = false;
            mainCamera.enabled = true;
            npcCamera.enabled = false;
        }

        // Smoothly transition camera position and rotation based on NPC position
        if (usingNpcCamera)
        {
            Vector3 targetPosition = currentTarget.transform.position + npcCameraPositionOffset;
            Quaternion targetRotation = Quaternion.Euler(npcCameraRotationOffset);

            npcCamera.transform.position = Vector3.Lerp(npcCamera.transform.position, targetPosition, Time.deltaTime * cameraTransitionSpeed);
            npcCamera.transform.rotation = Quaternion.Lerp(npcCamera.transform.rotation, targetRotation, Time.deltaTime * cameraTransitionSpeed);

            float distanceToNPC = Vector3.Distance(npcCamera.transform.position, currentTarget.transform.position);
            float npcFOV = Mathf.Lerp(60f, 20f, distanceToNPC / minDistance);
            npcCamera.fieldOfView = Mathf.Lerp(npcCamera.fieldOfView, npcFOV, Time.deltaTime * 5f);
        }
        else
        {
            mainCamera.transform.rotation = Quaternion.identity;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFOV, Time.deltaTime * cameraTransitionSpeed);
        }
    }
}
