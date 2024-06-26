using UnityEngine;
using UnityEngine.U2D.Animation; // Required for SpriteSkin

public class FacePlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform

    private SpriteRenderer spriteRenderer; // Reference to the enemy's SpriteRenderer component
    private SpriteSkin spriteSkin; // Reference to the enemy's SpriteSkin component

    private void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Get the SpriteSkin component attached to this GameObject
        spriteSkin = GetComponent<SpriteSkin>();
    }

    private void FixedUpdate()
    {
        // Calculate the direction vector from the enemy to the player
        Vector3 direction = playerTransform.position - transform.position;

        // Calculate the angle the enemy should face based on the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip the sprite and sprite skin based on the sign of the direction's X component
        bool shouldFlip = direction.x < 0f;
        
        
        if (spriteSkin != null)
        {
            Vector3 localScale = spriteSkin.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * (shouldFlip ? -1 : 1);
            spriteSkin.transform.localScale = localScale;
        }
    }
}