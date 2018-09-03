using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBattery : MonoBehaviour
{
    [SerializeField]
    TempPlayer player;

    string message;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        message = "Battery Charge: " + player.GetComponent<BatteryCharge>().GetAmountLeft().ToString() + "%";
        gameObject.GetComponent<UnityEngine.UI.Text>().text = message;
    }
}
