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
    
    // degrees of mustache rotation per turn stroke
    public float turnAnglePerStroke = 0.5f;
    public float rotationSpeed = 300.0f;
    public float moveSpeed = 5.0f;

    [Header("Jump settings")]
    public float jumpHeight = 1.5f;

    private bool isJumping = false;


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
        else if (Input.GetKey(KeyCode.W))
        {
            Move();
        }
    }

    void Move(char key)
    {

        if (key == 'D')
        {
            // pivot on the right tip
            StartCoroutine(RotateAroundPivot(rightTip, turnAnglePerStroke));
        }
        else
        {
            // pivot on the left tip
            StartCoroutine(RotateAroundPivot(leftTip, -turnAnglePerStroke));

        }
    }

    IEnumerator RotateAroundPivot(Transform pivotTransform, float totalAngle)
    {
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

    }

    void Jump()
    {
        isJumping = true;
        float g = Mathf.Abs(Physics.gravity.y);
        // vertical speed needed to reach jumpHeight at the peak of the arc
        float vY = Mathf.Sqrt(2f * g * jumpHeight);

        // only override the Y component, keep whatever horizontal velocity already exists
        Vector3 velocity = rb.linearVelocity;
        velocity.y = vY;
        rb.linearVelocity = velocity;
    }

    void Move()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 moveVelocity = forwardDirection * moveSpeed;

        // only override X/Z, preserve current vertical velocity (falling/jumping)
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveVelocity.x;
        velocity.z = moveVelocity.z;
        rb.linearVelocity = velocity;
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
