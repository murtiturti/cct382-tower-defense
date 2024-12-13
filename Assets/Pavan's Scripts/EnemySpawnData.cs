using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New EnemySpawnData", menuName = "Create EnemySpawnData")]
public class EnemySpawn : ScriptableObject
{
    public GameObject EnemyPrefab;
    public int EnemyID;
}
