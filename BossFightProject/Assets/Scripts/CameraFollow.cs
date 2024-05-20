using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Adjust this value to change the smoothness of the camera movement
    public Vector3 offset; // Offset position for the camera relative to the target

    public FloatData leftLimit; 
    public FloatData rightLimit;

    private void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);

        // Clamp the camera's X position within the specified limits
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftLimit.data, rightLimit.data);

        // Smoothly interpolate the camera's position towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
