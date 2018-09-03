﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Bot : MonoBehaviour
{

    public NavMeshAgent Agent { get { return _agent; } }
    // target object(player)
    [SerializeField]
    private Transform _target;
    // search hackable object ray diameter
    [SerializeField]
    private float _detectingRange = 2;

    // navmesh agent
    private NavMeshAgent _agent;

    private bool _isChasing;

    // debug text 
    [SerializeField]
    private Text _currentStateText;

    // parent object of patrol points
    [SerializeField]
    private GameObject _patrolingPoints;

    // States
    [SerializeField]
    private BotState _currentState = null;

    private Patrol _patrolState = new Patrol();
    private Chase _chaseState = new Chase();
    private Search _seachState = new Search();


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _isChasing = false;
    }

    // Use this for initialization
    void Start()
    {
        // Initialize each State
        _patrolState.Init(_patrolingPoints, _detectingRange);
        _chaseState.SetTarget(_target);

        ChangeState(_patrolState);

        // agent won't be stop
        _agent.autoBraking = false;

        StartCoroutine("LostPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Execute(this);

        SearchPlayer();
    }

    private void SearchPlayer()
    {
        // Raycasting to player
        NavMeshHit hit;
        if (_agent.Raycast(_target.position, out hit))
        {
            // can't detect player
            if (_isChasing == true)
                StartCoroutine("LostPlayer");

            _isChasing = false;
        }
        else
        {
            // detect player
            if (_isChasing == false)
                ChangeState(_chaseState);

            StopCoroutine("LostPlayer");
            _isChasing = true;
        }
    }

    // On lost player 3secons continue chase
    IEnumerator LostPlayer()
    {
        yield return new WaitForSeconds(3.0f);

        ChangeState(_patrolState);
    }

    // change own state
    void ChangeState(BotState newState)
    {
        _currentState = newState;

        if (_currentStateText != null)
            _currentStateText.text = _currentState.ToString();
    }
}
