using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerModel : MonoBehaviour
{

    [Header("-- Variables --")]
    [SerializeField] float turnSmoothning = 5;
    [SerializeField] float minTiltRequired = .2f;
    [SerializeField] float targetRotation;

    Vector3 tilt;

    PlayerMovementGyro movementScript;
    TempPlayer tp;

    [SerializeField] GameObject enemy;

    void Start()
    {
        movementScript = GetComponentInParent<PlayerMovementGyro>();
        tp = GetComponentInParent<TempPlayer>();
    }

    void Update()
    {
        tilt = tp.GetTilt();        
        targetRotation = Mathf.Atan2(tilt.x, tilt.y) * Mathf.Rad2Deg;
        CalculatePlayerRotation();
    }

    private void CalculatePlayerRotation()
    {
        if(!TempPlayer.playerIsDead)
        {
            if (!tp.DEBUG_useKeyboard)
            {
                if (Mathf.Abs(tilt.x) > minTiltRequired || Mathf.Abs(tilt.y) > minTiltRequired)
                {
                    Quaternion wantedRotation = Quaternion.Euler(transform.rotation.x, targetRotation, transform.rotation.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, turnSmoothning * Time.deltaTime);
                }
            }
            else
            {
                if (tilt.magnitude != 0)
                {
                    Quaternion wantedRotation = Quaternion.Euler(transform.rotation.x, targetRotation, transform.rotation.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, turnSmoothning * Time.deltaTime);
                }
            }
        }
        else
        {
            var lookPos = enemy.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSmoothning);
        }
    }

}
