using System.Collections;
using UnityEngine;

public class SingleSpikeTrap : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The local direction and distance the spike will move when popping up")]
    public Vector3 popUpOffset = new Vector3(0f, 2f, 0f);
    [Tooltip("How fast the spike moves up and down")]
    public float movementSpeed = 10f;

    [Header("Color Settings")]
    [Tooltip("The warning color the spike changes to before popping up")]
    public Color warningColor = Color.red;

    [Header("Timing Settings")]
    [Tooltip("Time spent warning (changing color) before moving")]
    public float warningDuration = 1.0f;
    [Tooltip("How long the spike stays fully extended at the peak")]
    public float activeDuration = 1.5f;

  [Header("Launch Angle Settings")]
[Tooltip("How much upward force is applied")]
public float throwForce = 15f;

[Range(0f, 90f)]
[Tooltip("Angle of launch: 90 is straight up, 45 is a perfect arc forward")]
public float launchAngleDegrees = 65f;


    private Vector3 retractPosition;
    private Vector3 popUpPosition;
    private Renderer spikeRenderer;
    private Color originalColor;

    private bool isDangerous; 

    void Awake()
    {

        GameObject parentObj = transform.parent.gameObject;
        // Store positions
        retractPosition = parentObj.transform.localPosition;
        popUpPosition = retractPosition + popUpOffset;

        // Cache renderer and original color
        spikeRenderer = GetComponent<Renderer>();
        

        // Fire the sequence exactly once
    }

    void OnEnable(){
        if (spikeRenderer != null)
        {
            originalColor = spikeRenderer.material.color;
        }
        else
        {
            originalColor = Color.white;
        }

        StartCoroutine(SingleTrapRoutine()); 
    }

    IEnumerator SingleTrapRoutine()
    {
        // 1. WARNING PHASE: Change to warning color
        SetSpikeColor(warningColor);
        yield return new WaitForSeconds(warningDuration);

        // 2. POP UP PHASE: Smoothly move to peak position
        yield return StartCoroutine(MoveSpike(popUpPosition));
        isDangerous = true; 

        // 3. ACTIVE PHASE: Hold position at the peak
        yield return new WaitForSeconds(activeDuration);

        isDangerous = false; 
        // 4. RETRACT PHASE: Smoothly move back down
        yield return StartCoroutine(MoveSpike(retractPosition));

        // 5. RESET PHASE: Restore original color
        SetSpikeColor(originalColor);

        // 6. DISABLE SCRIPT: Turns off the script component permanently
        enabled = false;
        Debug.Log($"Spike trap sequence complete. Script on {gameObject.name} disabled.");
    }

    // Helper coroutine to smoothly move the single spike
    IEnumerator MoveSpike(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.parent.localPosition, targetPosition) > 0.001f)
        {
            transform.parent.localPosition = Vector3.MoveTowards(
                transform.parent.localPosition, 
                targetPosition, 
                movementSpeed * Time.deltaTime
            );
            yield return null;
        }
        
        // Snap directly to target position to prevent floating point inaccuracies
        transform.parent.localPosition = targetPosition;
    }

    // Helper to change the color safely
    void SetSpikeColor(Color color)
    {
        if (spikeRenderer != null)
        {
            spikeRenderer.material.color = color;
        }
    }

void OnTriggerEnter(Collider collision)
{
    // Only launch if the spike is actively dangerous
    //if (!isDangerous) return;

    if (collision.gameObject.CompareTag("Player"))
    {
        Debug.Log("Stache Triggered");

        Rigidbody mustacheRb = collision.gameObject.GetComponent<Rigidbody>();
        
        if (mustacheRb != null)
        {
              Collider playerCollider = collision.gameObject.GetComponent<Collider>();
        
        if (playerCollider != null)
        {
            playerCollider.enabled = false; // Turns off the collider
            Debug.Log("Player collider disabled on impact!");
        }
            
            // 1. Calculate the launch direction based on the spike's own facing direction
            Vector3 forwardDir = transform.forward; 
            Vector3 upDir = Vector3.up;

            // Convert your inspector angle into radians for math calculations
            float angleInRadians = launchAngleDegrees * Mathf.Deg2Rad;

            // Mix the forward and upward vectors together to get the exact angle
            Vector3 launchDirection = (forwardDir * Mathf.Cos(angleInRadians)) + (upDir * Mathf.Sin(angleInRadians));
            launchDirection.Normalize(); // Keep the vector clean and uniform

            // 2. Clear current velocity so physics don't compound wildly
            mustacheRb.linearVelocity = Vector3.zero;
            
            // 3. Apply the force along your new angled vector
            mustacheRb.AddForce(launchDirection * throwForce, ForceMode.Impulse);
            
            Debug.Log($"Mustache launched at a {launchAngleDegrees} degree angle!");

            StartCoroutine(Delay());

            playerCollider.enabled = true;
            
        }
    }
}

IEnumerator Delay(){
    yield return new WaitForSeconds(0.5f);
}


}
