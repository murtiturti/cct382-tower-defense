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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
