using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTLight : IoTBaseObj
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // null check
        if (objectType == null)
        {
            objectType = "Light";
        }

        gameObject.GetComponent<Light>().intensity = 0.0f;
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

        // Turn light on/off
        if (isActivated)
            gameObject.GetComponent<Light>().intensity = 1.0f;
        else
            gameObject.GetComponent<Light>().intensity = 0.0f;

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

        gameObject.GetComponent<Light>().intensity = 0.0f;
    }
}
