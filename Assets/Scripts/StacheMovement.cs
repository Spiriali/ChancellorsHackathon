using UnityEngine;
using System.Collections;

public class StacheMovement : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Project Settings > Physics > CCD
        //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    // get right and left mustache tips for rotation
    [Header("Pivot Points")]
    public Transform leftTip;
    public Transform rightTip;

   
    [Header("Movement Settings")]

    // degrees of mustache rotation per forward stroke
    public float walkAnglePerStroke = 50.0f;
    // degrees of mustache rotation per turn stroke (to allow smaller turns)
    public float turnAnglePerStroke = 25.0f;
    public float rotationSpeed = 300.0f;

    [Header("Jump settings")]
    public float jumpHeight = 1.5f;
    public float jumpForwardDistance = 5.0f;

    private bool isRotating = false;
    private bool isJumping = false;

    // keep track of the last key hit to differentiate between forward movement and turning
    private char lastKey = ' ';

    void Update()
    {
        

        if (Input.GetKey(KeyCode.A))
        {
            Move('A');
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move('D');
        }
        // prevent double jumping
        else if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            Jump();
        }
    }

    void Move(char key)
    {
        // check if the player is trying to turn
        bool isTurning = (key == lastKey);

        // set appropriate angle
        float angle = isTurning ? turnAnglePerStroke : walkAnglePerStroke;

        // update last key
        lastKey = key;

        if (key == 'D')
        {
            // pivot on the right tip
            StartCoroutine(RotateAroundPivot(rightTip, angle));
        }
        else
        {
            // pivot on the left tip
            StartCoroutine(RotateAroundPivot(leftTip, -angle));

        }
    }

    IEnumerator RotateAroundPivot(Transform pivotTransform, float totalAngle)
    {
        isRotating = true;
        float rotated = 0.0f;
        float sign = Mathf.Sign(totalAngle);
        float absTotal = Mathf.Abs(totalAngle);

        while (rotated < absTotal)
        {
            // update the pivot's world position
            Vector3 pivot = pivotTransform.position;

            float step = rotationSpeed * Time.deltaTime;
            step = Mathf.Min(step, absTotal - rotated);
            transform.RotateAround(pivot, Vector3.up, sign * step);

            rotated += step;
            yield return null;
        }

        isRotating = false;
    }

    void Jump()
    {
        isJumping = true;

        float g = Mathf.Abs(Physics.gravity.y);

        // vertical speed needed to reach jumpHeight at the peak of the arc
        float vY = Mathf.Sqrt(2f * g * jumpHeight);

        // total time spent in the air, from launch back down to the same height
        float airTime = 2f * vY / g;

        // forward speed needed to cover jumpForwardDistance in that time
        float vForward = jumpForwardDistance / airTime;

        Vector3 forwardDirection = transform.forward;
        Vector3 launchVelocity = forwardDirection * vForward + Vector3.up * vY;
     
        rb.linearVelocity = launchVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // only clear isJumping once we hit something roughly beneath us to prevent wall jumping
        if (!isJumping) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isJumping = false;
                break;
            }
        }
    }

    /*
    IEnumerator Jump()
    {
        isJumping = true;

        Vector3 startPos = transform.position;

        // forward is the direction the mustache is facing
        Vector3 forwardDirection = transform.forward;

        float time = 0.0f;

        while(time < jumpDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / jumpDuration);

            // parabolic jump
            float heightOffset = 4.0f * jumpHeight * t * (1.0f - t);
            float forwardOffset = jumpForwardDistance * t;

            transform.position = startPos + (forwardDirection * forwardOffset) + (Vector3.up * heightOffset);

            yield return new WaitForFixedUpdate();
        }

        isJumping = false;
    }
    */
}
