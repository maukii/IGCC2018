using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTBaseObj : MonoBehaviour
{
    // A toggle to check if its on or off
    public bool isActivated = false;

    // bool that tells whether object can be hacked or not
    protected bool canHack = false;

    // Duration of cooldown
    public float hackCooldownDuration = 1.0f;

    // Cooldown on hack, (1 second?)
    protected float hackCooldown = 0.0f;

    // How long object stays activated after hack (0.0f = infinite)
    public float activationDuration = 0.0f;

    // If activation has duration, reduce this one;
    protected float activationTick = 0.0f;



	// Use this for initialization
	void Start()
    {
        // nothing, i guess?
	}
	
	// Update is called once per frame
	virtual protected void Update()
    {
        // Prevent rehack until cooldown is up
        if (hackCooldown > 0.0f)
        {
            hackCooldown -= Time.deltaTime;
            canHack = false;
        }
        else
        {
            canHack = true;
        }

        /// Shifted into the objects itself, since some doesn't have activation time
        //// Shut off object after timer is up
        //if (isActivated)
        //{
        //    if (activationTick > 0.0f)
        //        activationTick -= Time.deltaTime;
        //    else
        //        isActivated = false;
        //}

        // NOTE: TEMP delete this later
        if (Input.GetKeyDown("space"))
        {
            Hack();
        }
    }

    // This called from player when hacking an object
    virtual protected bool Hack()
    {
        if (!canHack)
            return false;

        return true;
    }
}
