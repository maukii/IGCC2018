using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyConstant : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        Destroy(GameObject.FindGameObjectWithTag("Constant"));
	}
}
