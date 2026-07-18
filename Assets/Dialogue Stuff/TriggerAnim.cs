using UnityEngine;
using Yarn.Unity;

public class TriggerAnim : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [YarnCommand("triggeranim")]
    public void TriggerAnimation()
    {
        animator.SetTrigger("OnLineEnd");
    }
}
