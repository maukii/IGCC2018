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
    float defaultAxisRot = 0.0f;

    // Flip door opening side
    [SerializeField]
    bool flipDirection = false;

    // Flip way of opening
    [SerializeField]
    bool isLid = false;

    // Leeway of detecting if door is open or closed
    float deltaOffset = 2.0f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // null check
        if (objectType.Length == 0)
        {
            objectType = "Door";
        }

        // Edit cooldownDuration and hackCooldown here
        // Default cooldown duration is 1.0f.


        if (isLid)
            defaultAxisRot = transform.rotation.eulerAngles.x;
        else
            defaultAxisRot = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isRot)
            RotateDoor();
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

        if (gameObject.GetComponent<AudioSource>())
            gameObject.GetComponent<AudioSource>().Play();
        else
            print("No audio detected");

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

    public override void Selected()
    {
        base.Selected();

        selectionTick = 0.1f;
    }

    public override void Disable()
    {
        base.Disable();

        gameObject.GetComponent<AudioSource>().Play();
        activationTick = 0.0f;
        isActivated = false;
        hackCooldown = hackCooldownDuration;

        // (Sliding Door)
        //gameObject.GetComponent<Transform>().Translate(new Vector3(-1.0f, 0.0f, 0.0f));

        // (Rotating Door)
        isRot = true;
        rotateDir = -90;
    }

    void RotateDoor()
    {
        if (isLid)
        {
            if (flipDirection)
            {
                transform.Rotate(Vector3.up * -rotateDir * Time.deltaTime);

                isRot = (rotateDir > 0) ?
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, defaultAxisRot + 90.0f)) <= deltaOffset) :
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, defaultAxisRot)) <= deltaOffset);
            }
            else
            {
                transform.Rotate(Vector3.up * rotateDir * Time.deltaTime);

                /// oh god this took me longer than I expected it to take
                isRot = (rotateDir > 0) ?
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, (defaultAxisRot - 90.0f))) <= deltaOffset) :
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, defaultAxisRot)) <= deltaOffset);
            }
        }
        else
        {
            if (flipDirection)
            {
                transform.Rotate(Vector3.up * -rotateDir * Time.deltaTime);

                /// oh god this took me longer than I expected it to take
                isRot = (rotateDir > 0) ?
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, (defaultAxisRot - 90.0f))) <= deltaOffset) :
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, defaultAxisRot)) <= deltaOffset);
            }
            else
            {
                transform.Rotate(Vector3.up * rotateDir * Time.deltaTime);

                isRot = (rotateDir > 0) ?
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, (defaultAxisRot + 90.0f))) <= deltaOffset) :
                    !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, defaultAxisRot)) <= deltaOffset);
            }
        }

        if (!isRot)
        {
            if (gameObject.GetComponent<AudioSource>())
                gameObject.GetComponent<AudioSource>().Stop();
            else
                print("No audio detected");
        }
    }
}
