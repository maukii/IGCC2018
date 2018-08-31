using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime = 0.5f;

    void Update()
    {

        // smooth transition
        Vector3 wantedPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, wantedPosition, smoothTime * Time.deltaTime);
        transform.LookAt(target);

    }
}
