using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private List<int> path;
    private List<Transform> pathObjects;

    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    
    //TODO: Add strengths and weaknesses
    
    private int _currentPathIndex = 0;

    private void Awake()
    {
        var assistant = FindObjectOfType<PathAssistant>();
        path = assistant.ChooseRandomPath();
        pathObjects = new List<Transform>();
        var parent = GameObject.Find("Nodes");
        foreach (Transform child in parent.transform)
        {
            pathObjects.Add(child);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pathObjects[_currentPathIndex].position;
        _currentPathIndex++;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pathObjects[_currentPathIndex].position, speed * Time.deltaTime);
        Navigate();
    }

    protected void Navigate()
    {
        if (Vector3.Distance(transform.position, pathObjects[_currentPathIndex].position) < 0.01)
        {
            _currentPathIndex++;            
        }
    }
}
