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
    [SerializeField]
    Quaternion deviceRotation; // debug
    [SerializeField] Vector3 tilt;              // debug
    [SerializeField] float speed = 10f;
    [SerializeField] bool hacking;

    void Start()
    {
        gyro = Input.gyro;
        DEBUG_keyboard = SystemInfo.supportsGyroscope ? false : true;

        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;

        Debug.Log(SystemInfo.supportsGyroscope ? "Supports gyroscope" : "No gyroscope support");
    }

    void Update()
    {
        hacking = player.hacking;

        if (DEBUG_keyboard) // debug
        {
            float hor = Input.GetAxis("Horizontal"); // debugging feature: movement with keyboard
            float ver = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(hor, 0, ver);

            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            deviceRotation = DeviceRotation.Get();
            transform.rotation = deviceRotation;
        }

    }

    void OnGUI()
    {
        GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + gyro.rotationRate); // debug gyro status on screen
        GUI.Label(new Rect(500, 350, 200, 40), "Gyro attitude" + gyro.attitude);
        GUI.Label(new Rect(500, 400, 200, 40), "Gyro enabled : " + gyro.enabled);
    }

}
