using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    private int candyPoints = 0;

    // For raycasting the last object
    GameObject lastHitObject = null;

    // Last object's material
    Material lastObjectMaterial = null;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        hideObjects();

        // By using simple move, manually placing gravity isnt needed.
        gameObject.GetComponent<CharacterController>().SimpleMove(new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical") * 400.0f * Time.deltaTime));

        gameObject.GetComponent<CharacterController>().SimpleMove(new Vector3((Input.GetAxis("Horizontal") * 400.0f * Time.deltaTime), 0.0f, 0.0f));
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

    void hideObjects()
    {
        RaycastHit hit;

        Ray ray = gameObject.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.55f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.Equals(lastHitObject))
                return;

            // If hitting something new or the player, set the last hit object back to its original material
            if (!hit.collider.gameObject.Equals(lastHitObject))
            {
                // Null check
                if (lastHitObject)
                {
                    lastHitObject.GetComponent<MeshRenderer>().material = lastObjectMaterial;
                }

                if (hit.collider.gameObject.CompareTag("Player"))
                    return;
            }

            lastHitObject = hit.collider.gameObject;
            lastObjectMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;

            Material newMat = Resources.Load("Translucent", typeof(Material)) as Material;
            hit.collider.gameObject.GetComponent<MeshRenderer>().material = newMat;
        }
    }
}
