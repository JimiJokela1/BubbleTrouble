using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnInterval = 1f;
    public GameObject EnemyPrefab;
    public int TotalEnemies = 1;

    private int _enemiesSpawned = 0;
    private float _spawnTimer = 0f;

    public AudioClip spawnSound;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > SpawnInterval && _enemiesSpawned < TotalEnemies)
        {
            _spawnTimer = 0;
            _enemiesSpawned++;
            SpawnEnemy();
        }
    }

    public void ResetAmount()
    {
        _enemiesSpawned = 0;
    }
    
    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(EnemyPrefab, transform.position + Vector3.down * 2f, Quaternion.identity);

        enemy.GetComponent<Enemy>().StartEmerge();

        AudioSource.PlayClipAtPoint(spawnSound, transform.position);
    }
}
