using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject allyPrefab;
    public GameObject player;

    public GameManager gameManager;

    public Transform[] spawnPoints;

    public float spawnTime = 5f;
    float playTime = 0f;
    int randomNumber;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isEndedGame)
        {
            Destroy(gameObject);
            return;
        }

        playTime += Time.deltaTime;
        randomNumber = (int)(playTime * 1000000);
        if (playTime >= spawnTime)
        {
            SpawnEnemy();
            playTime = 0;
        }
    }

    void SpawnEnemy()
    {
        int spawnPointIndex = randomNumber % spawnPoints.Length;
        int createIndex = randomNumber % 2;

        switch (createIndex)
        {
            case 0:
                Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.Euler(new Vector3(90, 0, 90)));
                break;
            case 1:
                Instantiate(allyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.Euler(new Vector3(90, 0, 90)));
                break;
        }
    }
}
