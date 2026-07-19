using UnityEngine;

public class RotateBackAndForth : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The axis to rotate around (e.g., Vector3.up for Y-axis)")]
    public Vector3 rotationAxis = Vector3.up;
    
    [Tooltip("Speed of the rotation in degrees per second")]
    public float speed = 45f;
    
    [Tooltip("Maximum angle in degrees from the starting rotation")]
    public float maxAngle = 45f;

    private Quaternion startRotation;

    void Start()
    {
        // Store the initial rotation of the object
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate the oscillating angle using a sine wave
        float angle = Mathf.Sin(Time.time * speed * Mathf.Deg2Rad) * maxAngle;
        
        // Apply the rotation relative to the starting rotation
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(angle, rotationAxis);
        transform.rotation = targetRotation;
    }
}
