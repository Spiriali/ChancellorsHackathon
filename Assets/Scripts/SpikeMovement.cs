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

    private Vector3 retractPosition;
    private Vector3 popUpPosition;
    private Renderer spikeRenderer;
    private Color originalColor;

    void Awake()
    {
        // Store positions
        retractPosition = transform.localPosition;
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

        // 3. ACTIVE PHASE: Hold position at the peak
        yield return new WaitForSeconds(activeDuration);

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
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, 
                targetPosition, 
                movementSpeed * Time.deltaTime
            );
            yield return null;
        }
        
        // Snap directly to target position to prevent floating point inaccuracies
        transform.localPosition = targetPosition;
    }

    // Helper to change the color safely
    void SetSpikeColor(Color color)
    {
        if (spikeRenderer != null)
        {
            spikeRenderer.material.color = color;
        }
    }
}
