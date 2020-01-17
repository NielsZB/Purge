using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("Wave times")]
    public float FirstWaveLength;
    public float FirstWaveSize;
    public bool FirstWaveDone = false;
    public float SecondWaveLength;
    public float SecondWaveSize;
    public bool SecondWaveDone = false;
    public float ThirdWaveLength;
    public float ThirdWaveSize;
    public bool ThirdWaveDone = false;
    public float FourthWaveLength;
    public float FourthWaveSize;
    public bool FourthdWaveDone = false;
    [Space(20)]

    GameObject [] spawnPointsGO;
    Transform [] robotSpawnPoints;

    public GameObject robotPrefab;
    
    List <GameObject> aliveRobots;

    [Header("Robots Data")]
    [SerializeField]
    float timeLeft;
    [SerializeField]
    float robotsLeft;

    void Start(){
        spawnPointsGO = GameObject.FindGameObjectsWithTag("SpawnPoint");
        robotSpawnPoints = new Transform[spawnPointsGO.Length];
        for(int i = 0; i < spawnPointsGO.Length; i ++){
            robotSpawnPoints [i] = spawnPointsGO[i].transform; 
        }
        spawnWave(3, 1);
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
                Instantiate(robotPrefab, robotSpawnPoints[deck[i]]);
            }         
        }
        else
        Debug.Log("More enemies than spawn points error");
    }
}
