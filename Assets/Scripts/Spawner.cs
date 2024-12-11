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

    private void Awake()
    {
        weights = new float[enemies.Count];
    }

    public void Spawn()
    {
        
    }
}
