using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    #region Parameters
    [SerializeField] private float rate;
    [SerializeField] private List<GameObject> mobPrefabs; // prefabs
    [SerializeField] private float[] weights;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private int minMobCount;
    [SerializeField] private int maxMobCount;
    [SerializeField] private float spawnBaseRate;
    [FormerlySerializedAs("spawnRate")] [SerializeField] private float spawnIncreaseRate;
    #endregion

    private float _timer;
    private float _spawnTime;
    private int _waveNumber;
    
    private Queue<GameObject> _spawnQueue;
    
    private enum SpawnType {
        Weighted,
        Sequential,
        Random
    }
    
    private SpawnType _spawnType;

    private void Awake()
    {
        SetupWeights();
        _timer = 0f;
        _waveNumber = 0;
        _spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        _spawnQueue = new Queue<GameObject>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnTime)
        {
            _timer = 0f;
            Spawn();
        }
    }

    public void Spawn()
    {
        if (_spawnQueue.Count != 0)
        {
            var mob = _spawnQueue.Dequeue();
            Instantiate(mob, transform.position, Quaternion.identity);
        }
        else
        {
            AddMobWave();
            _waveNumber++;
        }
        _spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void AddMobWave()
    {
        int spawnCount = minMobCount;
        if (_spawnType == SpawnType.Random)
        {
            spawnCount = (int) Math.Clamp(Math.Floor(minMobCount * Math.Pow(spawnBaseRate, _waveNumber * spawnIncreaseRate)), minMobCount, maxMobCount);
            for (int i = 0; i < spawnCount; i++)
            {
                var mob = mobPrefabs[UnityEngine.Random.Range(0, mobPrefabs.Count)];
                _spawnQueue.Enqueue(mob);
            }
        }
        else if (_spawnType == SpawnType.Weighted)
        {
            spawnCount = (int) Math.Clamp(Math.Floor(minMobCount * Math.Pow(spawnBaseRate, _waveNumber * spawnIncreaseRate)), minMobCount, maxMobCount);
            var adjustedWeights = GetAdjustedWeights();
            for (int i = 0; i < spawnCount; i++)
            {
                var mob = GetRandomMobByWeight(adjustedWeights, adjustedWeights.Sum());
                _spawnQueue.Enqueue(mob);
            }
        }
        else
        {
            //spawn type is levels, come back later
        }

        
    }
    
    private float[] GetAdjustedWeights()
    {
        float[] adjustedWeights = new float[weights.Length];
        for (int i = 0; i < weights.Length; i++)
        {
            adjustedWeights[i] = weights[i] * (1 + (_waveNumber * 0.1f));
        }
        return adjustedWeights;
    }
    
    private GameObject GetRandomMobByWeight(float[] adjustedWeights, float totalWeight)
    {
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < adjustedWeights.Length; i++)
        {
            cumulativeWeight += adjustedWeights[i];

            if (randomValue <= cumulativeWeight)
            {
                return mobPrefabs[i];
            }
        }

        // Fallback, in case of float precision issues
        return mobPrefabs[0];
    }

    public void SetSpawnerType(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Survival:
                _spawnType = SpawnType.Weighted;
                break;
            case GameMode.Timed:
                _spawnType = SpawnType.Random;
                break;
            case GameMode.Levels:
                _spawnType = SpawnType.Sequential;
                break;
            default:
                _spawnType = SpawnType.Weighted;
                break;
        }
    }

    private void SetupWeights()
    {
        /*
         * This function sets up the weights array to avoid exceptions pre weight-optimization
         */
        weights = new float[mobPrefabs.Count];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = 1f;
        }
    }
}
