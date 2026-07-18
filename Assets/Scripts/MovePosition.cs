using UnityEngine;

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
}
