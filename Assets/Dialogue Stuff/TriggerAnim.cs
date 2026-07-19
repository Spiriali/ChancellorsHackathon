using System.Diagnostics;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class TriggerAnim : MonoBehaviour
{
    private Animator animator;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [YarnCommand("triggeranim")]
    public void TriggerAnimation()
    {
        animator.SetTrigger("OnLineEnd");
    }

    [YarnCommand("gullcaw")]
    public void GullCaw()
    {
        audioSource.Play();
        UnityEngine.Debug.Log("played gull sound");
    }

    [YarnCommand("scenetransition")]
    public void SceneTransition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
