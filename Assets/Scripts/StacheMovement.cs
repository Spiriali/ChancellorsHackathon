using UnityEngine;
using System.Collections;

public class StacheMovement : MonoBehaviour
{
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
    public float jumpDuration = 2.0f;

    private bool isRotating = false;
    private bool isJumping = false;

    // keep track of the last key hit to differentiate between forward movement and turning
    private char lastKey = ' ';

    void Update()
    {
        // prevent holding down key
        if (isRotating)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move('A');
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move('D');
        }
        // prevent double jumping
        else if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(Jump());
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

            yield return null;
        }

        // ensure exact landing position
        transform.position = startPos + (forwardDirection * jumpForwardDistance);

        isJumping = false;
    }
}
