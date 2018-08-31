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

    // IoT Objects within hacking range
    List<GameObject> reachableIoT = new List<GameObject>();

    // Selected IoT to hack
    float selectedIndex = 0.0f;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // This turns the material of objects that are in the way of the camera viewing the player to translucent
        hideObjects();

        // Detects for IoT objects within a sphere radius
        detectObjects();
        
        // Player selects which IoT he wants to hack here
        selectIoT();

        // Hack the selected IoT, if possible
        if (Input.GetKeyDown("space"))
        {
            if (reachableIoT.Count > 0)
            {
                reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Hack();

                //foreach (GameObject obj in reachableIoT)
                //    obj.GetComponent<IoTBaseObj>().Hack();
            }
        }

        // By using simple move, manually placing gravity isnt needed.
        gameObject.GetComponent<CharacterController>().SimpleMove(new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical") * 400.0f * Time.deltaTime));
        gameObject.GetComponent<CharacterController>().SimpleMove(new Vector3((Input.GetAxis("Horizontal") * 400.0f * Time.deltaTime), 0.0f, 0.0f));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CandyPot") && Input.GetKey(KeyCode.F))
        {
            float distance = other.ClosestPoint(transform.position).magnitude;

            if (distance > 2.5f)
                return;

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

        Ray ray = gameObject.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

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

            Material newMat = Resources.Load("Materials/Translucent", typeof(Material)) as Material;
            hit.collider.gameObject.GetComponent<MeshRenderer>().material = newMat;
        }
    }

    void detectObjects()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 4);

        List<Collider> detectedList = new List<Collider>(detectedObjects);

        // Add IoT objects into hack list
        foreach (Collider coll in detectedList)
        {
            // If it is not an IoT (hackable object), ignore it
            if (!coll.gameObject.CompareTag("IoT"))
                continue;

            // If the list already contains the object, don't add it.
            if (!reachableIoT.Contains(coll.gameObject))
                reachableIoT.Add(coll.gameObject);
        }

        // List of things to remove
        List<GameObject> toRemove = new List<GameObject>();

        // Remove IoT objects that are no longer range from hack list
        foreach (GameObject obj in reachableIoT)
        {
            if (!detectedList.Contains(obj.GetComponent<Collider>()))
                toRemove.Add(obj);
        }

        // Remove objects that are no longer in range
        foreach (GameObject obj in toRemove)
        {
            reachableIoT.Remove(obj);
        }

        // Empty the lists
        detectedList.Clear();
        toRemove.Clear();
    }

    void selectIoT()
    {
        // If there's nothing to hack...
        if (reachableIoT.Count <= 0)
        {
            print("Nothing to hack");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            --selectedIndex;

        if (Input.GetKeyDown(KeyCode.E))
            ++selectedIndex;

        selectedIndex = Mathf.Repeat(selectedIndex, reachableIoT.Count);

        print(Mathf.FloorToInt(selectedIndex));

        print(reachableIoT[Mathf.FloorToInt(selectedIndex)]);

        // NOTE: delete this if check later.
        if (reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>())
            reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Selected();
    }
}
