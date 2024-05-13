using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform

    private SpriteRenderer spriteRenderer; // Reference to the enemy's SpriteRenderer component

    private void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Calculate the direction vector from the enemy to the player
        Vector3 direction = playerTransform.position - transform.position;

        // Calculate the angle the enemy should face based on the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip the sprite based on the sign of the direction's X component
        if (direction.x >= 0f)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else
        {
            spriteRenderer.flipX = true; // Face left
        }
    }
}