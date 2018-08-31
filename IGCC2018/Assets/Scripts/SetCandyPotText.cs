using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCandyPotText : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.GetComponent<TextMesh>().text = gameObject.GetComponentInParent<CandyPot>().remaindingCandy.ToString();
	}
}