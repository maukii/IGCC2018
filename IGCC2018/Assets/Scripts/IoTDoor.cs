using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTDoor : IoTBaseObj
{
    /// Open door = activated

    // Use this for initialization
    void Start ()
    {
        // Edit cooldownDuration and hackCooldown here
        // Default cooldown duration is 1.0f.
	}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // Check if there is a activation duration
        if (activationDuration > 0.0f)
        {
            // If active... (Run the disable once)
            if (isActivated)
            {
                if (activationTick > 0.0f)
                    activationTick -= Time.deltaTime;
                else
                {
                    isActivated = false;

                    gameObject.GetComponent<Transform>().Translate(new Vector3(-1.0f, 0.0f, 0.0f));
                }
            }
        }
    }

    protected override bool Hack()
    {
        // Check if cooldown is on
        if (!base.Hack())
            return false;

        /// Open hacking minigame (Wishlist)
        /// If successful, then toggle

        hackCooldown = hackCooldownDuration;
        activationTick = activationDuration;
        isActivated = !isActivated;

        // Open/Close door
        if (isActivated)
            gameObject.GetComponent<Transform>().Translate(new Vector3(1.0f, 0.0f, 0.0f));
        else
            gameObject.GetComponent<Transform>().Translate(new Vector3(-1.0f, 0.0f, 0.0f));

        return true;
    }
}
