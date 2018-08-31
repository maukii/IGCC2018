using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovementGyro : MonoBehaviour
{
    public PlayerData player;

    [Header("Use if no device connected")]
    public bool DEBUG_keyboard;

    Gyroscope gyro;

    [Header("-- Variables --")]
    [SerializeField] bool hacking = false;
    [SerializeField] bool isFlat = true;

    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float wantedPhoneScreenAngle = 45f;
    [SerializeField] float minTiltRequired = 0.35f;

    [HideInInspector] public Vector3 tilt;

    void Start()
    {
        gyro = Input.gyro;
        DEBUG_keyboard = SystemInfo.supportsGyroscope ? false : true;

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        Debug.Log(SystemInfo.supportsGyroscope ? "Supports gyroscope" : "No gyroscope support");
    }

    float currentSpeed;

    void Update()
    {

        hacking = player.hacking;
        player.playerPosition = transform.position;

        if (DEBUG_keyboard)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(hor, 0, ver);

            transform.Translate(direction * walkSpeed * Time.deltaTime);
        } // DEBUG -- keyboardinput
        else
        {
            tilt = Input.acceleration;

            if (isFlat)
                tilt = Quaternion.Euler(wantedPhoneScreenAngle, 0, 0) * tilt;

            if(Mathf.Abs(tilt.y) > minTiltRequired || Mathf.Abs(tilt.x) > minTiltRequired - 0.15)
            {
                if(currentSpeed < walkSpeed)
                {
                    currentSpeed += Time.deltaTime;
                }
                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime * tilt.magnitude);
            }
            else
            {
                if (currentSpeed > 0)
                {
                    currentSpeed -= Time.deltaTime;
                }
                else if(currentSpeed <= 0)
                {
                    currentSpeed = 0;
                }
                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime * tilt.magnitude);
            }

        }

        // DEBUG
        Debug.DrawRay(transform.position + Vector3.up, tilt, Color.green);

    }

    public Vector3 GetTilt()
    {
        return tilt;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + gyro.rotationRate);
        GUI.Label(new Rect(500, 350, 200, 40), "Gyro attitude" + gyro.attitude);
        GUI.Label(new Rect(500, 400, 200, 40), "Gyro enabled : " + gyro.enabled);
    }

}
