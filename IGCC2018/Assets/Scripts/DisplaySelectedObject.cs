using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySelectedObject : MonoBehaviour
{
    [SerializeField]
    TempPlayer player;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = player.getSelectedIoT();
	}
}
