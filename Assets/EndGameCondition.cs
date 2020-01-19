using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGameCondition : MonoBehaviour
{

    public Animator fadeOutAnimator;

    void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.layer == 10){
            StartCoroutine(startFadeout());
        }        
    }
    IEnumerator startFadeout(){
        fadeOutAnimator.SetBool("Dead", true);
        yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
