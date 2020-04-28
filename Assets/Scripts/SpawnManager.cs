using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private List<GameObject> _powerup;

    [SerializeField]
    private float _spawnRate = 3f;
    [SerializeField]
    private float _powerupSpawnRate = 7f;

    private float _minVertical = -13f;
    private float _maxVertical = 10f;

    private float _maxHorizontal = 9f;
    private float _minHorizontal = -9f;

    private bool keepSpawning = true;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while(keepSpawning)
        {
            int i = (int)Mathf.Ceil(Random.Range(0, _powerup.Count));
            Debug.Log("Spawned: " + _powerup[i].transform.name);

            GameObject newPowerup =
                Instantiate(_powerup[i],
                new Vector3(
                        Random.Range(_minHorizontal, _maxHorizontal),
                        _maxVertical,
                        0
                    ),
                Quaternion.identity);

            yield return new WaitForSeconds(_powerupSpawnRate);
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (keepSpawning)
        {
            GameObject newEnemy = 
                Instantiate(_enemyPrefab,
                new Vector3(
                        Random.Range(_minHorizontal, _maxHorizontal),
                        _maxVertical,
                        0
                    ),
                Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void StopSpawning()
    {
        keepSpawning = false;
    }
}
