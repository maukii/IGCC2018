using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeCollider : MonoBehaviour
{
    TempPlayer tPlayer;

	// Use this for initialization
	void Start ()
    {
        tPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<TempPlayer>();

        gameObject.GetComponent<LevelTransition>().isActive = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (tPlayer.getCandies() >= tPlayer.getRequirement())
        {
            gameObject.GetComponent<LevelTransition>().isActive = true;
        }
	}
}
