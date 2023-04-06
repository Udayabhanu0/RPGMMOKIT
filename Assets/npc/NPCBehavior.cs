using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    public List<GameObject> Destinations;
    public List<AnimationClip> PlayableAnimations;
    public float AnimationDelay = 1f;
    public float DestinationTime = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentDestinationIndex = 0;
    private int currentAnimationIndex = 0;
    private bool isWalking = false;
    private float timeAtDestination = 0f;
    private bool hasReachedDestination = false;
    private bool isWaiting = false;
    private float animationTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.Play(PlayableAnimations[currentAnimationIndex].name);
    }

    void Update()
    {
        if (!hasReachedDestination)
        {
            // Move the NPC towards the current destination
            agent.SetDestination(Destinations[currentDestinationIndex].transform.position);

            // Check if the NPC has reached the current destination
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                hasReachedDestination = true;
                timeAtDestination = 0f;
                isWaiting = true;
                isWalking = false;
                animator.SetBool("IsWalking", false);
            }
        }
        else
        {
            // Update the timer for how long the NPC stays at the destination
            timeAtDestination += Time.deltaTime;

            // Check if the NPC has been at the destination for long enough
            if (timeAtDestination >= DestinationTime)
            {
                hasReachedDestination = false;
                isWaiting = false;
                isWalking = true;
                animator.SetBool("IsWalking", true);

                // Move to the next destination
                currentDestinationIndex = (currentDestinationIndex + 1) % Destinations.Count;

                // Play the next animation clip
                currentAnimationIndex = (currentAnimationIndex + 1) % PlayableAnimations.Count;
                animator.Play(PlayableAnimations[currentAnimationIndex].name);

                // Reset the animation timer
                animationTimer = 0f;
            }
        }

        // Check if the NPC should play another animation clip
        if (isWalking && !isWaiting)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= AnimationDelay)
            {
                currentAnimationIndex = (currentAnimationIndex + 1) % PlayableAnimations.Count;
                animator.Play(PlayableAnimations[currentAnimationIndex].name);
                animationTimer = 0f;
            }
        }
    }
}
