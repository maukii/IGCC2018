using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerModel : MonoBehaviour
{
    float rot;
    bool backing;

    [Header("-- Variables --")]
    [SerializeField] float turnSpeed = 5, targetRotation;
    [SerializeField] Vector2 turnLimitMinMax;

    PlayerMovementGyro movementScript;

    void Start()
    {
        movementScript = GetComponentInParent<PlayerMovementGyro>();
    }

    void LateUpdate()
    {
        //targetRotation = Mathf.Atan2(tilt.x, tilt.z) * Mathf.Rad2Deg; // not in use atm


        Vector3 tilt = movementScript.GetTilt();

        if (tilt.y < 0.2f)
            backing = true;
        else
            backing = false;
        

        if(tilt.y > 0)
        {
            if (tilt.x < -0.1f)
            {
                rot -= turnSpeed * Time.deltaTime;
            }
            if (tilt.x > 0.1f)
            {
                rot += turnSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (tilt.x < -0.1f)
            {
                rot += turnSpeed * Time.deltaTime;
            }
            if (tilt.x > 0.1f)
            {
                rot -= turnSpeed * Time.deltaTime;
            }
        }

        Quaternion tarRot = Quaternion.Euler(0, rot, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, turnSpeed * Time.deltaTime * Mathf.Abs(tilt.x));

    }
}
