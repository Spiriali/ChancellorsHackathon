using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    [Header("Spike References")]
    [Tooltip("Add all the individual spike GameObjects to this list")]
    //public List<List<GameObject>> formations = new List<List<GameObject>>();
    public List<ItemRow> formations = new List<ItemRow>();

    [Header("Movement Settings")]
    [Tooltip("The local direction and distance the spikes will move when popping up")]
    public Vector3 popUpOffset = new Vector3(0f, 2f, 0f);
    [Tooltip("How fast the spikes move up and down")]
    public float movementSpeed = 10f;

    [Header("Color Settings")]
    [Tooltip("The warning color the spikes change to before popping up")]
    public Color warningColor = Color.red;

    [Header("Timing Settings")]
    [Tooltip("Time spent warning (changing color) before moving")]
    public float warningDuration = 1.0f;
    [Tooltip("How long the spikes stay fully extended at the peak")]
    public float activeDuration = 1.5f;
    [Tooltip("Time to wait between the end of one cycle and the start of the next")]
    public float cooldownDuration = 2.0f;

    // Internal tracking structures
    private List<Vector3> retractPositions = new List<Vector3>();
    private List<Vector3> popUpPositions = new List<Vector3>();
    private List<Renderer> spikeRenderers = new List<Renderer>();
    private List<Color> originalColors = new List<Color>();

    private int index = 0; 
    private List<GameObject> spikes = new List<GameObject>(); 

    void Start()
    {

        // Start the infinite trap loop
        StartCoroutine(ChooseFormation());
    }

    /*IEnumerator ChooseFormation(){
        
        while (true){
             index = Random.Range(0, formations.Count); 
            Debug.Log(index);
            spikes = formations[index].items;

            foreach (GameObject spike in spikes){
                if (spike != null){
                    SingleSpikeTrap script = spike.GetComponent<SingleSpikeTrap>();
                    script.enabled = true; 
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }*/

    IEnumerator ChooseFormation()
{
    while (true)
    {
        index = Random.Range(0, formations.Count);
        Debug.Log(index);
        spikes = formations[index].items;

        // 1. Fire off all spikes in this formation
        foreach (GameObject spike in spikes)
        {
            if (spike != null)
            {
                SingleSpikeTrap script = spike.GetComponentInChildren<SingleSpikeTrap>();
                if (script != null) script.enabled = true;
            }
        }

        // 2. STICKY WAIT: Dynamically wait until every single spike in this formation is done
        bool spikesAreStillActive = true;
        while (spikesAreStillActive)
        {
            spikesAreStillActive = false; // Assume they are done
            foreach (GameObject spike in spikes)
            {
                if (spike != null && spike.GetComponentInChildren<SingleSpikeTrap>().enabled)
                {
                    spikesAreStillActive = true; // Found one still running!
                    break;
                }
            }
            yield return null; // Wait one frame before checking again
        }

        // 3. Spikes are safe in the ground! Now apply your 3-second delay between formations
        yield return new WaitForSeconds(3f);
    }
}


        /*foreach (GameObject spike in spikes)
        {
            if (spike != null)
            {
                Vector3 startPos = spike.transform.localPosition;
                retractPositions.Add(startPos);
                popUpPositions.Add(startPos + popUpOffset);

                Renderer ren = spike.GetComponent<Renderer>();
                spikeRenderers.Add(ren);

                if (ren != null)
                {
                    // Store original material color (handles standard materials)
                    originalColors.Add(ren.material.color);
                }
                else
                {
                    originalColors.Add(Color.white);
                }
            }
        }
    }

    IEnumerator TrapCycleRoutine()
    {
        while (true){

            // 1. WARNING PHASE: Change to the warning color
            SetSpikeColors(warningColor);
            yield return new WaitForSeconds(warningDuration);

            // 2. POP UP PHASE: Move smoothly to peak positions
            yield return StartCoroutine(MoveSpikes(popUpPositions));

            // 3. ACTIVE PHASE: Wait at the peak
            yield return new WaitForSeconds(activeDuration);

            // 4. RETRACT PHASE: Move smoothly back down
            yield return StartCoroutine(MoveSpikes(retractPositions));

            // 5. RESET PHASE: Return to original colors
            ResetSpikeColors();

            // 6. COOLDOWN PHASE: Wait before starting the cycle all over again
            yield return new WaitForSeconds(cooldownDuration);

            setFormation(); 

        }
    }

   

    // Helper coroutine to smoothly interpolate positions
    IEnumerator MoveSpikes(List<Vector3> targetPositions)
    {
        bool allReached = false;
        while (!allReached)
        {
            allReached = true;
            for (int i = 0; i < spikes.Count; i++)
            {
                if (spikes[i] == null) continue;

                Transform t = spikes[i].transform;
                Vector3 target = targetPositions[i];

                t.localPosition = Vector3.MoveTowards(t.localPosition, target, movementSpeed * Time.deltaTime);

                if (Vector3.Distance(t.localPosition, target) > 0.001f)
                {
                    allReached = false;
                }
            }
            yield return null;
        }
    }

    // Helper to apply a uniform color
    void SetSpikeColors(Color color)
    {
        for (int i = 0; i < spikeRenderers.Count; i++)
            if (spikeRenderers[i] != null)
                spikeRenderers[i].material.color = color;
    }

    // Helper to restore cached colors
    void ResetSpikeColors()
    {
        for (int i = 0; i < spikeRenderers.Count; i++)
            if (spikeRenderers[i] != null)
                spikeRenderers[i].material.color = originalColors[i];
    }

     void setFormation(){

        retractPositions.Clear();
        popUpPositions.Clear();
        spikeRenderers.Clear();
        originalColors.Clear();
        
            index = Random.Range(0, formations.Count); 
            Debug.Log(index);
            spikes = formations[index].items;

        foreach (GameObject spike in spikes)
        {
            if (spike != null)
            {
                Vector3 startPos = spike.transform.localPosition;
                retractPositions.Add(startPos);
                popUpPositions.Add(startPos + popUpOffset);

                Renderer ren = spike.GetComponent<Renderer>();
                spikeRenderers.Add(ren);

                if (ren != null)
                {
                    // Store original material color (handles standard materials)
                    originalColors.Add(ren.material.color);
                }
                else
                {
                    originalColors.Add(Color.white);
                }
            }
        }
            StartCoroutine(TrapCycleRoutine());
    }*/
}

