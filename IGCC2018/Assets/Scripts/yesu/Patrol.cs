using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Patrol : BotState
{
    private const int RAY_HEIGHT = 5;
    private const float LIGHT_DETECT_DISTANCE = 6.0f;
    private const float NEARBY_DISTANCE = 0.2f;

    // array of patrolling points
    private Transform[] points;

    // patrolling points num（default = 0）
    private int destPoint = 0;

    private float _detectingRange;

    GameObject _patrolPoints = null;
    GameObject _hackableTarget = null;
    GameObject _lightingTarget = null;
    GameObject _oldHackable = null;
    GameObject _oldLight = null;

    IoTBaseObj _iotTrget = null;


    // Execute patrolState
    public override void Execute(Bot bot)
    {
        NavMeshAgent agent = bot.Agent;

        //  set next position if agent arived at nearby to destination

        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {

            if (!agent.pathPending && agent.remainingDistance < NEARBY_DISTANCE)
            {
                if (_iotTrget)
                    _iotTrget.Disable();

                _oldLight = _lightingTarget;
                _oldHackable = _hackableTarget;
                _lightingTarget = null;
                GotoNextPoint(agent);
            }

            // detecting
            if (!_lightingTarget)
            {
                SearchHackable(agent);

                SearchLightObject(agent);
            }
        }
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
        Ray ray = new Ray(rayPos, agent.transform.up * -1);

        // ray casting to around Objects
        RaycastHit[] hitinfos = Physics.SphereCastAll(ray, _detectingRange);

        // if detect hackable object change target
        foreach (var hitinfo in hitinfos)
        {
            // detect "IoT" object by tag
            GameObject hitObj = hitinfo.transform.gameObject;
            if (hitObj.tag != "IoT" || hitObj == _hackableTarget || hitObj == _oldHackable)
                continue;

            // check IoT object "isActive"
            if (FindIoT(hitObj))
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

        Vector3 rayPosUp = new Vector3(rayPos.x, rayPos.y + RAY_HEIGHT, rayPos.z);
        // ray for detect around object
        Ray ray = new Ray(rayPosUp, agent.transform.up * -1);

        // ray casting to around Objects
        RaycastHit[] hitinfos = Physics.SphereCastAll(ray, LIGHT_DETECT_DISTANCE);

        // if detect hackable object change target
        foreach (var hitinfo in hitinfos)
        {
            // detect "light" object by component
            GameObject hitObj = hitinfo.transform.gameObject;

            IoTLight lightCmp = hitObj.GetComponent<IoTLight>();
            if (lightCmp == null || hitObj == _lightingTarget || hitObj == _oldLight)
            {
                continue;
            }

            // check ghost can see light
            Ray lightRay = new Ray(rayPos, hitObj.transform.position - rayPos);
            RaycastHit hit;
            Physics.SphereCast(lightRay, 3.0f, out hit);

            if (hit.transform.gameObject == hitObj)
                continue;

            // check light object "isActive"
            if (FindIoT(hitObj))
            {
                _lightingTarget = hitObj;
                points[destPoint] = _lightingTarget.transform;
                GotoNextPoint(agent);
                InitPatrolPoints();

                return;
            }
        }
    }

    // Assume this is the IoT target or something
    bool FindIoT(GameObject target)
    {
        _iotTrget = target.GetComponent<IoTBaseObj>();

        // TRUE = its on
        if (_iotTrget!= null && _iotTrget.GetActivated())
        {
            // Check the type of the IoT.
            switch (target.GetComponent<IoTBaseObj>().GetIoTType())
            {
                case "Door":
                     return true;
                case "Audio":
                    return true;
                case "Light":
                    return true;

                default:
                    // If not tagged properly, ignore it
                    return false;
            }
        }
        return false;
    }
}
