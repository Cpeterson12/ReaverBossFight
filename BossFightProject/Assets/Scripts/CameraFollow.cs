using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public FloatData smoothSpeed; // Adjust this value to change the smoothness of the camera movement
    public Vector3 offset; // Offset position for the camera relative to the target

    private void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);

        // Smoothly interpolate the camera's position towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed.data);
        transform.position = smoothedPosition;
    }
}
