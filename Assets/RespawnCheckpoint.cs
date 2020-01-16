using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    public GameObject Violet;
    public bool violetInsideCheckpoint;
    [SerializeField]
    public static int latestCheckpoint = 0;
    public int thisCheckpointNr;
    public Transform [] checkpoints;
    public bool respawnFinished = true;
    public Animator fadeOutAnimator;
    
    PlayerHealth _playerhealth;
    Sword _sword;
    Shielding _shielding;

    void Start(){
        _playerhealth = Violet.GetComponent<PlayerHealth>();
        _sword = Violet.GetComponent<Sword>();
        _shielding = Violet.GetComponent<Shielding>();
    }

    void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.layer == 10 && thisCheckpointNr > latestCheckpoint){
            Debug.Log("Before new spawn point" + latestCheckpoint);
            latestCheckpoint = thisCheckpointNr;
            Debug.Log("After new spawn point" + latestCheckpoint);
        }
        
    }

    public void Respawn(){
        if(respawnFinished){

            StartCoroutine(startFadeOut());
            respawnFinished = false;
        }
    }

    IEnumerator startFadeOut(){
        Debug.Log("Coroutine Called");
        fadeOutAnimator.SetBool("Dead", true);
        yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);
        _playerhealth.ResetHealth();
        _sword.ResetHeat();
        _shielding.ResetStamina();
        Violet.transform.position = checkpoints[latestCheckpoint].transform.position;
        fadeOutAnimator.SetBool("Dead", false);
        respawnFinished = true;
    }
}