[System.Serializable]
public class ItemRow
{
    public List<GameObject> items;
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour 
{
    [Header("Spike References")]
    [Tooltip("Add all the individual spike GameObjects to this list")]
    public List<ItemRow> formations = new List<ItemRow>();

    [Header("Movement Settings")]
    [Tooltip("The local direction and distance the spikes will move when popping up")]
    public Vector3 popUpOffset = new Vector3(0f, 2f, 0f);
    [Tooltip("How fast the spikes move up and down")]
    public float movementSpeed = 10f;

    [Header("Color Settings")]
    [Tooltip("The warning color the spikes change to before popping up")]
    public Color warningColor = Color.red;

    [Header("Timing Settings")]
    [Tooltip("Time spent warning (changing color) before moving")]
    public float warningDuration = 1.0f;
    [Tooltip("How long the spikes stay fully extended at the peak")]
    public float activeDuration = 1.5f;
    [Tooltip("Time to wait between the end of one cycle and the start of the next")]
    public float cooldownDuration = 2.0f;

    // Internal tracking structures
    private List<Vector3> retractPositions = new List<Vector3>();
    private List<Vector3> popUpPositions = new List<Vector3>();
    private List<Renderer> spikeRenderers = new List<Renderer>();
    private List<Color> originalColors = new List<Color>();
    private List<GameObject> spikes = new List<GameObject>();

    void Start() 
    {
        // Safety check: ensure we actually have formations set up in the inspector
        if (formations == null || formations.Count == 0)
        {
            Debug.LogError("No formations assigned to SpikeTrapController!", this);
            return;
        }

        // Start the infinite trap loop safely from Start
        StartCoroutine(TrapCycleRoutine());
    }

    IEnumerator TrapCycleRoutine() 
    {
        // while(true) keeps the coroutine running forever without relying on Update()
        while (true)
        {
            // 0. PREPARATION: Pick a new formation and setup references
            SetupCurrentFormation();

            if (spikes.Count > 0)
            {
                // 1. WARNING PHASE: Change to the warning color
                SetSpikeColors(warningColor);
                yield return new WaitForSeconds(warningDuration);

                // 2. POP UP PHASE: Move smoothly to peak positions
                yield return StartCoroutine(MoveSpikes(popUpPositions));

                // 3. ACTIVE PHASE: Wait at the peak
                yield return new WaitForSeconds(activeDuration);

                // 4. RETRACT PHASE: Move smoothly back down
                yield return StartCoroutine(MoveSpikes(retractPositions));

                // 5. RESET PHASE: Return to original colors
                ResetSpikeColors();
            }

            // 6. COOLDOWN PHASE: Wait before starting the cycle all over again
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    // Helper method to completely reset and populate caching lists for the new formation
    void SetupCurrentFormation()
    {
        // Crucial: Clear out the old cached data from the previous formation cycle!
        retractPositions.Clear();
        popUpPositions.Clear();
        spikeRenderers.Clear();
        originalColors.Clear();

        int index = Random.Range(0, formations.Count);
        spikes = formations[index].items;
        Debug.Log(spikes);

        if (spikes == null) return;

        foreach (GameObject spike in spikes) 
        {
            if (spike != null) 
            {
                Vector3 startPos = spike.transform.localPosition;
                retractPositions.Add(startPos);
                popUpPositions.Add(startPos + popUpOffset);
                
                Renderer ren = spike.GetComponent<Renderer>();
                spikeRenderers.Add(ren);
                
                if (ren != null) 
                {
                    originalColors.Add(ren.material.color);
                } 
                else 
                {
                    originalColors.Add(Color.white);
                }
            }
        }
    }

    // Helper coroutine to smoothly interpolate positions
    IEnumerator MoveSpikes(List<Vector3> targetPositions) 
    {
        bool allReached = false;
        while (!allReached) 
        {
            allReached = true;
            for (int i = 0; i < spikes.Count; i++) 
            {
                // Safety check for destroyed spike items
                if (spikes[i] == null || i >= targetPositions.Count) continue;
                
                Transform t = spikes[i].transform;
                Vector3 target = targetPositions[i];
                t.localPosition = Vector3.MoveTowards(t.localPosition, target, movementSpeed * Time.deltaTime);
                
                if (Vector3.Distance(t.localPosition, target) > 0.001f) 
                {
                    allReached = false;
                }
            }
            yield return null;
        }
    }

    // Helper to apply a uniform color
    void SetSpikeColors(Color color) 
    {
        for (int i = 0; i < spikeRenderers.Count; i++) 
        {
            if (spikeRenderers[i] != null) 
            {
                spikeRenderers[i].material.color = color;
            }
        }
    }

    // Helper to restore cached colors
    void ResetSpikeColors() 
    {
        for (int i = 0; i < spikeRenderers.Count; i++) 
        {
            if (spikeRenderers[i] != null && i < originalColors.Count) 
            {
                spikeRenderers[i].material.color = originalColors[i];
            }
        }
    }
}

[System.Serializable]
public class ItemRow
{
    public List<GameObject> items;
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    [Header("Spike References")]
    [Tooltip("Add all the individual spike GameObjects to this list")]
    public List<GameObject> spikes = new List<GameObject>();

    [Header("Movement Settings")]
    [Tooltip("The local direction and distance the spikes will move when popping up")]
    public Vector3 popUpOffset = new Vector3(0f, 2f, 0f);
    [Tooltip("How fast the spikes move up and down")]
    public float movementSpeed = 10f;

    [Header("Color Settings")]
    [Tooltip("The warning color the spikes change to before popping up")]
    public Color warningColor = Color.red;

    [Header("Timing Settings")]
    [Tooltip("Time spent warning (changing color) before moving")]
    public float warningDuration = 1.0f;
    [Tooltip("How long the spikes stay fully extended at the peak")]
    public float activeDuration = 1.5f;
    [Tooltip("Time to wait between the end of one cycle and the start of the next")]
    public float cooldownDuration = 2.0f;

    // Internal tracking structures
    private List<Vector3> retractPositions = new List<Vector3>();
    private List<Vector3> popUpPositions = new List<Vector3>();
    private List<Renderer> spikeRenderers = new List<Renderer>();
    private List<Color> originalColors = new List<Color>();

    void Start()
    {
        // Cache positions, renderers, and original colors to avoid runtime overhead
        foreach (GameObject spike in spikes)
        {
            if (spike != null)
            {
                Vector3 startPos = spike.transform.localPosition;
                retractPositions.Add(startPos);
                popUpPositions.Add(startPos + popUpOffset);

                Renderer ren = spike.GetComponent<Renderer>();
                spikeRenderers.Add(ren);

                if (ren != null)
                {
                    // Store original material color (handles standard materials)
                    originalColors.Add(ren.material.color);
                }
                else
                {
                    originalColors.Add(Color.white);
                }
            }
        }

        // Start the infinite trap loop
        StartCoroutine(TrapCycleRoutine());
    }

    IEnumerator TrapCycleRoutine()
    {
        while (true)
        {
            // 1. WARNING PHASE: Change to the warning color
            SetSpikeColors(warningColor);
            yield return new WaitForSeconds(warningDuration);

            // 2. POP UP PHASE: Move smoothly to peak positions
            yield return StartCoroutine(MoveSpikes(popUpPositions));

            // 3. ACTIVE PHASE: Wait at the peak
            yield return new WaitForSeconds(activeDuration);

            // 4. RETRACT PHASE: Move smoothly back down
            yield return StartCoroutine(MoveSpikes(retractPositions));

            // 5. RESET PHASE: Return to original colors
            ResetSpikeColors();

            // 6. COOLDOWN PHASE: Wait before starting the cycle all over again
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    // Helper coroutine to smoothly interpolate positions
    IEnumerator MoveSpikes(List<Vector3> targetPositions)
    {
        bool allReached = false;
        while (!allReached)
        {
            allReached = true;
            for (int i = 0; i < spikes.Count; i++)
            {
                if (spikes[i] == null) continue;

                Transform t = spikes[i].transform;
                Vector3 target = targetPositions[i];

                t.localPosition = Vector3.MoveTowards(t.localPosition, target, movementSpeed * Time.deltaTime);

                if (Vector3.Distance(t.localPosition, target) > 0.001f)
                {
                    allReached = false;
                }
            }
            yield return null;
        }
    }

    // Helper to apply a uniform color
    void SetSpikeColors(Color color)
    {
        for (int i = 0; i < spikeRenderers.Count; i++)
            if (spikeRenderers[i] != null)
                spikeRenderers[i].material.color = color;
    }

    // Helper to restore cached colors
    void ResetSpikeColors()
    {
        for (int i = 0; i < spikeRenderers.Count; i++)
            if (spikeRenderers[i] != null)
                spikeRenderers[i].material.color = originalColors[i];
    }
}
*/


