using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerModel : MonoBehaviour
{

    [Header("-- Variables --")]
    [SerializeField] float turnSmoothning = 5;
    [SerializeField] float minTiltRequired = .2f;

    PlayerMovementGyro movementScript;
    TempPlayer tp;

    void Start()
    {
        movementScript = GetComponentInParent<PlayerMovementGyro>();
        tp = GetComponentInParent<TempPlayer>();
    }

    void Update()
    {

        Vector3 tilt = tp.GetTilt();        
        float targetRotation = Mathf.Atan2(tilt.x, tilt.y) * Mathf.Rad2Deg; // used to get right phones angle 

        if(Mathf.Abs(tilt.x) > minTiltRequired || Mathf.Abs(tilt.y) > minTiltRequired)
        {
            Quaternion wantedRotation = Quaternion.Euler(transform.rotation.x, targetRotation, transform.rotation.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, turnSmoothning * Time.deltaTime);
        }

    }
}
