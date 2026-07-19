using UnityEngine;
using Yarn;
using Yarn.Unity;

public class TriggerNode : MonoBehaviour
{
    [SerializeField] GameObject dialogueSystem;
    private DialogueRunner dialogueRunner;
    private DialogueBox dialogue;
    private StacheMovement stacheMovement;
    //[SerializeField] MouseMovement mouseMovement;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("dialogue"))
        {
            if (collision.gameObject.GetComponent<DialogueBox>() != null)
            {
                dialogue = collision.gameObject.GetComponent<DialogueBox>();
                DialogueStart();
            }
        }
    }

    private void Start()
    {
        dialogueRunner = dialogueSystem.GetComponent<DialogueRunner>();
        stacheMovement = GetComponent<StacheMovement>();
        //if (mouseMovement == null) { mouseMovement = GetComponent<MouseMovement>(); }
    }

    private void DialogueStart()
    {
        if (dialogue != null && !dialogueRunner.IsDialogueRunning && !dialogue.triggered)
        {
            dialogueRunner.StartDialogue(dialogue.dialogueNode);
            UnEnable();
        }
    }

    [YarnCommand("dialogueend")]
    public void DialogueEnd()
    {
        if (dialogue != null) { dialogue.triggered = true; }
        Invoke(nameof(ReEnable), 0.5f);
    }

    [YarnCommand("dialoguestart")]
    public void UnEnable()
    {
        if (stacheMovement != null) { stacheMovement.enabled = false; }
        //if (mouseMovement != null) { mouseMovement.enabled = false; }
    }
    private void ReEnable()
    {
        if (stacheMovement != null) { stacheMovement.enabled = true; }
        //if (mouseMovement != null) { mouseMovement.enabled = true; }
    }
}
