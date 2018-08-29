using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovementGyro : MonoBehaviour
{

    public PlayerData player;
    public GameObject playerModel;

    [Header("Use if no device connected")]
    public bool DEBUG_keyboard;

    Gyroscope gyro;

    [Header("-- Variables --")]
    [SerializeField] bool hacking = false, isFlat = true; 
    [SerializeField] float speed = 10f, rotSpeed;
    Quaternion deviceRotation;

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

    void Update()
    {
        hacking = player.hacking;
        player.playerPosition = transform.position;

        if (DEBUG_keyboard)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(hor, 0, ver);

            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            deviceRotation = DeviceRotation.Get();
            Vector3 direction = new Vector3(Input.acceleration.x, -Input.acceleration.y, 0);

            if (isFlat)
                direction = Quaternion.Euler(-90, 0, 0) * direction;

            // MOVEMENT
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // ROTATION
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            Debug.DrawRay(transform.position + Vector3.up, direction, Color.green);
        }
        //playerModel.transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + gyro.rotationRate);
        GUI.Label(new Rect(500, 350, 200, 40), "Gyro attitude" + gyro.attitude);
        GUI.Label(new Rect(500, 400, 200, 40), "Gyro enabled : " + gyro.enabled);
    }

}
