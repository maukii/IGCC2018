using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTDoor : IoTBaseObj
{
    /// Open door = activated
    
    // Direction to rotate door and strength of rotation (neg/pos)
    float rotateDir = 0.0f;

    // Is door being opened/closed right now?
    bool isRot = false;

    // Closed door position
    float defaultRotY = 0.0f;

    // Use this for initialization
    void Start ()
    {
        // Edit cooldownDuration and hackCooldown here
        // Default cooldown duration is 1.0f.

        defaultRotY = transform.rotation.eulerAngles.y;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isRot)
            RotateDoor();

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
                    hackCooldown = hackCooldownDuration;

                    // (Sliding Door)
                    //gameObject.GetComponent<Transform>().Translate(new Vector3(-1.0f, 0.0f, 0.0f));

                    // (Rotating Door)
                    isRot = true;
                    rotateDir = -90;
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

        //// Open/Close door(Sliding Door)
        //if (isActivated)
        //    gameObject.GetComponent<Transform>().Translate(new Vector3(1.0f, 0.0f, 0.0f));
        //else
        //    gameObject.GetComponent<Transform>().Translate(new Vector3(-1.0f, 0.0f, 0.0f));

        // (Rotating Door)
        isRot = true;

        if (isActivated)
            rotateDir = 90;
        else
            rotateDir = -90;

        return true;
    }

    void RotateDoor()
    {
        transform.Rotate(Vector3.up * rotateDir * Time.deltaTime);

        if (transform.rotation.eulerAngles.y >= (defaultRotY + 90.0f) || gameObject.GetComponent<Transform>().rotation.eulerAngles.y <= defaultRotY)
        {
            // NOTE: PRINT
            print(transform.rotation.eulerAngles.y);
            isRot = false;
        }
    }
}
