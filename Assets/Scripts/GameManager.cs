using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameMode gameMode;
    
    private Spawner _spawner;
    // Start is called before the first frame update
    void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
        _spawner.SetSpawnerType(gameMode);
    }

    // Update is called once per frame
    void Update()
    {
        // Game started at first frame Update is called
        /*
         * Gameplay:
         * Start spawning
         * Player has currency
         * Player has access to tower purchasing at all times
         * Mobs walk from node to node
         * Game ends when player loses all health points
         */
    }
}
