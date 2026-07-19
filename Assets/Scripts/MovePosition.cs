/*using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform pathParent; // Drag the 'Path' parent object here
    public float speed = 5f;
    public bool loopPath = true;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Automatically populate the waypoints array using the parent's children
        int childCount = pathParent.childCount;
        waypoints = new Transform[childCount];
        
        for (int i = 0; i < childCount; i++)
        {
            waypoints[i] = pathParent.GetChild(i);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Target the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Move the object smoothly towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Check if the object has closely reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            // Handle path looping or ending
            if (currentWaypointIndex >= waypoints.Length)
            {
                if (loopPath)
                {
                    currentWaypointIndex = 0; // Restart path
                }
                else
                {
                    enabled = false; // Stop updating script
                }
            }
        }
    }
}*/

using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public enum MoveAxis { X, Y, Z }

    [Header("Movement Settings")]
    [Tooltip("The local axis to move along")]
    public MoveAxis selectedAxis = MoveAxis.X;

    [Tooltip("Speed of the movement loop")]
    public float speed = 2f;

    [Tooltip("Total distance the object will travel from the center to one side")]
    public float distance = 3f;

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate the ping-pong offset using a sine wave instantly using Time.time
        float offset = Mathf.Sin(Time.time * speed) * distance;

        // Determine the movement vector based on the selected axis
        Vector3 movementVector = Vector3.zero;
        switch (selectedAxis)
        {
            case MoveAxis.X:
                movementVector = new Vector3(offset, 0f, 0f);
                break;
            case MoveAxis.Y:
                movementVector = new Vector3(0f, offset, 0f);
                break;
            case MoveAxis.Z:
                movementVector = new Vector3(0f, 0f, offset);
                break;
        }

        // Apply the movement relative to the starting position
        transform.localPosition = startPosition + movementVector;
    }
}

