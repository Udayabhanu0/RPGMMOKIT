using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPointer : MonoBehaviour
{
    // The buttons that the pointer will move to
    public List<Button> buttons;
    // The particle system to use as the pointer
    public ParticleSystem particleSystem;
    // The speed at which the pointer moves
    public float speed = 1f;
    // The current button index that the pointer is pointing to
    private int currentButtonIndex = 0;

    private void Start()
    {
        // Set the particle system position to the first button position
        particleSystem.transform.position = buttons[currentButtonIndex].transform.position;
    }

    private void Update()
    {
        // Check if the current button has been clicked
        if (buttons[currentButtonIndex].GetComponent<Button>().interactable == false)
        {
            // Move to the next button index
            currentButtonIndex++;

            // Check if we've reached the end of the button list
            if (currentButtonIndex >= buttons.Count)
            {
                // Destroy the particle system
                Destroy(particleSystem.gameObject);
                return;
            }

            // Set the particle system position to the next button position
            particleSystem.transform.position = buttons[currentButtonIndex].transform.position;
        }

        // Move the particle system towards the current button position
        Vector3 direction = buttons[currentButtonIndex].transform.position - particleSystem.transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame)
        {
            // If the pointer has reached the button, snap it to the position
            particleSystem.transform.position = buttons[currentButtonIndex].transform.position;
        }
        else
        {
            // Otherwise, move towards the button
            particleSystem.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }
    }
}
