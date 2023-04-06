using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] destinations; // List of destinations for the NPC to walk to
    public Animator animator; // Animator component for playing animations
    public string walkAnimationVariableName; // Name of the walk animation variable in the animator
    public string[] destinationAnimationVariableNames; // Names of the destination animation variables in the animator
    public float destinationAnimationDuration = 10f; // Duration of destination animation in seconds

    private int currentDestinationIndex = 0; // Index of the current destination
    private NavMeshAgent agent; // NavMeshAgent component for navigating the NPC
    private bool isWalking = false; // Whether the NPC is currently walking or not
    private bool isDestinationAnimating = false; // Whether the destination animation is currently playing or not
    private float destinationAnimationStartTime = 0f; // Start time of the current destination animation

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            if (agent.remainingDistance < 0.5f)
            {
                isWalking = false;
                animator.SetBool(walkAnimationVariableName, false);
                PlayDestinationAnimation();
            }
        }
        else if (isDestinationAnimating)
        {
            if (Time.time - destinationAnimationStartTime >= destinationAnimationDuration)
            {
                isDestinationAnimating = false;
                animator.SetBool(destinationAnimationVariableNames[currentDestinationIndex], false);
                GoToNextDestination();
            }
        }
    }

    // Go to the next destination in the list
    void GoToNextDestination()
    {
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
        agent.SetDestination(destinations[currentDestinationIndex].position);
        isWalking = true;
        animator.SetBool(walkAnimationVariableName, true);
    }

    // Play the destination animation for the current destination
    void PlayDestinationAnimation()
    {
        if (!isDestinationAnimating)
        {
            isDestinationAnimating = true;
            animator.SetBool(destinationAnimationVariableNames[currentDestinationIndex], true);
            destinationAnimationStartTime = Time.time;
        }
    }
}
