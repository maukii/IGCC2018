using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    private int candyPoints = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-5.0f * Time.deltaTime, 0.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0.0f, 0.0f, -5.0f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(5.0f * Time.deltaTime, 0.0f, 0.0f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CandyPot") && Input.GetKey(KeyCode.F))
        {
            other.GetComponentInParent<CandyPot>().Loot(this);
        }
    }

    public void addCandyPoints(int points)
    {
        candyPoints += points;

        print("Gained " + points + " candies");
        print("Total of: " + candyPoints + " candies");
    }
}
