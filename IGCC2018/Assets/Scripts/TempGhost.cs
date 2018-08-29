using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGhost : MonoBehaviour
{
    bool toggle = false;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.M))
        {
            print("change");

            toggle = !toggle;

            gameObject.GetComponent<Animator>().SetBool("isMoving", toggle);
        }
	}
}
