using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1000.0f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private int framesToSkip = 5;

    private float pitch = 0f;
    private int frameCount = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pitch = transform.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
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

        pitch -= verticalInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 currentAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(pitch, currentAngles.y, currentAngles.z);
    }
}