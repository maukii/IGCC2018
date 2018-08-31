using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[DefaultExecutionOrder(-103)]

public class BuildNavMesh : SingletonMonoBehaviour<BuildNavMesh> {

    [SerializeField]
    private float _bakeInterval = 0.1f;

    public NavMeshSurface GetSurface()
    {
        return GetComponent<NavMeshSurface>();
    }

    private void Awake()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Use this for initialization
    void Start () {
        StartCoroutine("AutoBakeRepeat");

        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Update is called once per frame
    void Update () {
    }

    public IEnumerator AutoBakeRepeat()
    {
        yield return new WaitForSeconds(_bakeInterval);

        //GetComponent<NavMeshSurface>().BuildNavMesh();
        //StartCoroutine("AutoBakeRepeat");
    }
}
