using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public int spawnCount = 5;
    public float spawnTimerSet = 1f;
    public float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnTimerSet;
        GameManager.instance.activeSpawners.Add(gameObject);
    }
    

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && spawnCount > 0)
        {
            spawnTimer = spawnTimerSet;
            int pointPick = (int)Random.Range(0, spawnPoints.Count-1);
            GameObject enemyToSpawn = GameManager.instance.enemyLevels[0];
            if (GameManager.instance.gameplayTimer >= 120)
            {
                enemyToSpawn = GameManager.instance.enemyLevels[2];
            }
            else if (GameManager.instance.gameplayTimer >= 60)
            {
                enemyToSpawn = GameManager.instance.enemyLevels[1];
            }
            Instantiate(enemyToSpawn, spawnPoints[pointPick].transform.position, Quaternion.identity);
            spawnCount--;
            
        }
        else if (spawnCount <= 0)
        {
            GameManager.instance.activeSpawners.Remove(gameObject);
            Destroy(gameObject);
        }
    }

}
