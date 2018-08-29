using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMovement : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Transform>().Translate(new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<Transform>().Translate(new Vector3(-5.0f * Time.deltaTime, 0.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.S))
        {
            gameObject.GetComponent<Transform>().Translate(new Vector3(0.0f, 0.0f, -5.0f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Transform>().Translate(new Vector3(5.0f * Time.deltaTime, 0.0f, 0.0f));
        }
    }
}
