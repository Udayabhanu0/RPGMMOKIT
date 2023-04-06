using UnityEngine;
using Cysharp.Threading.Tasks;
using MultiplayerARPG;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float closestDistance = 3.0f;
    [SerializeField] private LayerMask npcLayer;
    private Vector3 _currentVelocity = Vector3.zero;
    private Vector3 _offset;
    private Transform _target;
    private bool _isMoving;

    private async void Update()
    {
        if(playerTransform == null)
        {
            playerTransform = GameInstance.PlayingCharacterEntity.transform;
        }
        // Get all game objects with the "NpcTag" tag
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NpcTag");

        // Calculate the closest NPC to the player
        float minDistance = Mathf.Infinity;
        foreach (GameObject npc in npcs)
        {
            float dist = Vector3.Distance(playerTransform.position, npc.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                _target = npc.transform;
            }
        }

        // Move the camera closer to the closest NPC if it's within range
        if (minDistance < distance && !_isMoving)
        {
            _isMoving = true;
            _offset = transform.position - _target.position + Vector3.up * closestDistance;
            await UniTask.WaitWhile(() => _isMoving);
        }

        // Move the camera back to the player if the closest NPC is out of range
        if (minDistance >= distance && _isMoving)
        {
            _isMoving = false;
            _offset = transform.position - playerTransform.position;
            await UniTask.WaitWhile(() => _isMoving);
        }
    }

    private void LateUpdate()
    {
        if (_isMoving)
        {
            Vector3 targetPosition = _target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
        else
        {
            Vector3 targetPosition = playerTransform.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NpcTag"))
        {
            _isMoving = true;
            _target = other.transform;
            _offset = transform.position - _target.position + Vector3.up * closestDistance;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NpcTag"))
        {
            _isMoving = false;
            _target = null;
            _offset = transform.position - playerTransform.position;
        }
    }
}
