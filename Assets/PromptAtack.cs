using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptAtack : MonoBehaviour
{
    TutorialPrompt instanceOfPrompt;
    public GameObject robot;
    public Transform Violet;
    RobotBehavior _robotBehaviour;
    public RobotManager _robotManager;
    bool playonce = false;
    public TutorialPrompt _drawSheathe;
    // Start is called before the first frame update
    void Start()
    {
        instanceOfPrompt = GetComponent<TutorialPrompt>();
        _robotBehaviour = robot.GetComponentInChildren<RobotBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("X") && instanceOfPrompt.checkIfinRange() && !playonce){
            instanceOfPrompt.checkConditions();
            _robotBehaviour.SetTarget(Violet);
            _robotBehaviour.Activate();
            playonce = true;
            _drawSheathe.checkConditions();
        }
    }
}
