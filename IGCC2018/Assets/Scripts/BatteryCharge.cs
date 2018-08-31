using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCharge : MonoBehaviour
{
    // Max battery charge
    [SerializeField]
    float maxCharge = 100.0f;

    // Current Battery level
    float currCharge = 0.0f;

    // How fast the battery recharges
    float regenRate = 2.0f;

    // How long it takes before a single regen tick
    float regenTick = 1.0f;

	// Use this for initialization
	void Start ()
    {
        currCharge = maxCharge;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currCharge < maxCharge)
            Recharge();

        print(currCharge);
	}

    void Recharge()
    {
        regenTick -= Time.deltaTime;

        if (regenTick <= 0.0f)
        {
            regenTick = 1.0f;
            currCharge += 2.0f;

            currCharge = Mathf.Clamp(currCharge, 0.0f, maxCharge);
        }
    }

    public bool CanHack(float amount)
    {
        if (currCharge <= amount)
            return false;

        return true;
    }

    public void DrainBattery(float amount)
    {
        currCharge -= amount;
    }

    public float GetAmountLeft()
    {
        return currCharge;
    }
}
