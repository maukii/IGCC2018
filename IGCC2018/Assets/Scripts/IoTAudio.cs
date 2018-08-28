﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTAudio : IoTBaseObj
{
	// Use this for initialization
	void Start ()
    {
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

                    gameObject.GetComponent<AudioSource>().Stop();
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

        if (isActivated)
            gameObject.GetComponent<AudioSource>().Play();
        else
            gameObject.GetComponent<AudioSource>().Stop();

        return true;
    }
}