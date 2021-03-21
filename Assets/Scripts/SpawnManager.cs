using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyObjs;
    [SerializeField]
    private Transform[] spawnPoints;

    private float maxSpawnDelay = 2;
    private float curSpawnDelay;


    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay >= maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 3);

        Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

    }
}
