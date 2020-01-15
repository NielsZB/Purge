using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptSelectEnemy : MonoBehaviour
{
    TutorialPrompt instanceOfPrompt;
    // Start is called before the first frame update
    void Start()
    {
        instanceOfPrompt = GetComponent<TutorialPrompt>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("RightStickButton") && instanceOfPrompt.checkIfinRange()){
            instanceOfPrompt.checkConditions();
        }
    }
}
