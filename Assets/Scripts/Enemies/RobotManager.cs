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
    
    public List <GameObject> aliveRobots;

    [Header("Robots Data")]
    [SerializeField]
    static float timeLeft;
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
        checkCombatStates();
    }

    public void spawnWave(int waveSize, int waveLength){       
        
        //Randomly populated array
        int count = robotSpawnPoints.Length;
        int[] deck = new int[count];

        for(int i = 0; i < count; i++)
        {
            int j = Random.Range(0, i);

            deck[i] = deck[j];
            deck[j] = 0 + i;
        }

        for(int i = 0; i < deck.Length; i ++){
            print(deck [i]); 
        }
        
        if(waveSize<=spawnPointsGO.Length){
            for(int i = 0; i < waveSize; i++){
                aliveRobots.Add(Instantiate(robotPrefab, robotSpawnPoints[deck[i]]));
            }         
        }
        else
        Debug.Log("More enemies than spawn points error");
    }

     public void StartCombat(){
        StartCoroutine(startTheTimer());
        spawnWave(FirstWaveSize, FirstWaveLength);
    }

    public void RestartCombat(){
        StopCoroutine(startTheTimer());
        removeAllRobots();
        StartCoroutine(startTheTimer());
        FirstWaveDone = false;
        SecondWaveDone = false;
        ThirdWaveDone = false;
        FourthWaveDone = false;
    }

    IEnumerator startTheTimer(){
        while(true){
            timeLeft = Time.deltaTime;
            Debug.Log(timeLeft);
            yield return null;
        }
    }

    void removeAllRobots(){
        foreach(GameObject go in aliveRobots){
            aliveRobots.Remove(go);
            Destroy(go);
        }
    }

    void checkCombatStates(){
        if(aliveRobots.Count== 0){
            respawnOnce = true;
        }
        if(aliveRobots.Count==0 && respawnOnce){
            spawnWave(SecondWaveSize, SecondWaveLength);
            respawnOnce = false;
            FirstWaveDone = true;
        }

        if(aliveRobots.Count==0 && FirstWaveDone && respawnOnce){
            spawnWave(ThirdWaveSize, ThirdWaveLength);
            respawnOnce = false;
            SecondWaveDone = true;
        }

        if(aliveRobots.Count==0 && FirstWaveDone && SecondWaveDone && respawnOnce){
            spawnWave(FourthWaveSize, FourthWaveLength);
            respawnOnce = false;
            ThirdWaveDone = true;
        }

        if(aliveRobots.Count==0 && FirstWaveDone && SecondWaveDone && ThirdWaveDone && respawnOnce){
            respawnOnce = false;
            FourthWaveDone = true;
        }
    }

    public void removeRobot(GameObject go){
        aliveRobots.Remove(go);
    }
}
