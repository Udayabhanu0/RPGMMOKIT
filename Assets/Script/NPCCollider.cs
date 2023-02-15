using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCollider : MonoBehaviour
{
    public delegate void NpcColliderCheckEvent(bool isColliding);

    public static event NpcColliderCheckEvent CollisionChecked;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checked");
        Debug.Log(other.gameObject.tag);
        int layer = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        Debug.Log(layerName);

        if (layerName == "Ignore Raycast")
        {
        Debug.Log("Hi");
        CollisionChecked?.Invoke(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        CollisionChecked?.Invoke(false);
        if (other.gameObject.CompareTag("PlayerTag"))
        {
            Debug.Log("Hiexit");
        }
    }
}
