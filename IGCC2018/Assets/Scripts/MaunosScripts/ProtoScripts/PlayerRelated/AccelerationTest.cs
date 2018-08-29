using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationTest : MonoBehaviour
{

    public float speed;
    public bool isFlat;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // should work in real build

        Debug.Log("gyro attitude" + Input.gyro.attitude);
        Debug.Log("gyro enabled: " + Input.gyro.enabled);
        Debug.Log("system supports gyro: " + SystemInfo.supportsGyroscope);

        if(SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
    }

    void Update()
    {
        Vector3 direction = new Vector3(Input.acceleration.x, -Input.acceleration.y, 0);

        if (isFlat)
            direction = Quaternion.Euler(-90, 0, 0) * direction;

        transform.Translate(direction * speed * Time.deltaTime);
        Debug.DrawRay(transform.position + Vector3.up, direction, Color.green);
    }
}
