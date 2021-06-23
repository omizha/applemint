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
    }

    void SpawnEnemy()
    {
        // spawnPoints 중 한 곳에서 NPC가 생성되도록 합니다.
        int spawnPointIndex = (int)Random.Range(0, spawnPoints.Length);
        int createIndex = (int)Random.Range(0, 2);

        // enemy와 ally 둘 중 하나가 생성되도록 합니다.
        switch (createIndex)
        {
            case 0:
                Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.Euler(new Vector3(90, 0, 90)));
                break;
            case 1:
                Instantiate(allyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.Euler(new Vector3(90, 0, 90)));
                break;
        }

        Invoke("SpawnEnemy", Random.Range(spawnTime - 0.25f, spawnTime + 0.25f));
    }
}
