using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("Wave times")]
    public int FirstWaveLength;
    public int FirstWaveSize;
    public bool FirstWaveDone = false;
    public int SecondWaveLength;
    public int SecondWaveSize;
    public bool SecondWaveDone = false;
    public int ThirdWaveLength;
    public int ThirdWaveSize;
    public bool ThirdWaveDone = false;
    public int FourthWaveLength;
    public int FourthWaveSize;
    public bool FourthWaveDone = false;
    [Space(20)]

    GameObject [] spawnPointsGO;
    Transform [] robotSpawnPoints;

    public GameObject robotPrefab;
    public GameObject FirstRobotToSlay;
    
    public List <GameObject> aliveRobots = new List<GameObject>();
    public List <GameObject> deadRobots = new List<GameObject>();

    public Transform Violet;
    public IEnumerator timecounter;

    [Header("Robots Data")]
    static float timeLeft = 5f;
    public bool respawnOnce = false;

    void Start(){
        spawnPointsGO = GameObject.FindGameObjectsWithTag("SpawnPoint");
        robotSpawnPoints = new Transform[spawnPointsGO.Length];
        for(int i = 0; i < spawnPointsGO.Length; i ++){
            robotSpawnPoints [i] = spawnPointsGO[i].transform; 
        }
        aliveRobots.Add(FirstRobotToSlay);
    }

    void Update(){
        checkRobothealth();
        checkCombatStates();
    }

    void spawnWave(int waveSize, int waveLength){
        StopAllCoroutines();
        timecounter = restartTheTimer(waveLength);
        StartCoroutine(timecounter);

        //Randomly populated array
        int count = robotSpawnPoints.Length;
        int[] deck = new int[count];

        for(int i = 0; i < count; i++)
        {
            int j = Random.Range(0, i);

            deck[i] = deck[j];
            deck[j] = 0 + i;
        }
        
        if(waveSize<=spawnPointsGO.Length){
            for(int i = 0; i < waveSize; i++){
                GameObject addedRobot= Instantiate(robotPrefab, robotSpawnPoints[deck[i]].position, robotSpawnPoints[deck[i]].rotation) as GameObject;
                addedRobot.GetComponentInChildren<RobotBehavior>().SetTarget(Violet);
                aliveRobots.Add(addedRobot);
            }         
        }

        else
        Debug.Log("More enemies than spawn points error");
    }

    public void RestartCombat(){
        FirstWaveDone = false;
        SecondWaveDone = false;
        ThirdWaveDone = false;
        FourthWaveDone = false;
        removeAllRobots();
        StopAllCoroutines();
        timeLeft = 5f;
    }

    IEnumerator restartTheTimer(float _waveLength){
        timeLeft = _waveLength;
        while(true){
            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft);
            yield return null;
        }
    }

    void removeAllRobots(){
        if(aliveRobots.Count>0){
            for(int i = 0; i < aliveRobots.Count; i++){
                GameObject go = aliveRobots[i];
                removeRobot(aliveRobots[i]);
                Destroy(go);
            }

            for(int i = 0; i < deadRobots.Count; i++){
                GameObject go1 = deadRobots[i];
                removeRobot(deadRobots[i]);
                Destroy(go1);
            }
        }
    }

    void checkCombatStates(){
        if(aliveRobots.Count == 0 || timeLeft<=0){
            respawnOnce = true;
        }

        if(respawnOnce && !FirstWaveDone){
            Debug.Log("First Wave spawned");
            spawnWave(FirstWaveSize, FirstWaveLength);
            respawnOnce = false;
            FirstWaveDone = true;
        }

        if(FirstWaveDone && respawnOnce && !SecondWaveDone){
            spawnWave(SecondWaveSize, SecondWaveLength);
            respawnOnce = false;
            SecondWaveDone = true;
        }

        if(SecondWaveDone && respawnOnce && !ThirdWaveDone){
            spawnWave(ThirdWaveSize, ThirdWaveLength);
            respawnOnce = false;
            ThirdWaveDone = true;
        }

        if(ThirdWaveDone && respawnOnce && !FourthWaveDone){
            spawnWave(FourthWaveSize, FourthWaveLength);
            respawnOnce = false;
            FourthWaveDone = true;
        }
        if(FourthWaveDone && respawnOnce){
            StopAllCoroutines();
        }
    }

    public void removeRobot(GameObject go){
        aliveRobots.Remove(go);
    }

    void checkRobothealth(){
        
        if(aliveRobots.Count>0){
            for(int i = 0; i < aliveRobots.Count; i++){
                if(aliveRobots[i].GetComponent<EnemyHealth>().CurrentHealth <=0){
                    deadRobots.Add(aliveRobots[i]);
                    removeRobot(aliveRobots[i]);
                }
            }
        }
    }
}
