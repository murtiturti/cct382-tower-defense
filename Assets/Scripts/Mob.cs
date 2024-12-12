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
    
    // Start is called before the first frame update
    void Start()
    {
        var assistant = FindObjectOfType<PathAssistant>();
        path = assistant.ChooseRandomPath();
        var parent = GameObject.Find("Nodes");
        foreach (Transform child in parent.transform)
        {
            pathObjects.Add(child);
        }
        transform.position = pathObjects[_currentPathIndex].position;
    }
}
