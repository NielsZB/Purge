using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    MeshRenderer prompt;
    public bool done = false;
    public float waitBeforeFading = 1f;
    public float fadeDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("LeftHorizontal") > 0 && !done){
            StartCoroutine(fadeAway());
            done = true;
        }
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
}
