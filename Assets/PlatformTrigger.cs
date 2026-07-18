using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Transform platform; 

    private void Start(){
        platform = transform.parent;
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.transform.parent = platform;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.transform.parent = null; 
        }
    }
}
