using UnityEngine;
public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform pivot; // the empty GameObject above the player
    [SerializeField] private float rotationSpeed = 1000.0f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private int framesToSkip = 5;

    private float pitch = 0f;
    private float yaw = 0f;
    private float distance = 0f;
    private int frameCount = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize yaw/pitch/distance from current camera position relative to pivot
        Vector3 offset = transform.position - pivot.position;
        distance = offset.magnitude;

        Vector3 angles = Quaternion.LookRotation(-offset.normalized).eulerAngles;
        pitch = angles.x;
        if (pitch > 180f) pitch -= 360f;
        yaw = angles.y;
    }

    void Update()
    {
        if (frameCount < framesToSkip)
        {
            frameCount++;
            return;
        }
        CamOrbit();
    }

    private void CamOrbit()
    {
        float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        pitch -= verticalInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        yaw += horizontalInput;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 direction = rotation * new Vector3(0f, 0f, -distance);

        transform.position = pivot.position + direction;
        transform.LookAt(pivot.position);
    }
}