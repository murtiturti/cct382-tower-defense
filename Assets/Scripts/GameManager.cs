using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameMode gameMode;
    
    private Spawner _spawner;
    private Player _player;

    private int _score;

    [SerializeField] private IntEvent scoreGain;

    private float _gameTimer;
    private int _timedModeLength = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
        _spawner.SetSpawnerType(gameMode);
        
        _player = FindObjectOfType<Player>();
        scoreGain.RegisterListener(OnScoreGain);
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
        if (PlayerDied()) {
            Debug.Log("Game Over");
            _spawner.enabled = false;
            return;
        }

        if (gameMode == GameMode.Timed)
        {
            _gameTimer += Time.deltaTime;
            if (_gameTimer >= _timedModeLength * 60)
            {
                Debug.Log("Game Over");
                _spawner.enabled = false;
                return;
            }
        }
    }

    bool PlayerDied()
    {
        return _player.Health() <= 0;
    }

    void OnScoreGain(int gain)
    {
        _score += gain;
    }

    private void OnDestroy()
    {
        scoreGain.UnregisterListener(OnScoreGain);
    }
}
