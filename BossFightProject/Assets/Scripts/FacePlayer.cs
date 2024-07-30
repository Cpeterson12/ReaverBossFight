using UnityEngine;
using UnityEngine.U2D.Animation; 

public class FacePlayer : MonoBehaviour
{
    public Transform playerTransform; 

    public SpriteRenderer spriteRenderer; 
    public SpriteSkin spriteSkin;
    private bool isTracking = true;

    private void Start()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();

       // spriteSkin = GetComponent<SpriteSkin>();
    }
    public void EnableTracking(bool enable)
    {
        Debug.Log("BOOL");
        isTracking = enable;
    }

    private void FixedUpdate()
    {
        if (isTracking)
        {
            Vector3 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bool shouldFlip = direction.x < 0f;
            
            if (spriteSkin != null)
            {
                Vector3 localScale = spriteSkin.transform.localScale;
                localScale.x = Mathf.Abs(localScale.x) * (shouldFlip ? -1 : 1);
                spriteSkin.transform.localScale = localScale;
            }
        }


    }
}