using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PathGuidingSystem : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;  // Reference to the particle system prefab
    public Transform playerTransform;  // Reference to the player's transform
    public float moveSpeed = 5f;  // The speed at which the particles move along the path

    public List<Transform> destinationObjects;  // List of destination objects for the particle system to follow
    private int currentDestinationIndex = 0;  // Index of the current destination object in the list

    public Button button;  // Reference to the UI button

    private ParticleSystem particleSystemInstance;  // Reference to the spawned particle system instance
    private NavMeshAgent navMeshAgent;  // Reference to the NavMeshAgent component of the particle system instance

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);  // Register the OnButtonClick method to the button's click event
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);  // Unregister the OnButtonClick method from the button's click event when this script is destroyed
    }

    private void Update()
    {
        // If the particle system instance is active, check if it has reached the current destination
        if (particleSystemInstance != null && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // If the particle system has reached the current destination, move to the next destination
            currentDestinationIndex++;

            // If there are no more destinations, destroy the particle system and reset the current destination index
            if (currentDestinationIndex >= destinationObjects.Count)
            {
                Destroy(particleSystemInstance.gameObject);
                particleSystemInstance = null;
                currentDestinationIndex = 0;
                return;
            }

            // Set the NavMeshAgent's destination to the next destination object's position
            navMeshAgent.SetDestination(destinationObjects[currentDestinationIndex].position);
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("hiii");
        // When the button is clicked, instantiate a new particle system at the player's position
        particleSystemInstance = Instantiate(particleSystemPrefab, playerTransform.position, Quaternion.identity);

        // Get the NavMeshAgent component of the particle system instance
        navMeshAgent = particleSystemInstance.GetComponent<NavMeshAgent>();

        // Set the NavMeshAgent's speed
        navMeshAgent.speed = moveSpeed;

        // Set the NavMeshAgent's destination to the first destination object's position
        navMeshAgent.SetDestination(destinationObjects[currentDestinationIndex].position);
    }
}
