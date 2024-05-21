using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public ParticleSystem particleSystem; // Reference to the particle system

    private ParticleSystem.ShapeModule shapeModule; // Reference to the particle system's shape module

    private void Start()
    {
        // Get the particle system's shape module
        shapeModule = particleSystem.shape;
    }

    private void LateUpdate()
    {
        // Update the particle system's position to match the player's position
        particleSystem.transform.position = playerTransform.position;

        // Update the particle system's shape to follow the player's rotation
        shapeModule.rotation = playerTransform.rotation.eulerAngles;
    }
}
