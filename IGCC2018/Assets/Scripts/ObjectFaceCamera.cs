using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceCamera : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
