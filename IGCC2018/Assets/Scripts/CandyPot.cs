using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPot : MonoBehaviour
{
    // how much candy this object has in total
    public int totalCandy = 30;

    // how much candy is left
    int remaindingCandy = 0;

    // Duration needed to loot a single candy
    float lootDuration = 0.2f;

    // Reduce this one
    float lootTick = 0.0f;

	// Use this for initialization
	void Start ()
    {
        remaindingCandy = totalCandy;
        lootTick = lootDuration;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // NOTE: TEMP delete this later, player must be within range
        if (Input.GetKey("f"))
        {
            //Pass this in loot
            //other.GetComponentInParent<PlayerScriptHere>()
            Loot();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If this other collider belongs to the player...

        // This doesnt work fam wtf
    }

    void Loot()
    {
        if (remaindingCandy <= 0)
            return;

        if (lootTick > 0.0f)
        {
            lootTick -= Time.deltaTime;
        }
        else
        {
            lootTick = lootDuration;

            //Give player 1 candy
            --remaindingCandy;

            print("1 candy taken");
            print(remaindingCandy + " candies are left");
        }
    }
}
