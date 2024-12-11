using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Parameters
    [SerializeField] private float rate;
    [SerializeField] private List<GameObject> enemies; // should be list of Mobs when Mob base class is created
    [SerializeField] private float[] weights;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    #endregion

    private float _timer;
    
    private enum SpawnType {
        Weighted,
        Sequential,
        Random
    }
    
    private SpawnType _spawnType;

    private void Awake()
    {
        SetupWeights();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public void Spawn()
    {
        
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
        weights = new float[enemies.Count];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = 1f;
        }
    }
}
