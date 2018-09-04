using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTAudio : IoTBaseObj
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // null check
        if (objectType == null)
        {
            objectType = "Audio";
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override bool Hack()
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

    public override void Selected()
    {
        base.Selected();

        selectionTick = 0.1f;
    }

    public override void Disable()
    {
        base.Disable();

        isActivated = false;

        gameObject.GetComponent<AudioSource>().Stop();
    }
}
