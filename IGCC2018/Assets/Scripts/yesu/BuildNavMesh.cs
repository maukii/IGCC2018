using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[DefaultExecutionOrder(-103)]

public class BuildNavMesh : SingletonMonoBehaviour<BuildNavMesh> {

    public NavMeshSurface GetSurface()
    {
        return GetComponent<NavMeshSurface>();
    }   

    void Awake()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Use this for initialization
    void Start () {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
