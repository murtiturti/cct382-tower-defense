using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAssistant : MonoBehaviour
{
    public List<int> path0;
    public List<int> path1;
    public List<int> path2;
    public List<int> path3;
    public List<int> path4;
    public List<int> path5;
    
    public List<List<int>> paths;

    private void Awake()
    {
        paths = new List<List<int>>();
        paths.Add(path0);
        paths.Add(path1);
        paths.Add(path2);
        paths.Add(path3);
        paths.Add(path4);
        paths.Add(path5);

    }

    public List<int> ChooseRandomPath()
    {
        return paths[UnityEngine.Random.Range(0, paths.Count)];
    }
}
