using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera should follow (your player).
    public Transform target;

    // How quickly the camera catches up to the target.
    // A smaller value will be a slower, smoother follow.
    public float smoothSpeed = 0.125f;

    // The offset distance from the target (e.g., for a 2D game, Z should be -10).
    public Vector3 offset;

    // LateUpdate is called after all Update functions have been called.
    // This is the best place for camera logic, as it ensures the target has finished moving for the frame.
    void LateUpdate()
    {
        // Check if a target has been assigned.
        if (target != null)
        {
            // Calculate the desired position for the camera.
            Vector3 desiredPosition = target.position + offset;
            
            // Smoothly interpolate from the camera's current position to the desired position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Apply the new position to the camera.
            transform.position = smoothedPosition;
        }
    }
}