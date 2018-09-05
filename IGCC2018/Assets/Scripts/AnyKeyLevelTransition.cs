using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyLevelTransition : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<LevelTransition>().isActive = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.anyKey)
            gameObject.GetComponent<LevelTransition>().isActive = true;
    }
}
