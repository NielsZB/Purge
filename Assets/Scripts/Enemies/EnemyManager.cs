using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public struct Wave
{
    public int count;
    public float duration;
    public float breakDuration;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] Transform target;
    [SerializeField] GameObject prefab = null;
    [SerializeField] Wave[] waves;
    int waveIndex;

    Transform[] spawnpoint = null;
    bool[] spawnpointEnabled;

    bool waveInProgress;
    int numberOfRobots;
    int numberEnabledSpawnpoints;
    WaitForSecondsRealtime CombatEncounterForSeconds = new WaitForSecondsRealtime(1f);
    WaitForSecondsRealtime SpawnWaveForSeconds = new WaitForSecondsRealtime(5f);

    public void TriggerEncounter()
    {
        spawnpoint = new Transform[transform.childCount];
        spawnpointEnabled = new bool[spawnpoint.Length];

        for (int i = 0; i < spawnpoint.Length; i++)
        {
            spawnpoint[i] = transform.GetChild(i);
            spawnpointEnabled[i] = true;

        }
        StartCoroutine(CombatEncounter());
    }

    IEnumerator CombatEncounter()
    {
        while (waveIndex < waves.Length)
        {
            if (!waveInProgress)
            {
                StartCoroutine(SpawnWave());
                waveInProgress = true;
            }

            yield return CombatEncounterForSeconds;
        }
    }

    IEnumerator SpawnWave()
    {
        int robotsSpawnedInWave = 0;
        numberEnabledSpawnpoints = spawnpoint.Length;
        while (robotsSpawnedInWave < waves[waveIndex].count)
        {
            if (numberEnabledSpawnpoints == 0)
            {
                yield return SpawnWaveForSeconds;
            }
            int index = Random.Range(0, spawnpoint.Length);

            if (!spawnpointEnabled[index])
            {
                yield return null;
            }
            else
            {
                robotsSpawnedInWave++;
                numberOfRobots++;
                numberEnabledSpawnpoints--;
                StartCoroutine(DelayRobotSpawn(spawnpoint[index]));
                spawnpointEnabled[index] = false;
            }
            yield return null;
        }
        StartCoroutine(WaveTimer(waves[waveIndex].duration));
    }

    IEnumerator WaveTimer(float duration)
    {
        float t = 0;

        while (t < 1 & numberOfRobots > 0)
        {
            t += Time.deltaTime / duration;
            yield return null;
        }

        if (waves[waveIndex].duration == 0)
        {
            waveInProgress = false;
        }
        else
        {
            StartCoroutine(Break());
        }

        if (waveIndex < waves.Length)
        {

            waveIndex++;
        }
    }

    IEnumerator Break()
    {
        float t = 0;
        int index = waveIndex;
        while(t < 1)
        {
            t += Time.deltaTime / waves[index].duration;

            yield return null;
        }

        waveInProgress = false;
    }
    IEnumerator DelayRobotSpawn(Transform point)
    {
        float t = 0;
        float duration = Random.Range(0f, 5f);

        if (numberOfRobots != 1)
        {
            while (t < 1)
            {
                t += Time.deltaTime / duration;

                yield return null;
            }
        }

        GameObject newRobot = Instantiate(prefab, point.position, point.rotation);
        AddRobot(newRobot, point);
    }

    public void AddRobot(GameObject robotGameObject, Transform point)
    {
        combatManager.AddEntity(robotGameObject);

        RobotBehavior behavior = robotGameObject.GetComponentInChildren<RobotBehavior>();
        behavior.SetTarget(target);
        behavior.SetSpawnpoint(point);
        behavior.InitializeRobot(this);

        EnemyHealth health = robotGameObject.GetComponent<EnemyHealth>();
        health.InitializeRobot(this);
    }
    public void RemoveRobot()
    {
        numberOfRobots--;
    }

    public void EnableSpawnpoint(Transform point)
    {
        numberEnabledSpawnpoints++;
        for (int i = 0; i < spawnpoint.Length; i++)
        {
            if (spawnpoint[i] == point)
            {
                spawnpointEnabled[i] = true;
            }
        }
    }
}
