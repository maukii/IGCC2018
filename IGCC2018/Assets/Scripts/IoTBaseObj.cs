using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoTBaseObj : MonoBehaviour
{
    // A toggle to check if its on or off
    [SerializeField]
    protected bool isActivated = false;

    // bool that tells whether object can be hacked or not
    protected bool canHack = false;

    // Duration of cooldown
    [SerializeField]
    protected float hackCooldownDuration = 1.0f;

    // Cooldown on hack, (1 second?)
    protected float hackCooldown = 0.0f;

    // How long object stays activated after hack (0.0f = infinite)
    [SerializeField]
    protected float activationDuration = 0.0f;

    // If activation has duration, reduce this one;
    protected float activationTick = 0.0f;

    // how long object stays selected for.
    protected float selectionTick = 0.0f;

    // initial material
    protected Material defaultMat = null;

    // Use this for initialization
    virtual protected void Start()
    {
        if (gameObject.GetComponent<MeshRenderer>())
            defaultMat = gameObject.GetComponent<MeshRenderer>().material;
        else
            defaultMat = gameObject.GetComponentInParent<MeshRenderer>().material;
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

        if (selectionTick > 0.0f)
        {
            selectionTick -= Time.deltaTime;

            if (gameObject.GetComponent<MeshRenderer>())
                gameObject.GetComponent<MeshRenderer>().material = Resources.Load("Materials/Selected", typeof(Material)) as Material;
            else
                gameObject.GetComponentInParent<MeshRenderer>().material = Resources.Load("Materials/Selected", typeof(Material)) as Material;
        }
        else
        {
            if (gameObject.GetComponent<MeshRenderer>())
                gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            else
                gameObject.GetComponentInParent<MeshRenderer>().material = defaultMat;
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
        //if (Input.GetKeyDown("space"))
        //{
        //    Hack();
        //}
    }

    // This called from player when hacking an object
    virtual public bool Hack()
    {
        if (!canHack)
            return false;

        return true;
    }

    virtual public void Selected()
    {
    }
}
