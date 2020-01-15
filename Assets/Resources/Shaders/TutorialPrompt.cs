using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    public GameObject Violet;
    MeshRenderer prompt;
    public bool violetInRange = false;
    public bool done = false;
    public float waitBeforeFading = .1f;
    public float fadeDuration = 4f;
    Collider [] collidersInRange;
    public float violetCheckingRange = 5f;
    float distanceFromViolet;
    
    // Start is called before the first frame update
    void Start()
    {
    prompt = GetComponent<MeshRenderer>();
    }

    IEnumerator fadeAway(){
        yield return new WaitForSeconds(waitBeforeFading);
        float t = 0;
        while(prompt.material.GetFloat("_Alpha") > 0){
            t += Time.deltaTime / fadeDuration;
            prompt.material.SetFloat("_Alpha", Mathf.Lerp(1, 0, t));
        yield return null;
        }
    }

    public bool checkIfinRange(){
        /*collidersInRange = Physics.OverlapSphere(transform.position, violetCheckingRange, 10);

        if(collidersInRange.Length>0){
            foreach(Collider other in collidersInRange){
                Debug.Log(other.gameObject.layer);
                if(other.gameObject.layer == 10){
                    violetInRange = true;
                }
                else{
                    violetInRange = false;
                }
            }
        }*/

        distanceFromViolet = Vector3.Distance(transform.position, Violet.transform.position);

        if(distanceFromViolet <= violetCheckingRange){
            return true;
        }
        else
        return false;
    }

    public void checkConditions(){
        if(!done){
            StartCoroutine(fadeAway());
            done = true;
        }
    }
}
