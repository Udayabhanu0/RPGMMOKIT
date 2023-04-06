using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : MonoBehaviour
{
    public ParticleSystem[] windParticles;
    public Camera mainCamera;
    public float spawnInterval = 5f;
    public float spawnDistance = 10f;

    private float timer = 0f;

    private void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a wind particle
        if (timer >= spawnInterval)
        {
            // Reset timer
            timer = 0f;

            // Get a random wind particle from the list
            ParticleSystem wind = windParticles[Random.Range(0, windParticles.Length)];

            // Spawn the wind particle in a random position in front of the camera
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;
            spawnPosition += new Vector3(Random.Range(-5f, 5f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            Instantiate(wind, spawnPosition, Quaternion.identity);
        }
    }
}
