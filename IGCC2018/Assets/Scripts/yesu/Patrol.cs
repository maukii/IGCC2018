using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Patrol : BotState
{
    private const int RAY_HEIGHT = 5;
    private const float NEARBY_DISTANCE = 0.5f;

    // array of patrolling points
    private Transform[] points;

    // patrolling points num（default = 0）
    private int destPoint = 0;

    // ray for detect around object
    private Ray _ray;

    private float _detectingRange;

    GameObject _patrolPoints = null;
    GameObject _hackableTarget = null;
    GameObject _lightingTarget = null;


    // Execute patrolState
    public override void Execute(Bot bot)
    {
        NavMeshAgent agent = bot.Agent;

        //  set next position if agent arived at nearby to destination
        if (!agent.pathPending && agent.remainingDistance < NEARBY_DISTANCE)
            GotoNextPoint(agent);

        // detecting
        SearchLightObject(agent);
        // Do not search for hackable objects when lightingTarget target exists
        if (!_lightingTarget)
            SearchHackable(agent);

    }

    // Initialize patrolState
    public void Init(GameObject patrolPoints, float detecingtrange)
    {
        _detectingRange = detecingtrange;
        _patrolPoints = patrolPoints;
        InitPatrolPoints();
    }

    void InitPatrolPoints()
    {
        // set patrolPoints from children
        int childNum = _patrolPoints.transform.childCount;
        points = new Transform[childNum];

        // Init Patrol Points
        for (int i = 0; i < childNum; i++)
        {
            points[i] = _patrolPoints.transform.GetChild(i);
        }
    }

    void GotoNextPoint(NavMeshAgent agent)
    {
        // return if no patrolling points
        if (points.Length == 0)
            return;

        // set destination position 
        agent.destination = points[destPoint].position;
        // looping next points
        destPoint = (destPoint + 1) % points.Length;
    }

    private void SearchHackable(NavMeshAgent agent)
    {
        // ray reset
        Vector3 rayPos = agent.transform.position;
        rayPos.y = rayPos.y + RAY_HEIGHT;
        _ray = new Ray(rayPos, agent.transform.up * -1);

        // ray casting to around Objects
        RaycastHit[] hitinfos = Physics.SphereCastAll(_ray, _detectingRange);

        // if detect hackable object change target
        foreach (var hitinfo in hitinfos)
        {
            // detect "hackable" object by tag
            GameObject hitObj = hitinfo.transform.gameObject;
            if (hitObj.tag == "Hackable" && hitObj != _hackableTarget)
            {
                _hackableTarget = hitObj;
                points[destPoint] = _hackableTarget.transform;
                GotoNextPoint(agent);
                InitPatrolPoints();

                return;
            }
        }

        _hackableTarget = null;
    }

    private void SearchLightObject(NavMeshAgent agent)
    {
        // ray reset
        Vector3 rayPos = agent.transform.position;
        rayPos.y = rayPos.y + RAY_HEIGHT;
        _ray = new Ray(rayPos, agent.transform.up * -1);

        // ray casting to around Objects
        RaycastHit[] hitinfos = Physics.SphereCastAll(_ray, _detectingRange);

        // if detect hackable object change target
        foreach (var hitinfo in hitinfos)
        {
            // detect "light" object by tag
            GameObject hitObj = hitinfo.transform.gameObject;
            if (hitObj.tag != "light" || hitObj == _lightingTarget)
            {
                continue;
            }

            // TODO: check light ON/OFF
            if (true)
            {
                _lightingTarget = hitObj;
                points[destPoint] = _lightingTarget.transform;
                GotoNextPoint(agent);
                InitPatrolPoints();

                return;
            }
        }
        _lightingTarget = null;

    }

}
