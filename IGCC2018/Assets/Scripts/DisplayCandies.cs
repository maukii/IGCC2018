using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCandies : MonoBehaviour
{
    [SerializeField]
    TempPlayer player;

    string message;

	// Update is called once per frame
	void Update ()
    {
        message = player.getCandies() + "/" + player.getRequirement();
        gameObject.GetComponent<UnityEngine.UI.Text>().text = message;
    }
}
