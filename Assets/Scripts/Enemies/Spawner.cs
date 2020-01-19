using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Spawner : MonoBehaviour
{
    public bool spawning;
    public Transform target;
    public GameObject prefab;
    public Transform[] spawnPoints;
    [MinMaxSlider(5f, 20f)] public Vector2 waitTimeRange;
    private void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    public void StartSpawning()
    {
        if (!spawning)
        {
            StartCoroutine(SpawnTimer());
            spawning = true;
        }

    }
    IEnumerator SpawnTimer()
    {
        while (true)
        {
            int randomPoint = Random.Range(0, spawnPoints.Length);
            float waitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / waitTime;
                yield return null;
            }
            GameObject enemy = Instantiate(prefab, spawnPoints[randomPoint].position, spawnPoints[randomPoint].rotation);
            enemy.GetComponentInChildren<RobotBehavior>().SetTarget(target);

            waitTimeRange.y = Mathf.Clamp(waitTimeRange.y - 0.5f, 1f, 10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartSpawning();
    }
}
