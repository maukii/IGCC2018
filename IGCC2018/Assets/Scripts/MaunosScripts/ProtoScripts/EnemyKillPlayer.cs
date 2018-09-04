using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKillPlayer : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float extraDistance = 0.1f;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target.transform.position) <= agent.stoppingDistance + extraDistance)
        {
            target.GetComponentInChildren<Animator>().SetTrigger("Die");
            TempPlayer.playerIsDead = true;
        }
    }
}
