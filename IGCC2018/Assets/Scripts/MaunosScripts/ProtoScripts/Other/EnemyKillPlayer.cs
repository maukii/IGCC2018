using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKillPlayer : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float minDistance = 0.5f;
    NavMeshAgent agent;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target.transform.position) <= minDistance)
        {
            agent.isStopped = true;

            if(!TempPlayer.playerIsDead)
            {
                TempPlayer.playerIsDead = true;
                target.GetComponent<TempPlayer>().Die();
            }
        }
    }

    public void Respawn()
    {
        agent.isStopped = false;
        transform.position = startPos;
    }

}
