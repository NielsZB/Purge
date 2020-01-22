using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject prefab;
    [SerializeField] float target;

    List<RobotBehavior> activeRobots = new List<RobotBehavior>();
    bool[] spawnpointReady;
    public int[] waveRobotAmount;


    public void InitiateWave(int waveIndex)
    {
    }

    IEnumerator SpawningWave(int waveIndex)
    {
        while (waveRobotAmount[waveIndex] > 0)
        {
            Transform point;
            int spawnpointIndex = Random.Range(0, spawnpoints.Length - 1);
            if (spawnpointReady[spawnpointIndex])
            {
                point = spawnpoints[spawnpointIndex];
            }
            else
            {
                yield return null; 
            }
            yield return null;
        }
    }

    IEnumerator waitForSpace(int spawnpointIndex)
    {
        spawnpointReady[spawnpointIndex] = false;

        float t = 0;
        float duration = 7.2f;

        while (t < 1)
        {
            t = Time.deltaTime / duration;

            yield return null;
        }

        spawnpointReady[spawnpointIndex] = true;
    }
}
