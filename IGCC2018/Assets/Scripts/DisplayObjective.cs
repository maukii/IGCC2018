using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayObjective : MonoBehaviour
{
    [SerializeField]
    TempPlayer player;

    string message;

    private void Start()
    {
        message = "";
        gameObject.GetComponent<UnityEngine.UI.Text>().text = message;
    }

    // Update is called once per frame
    void Update ()
    {
        if (player.getCandies() >= player.getRequirement())
        {
            message = "Escape the house from the entrance!";
            gameObject.GetComponent<UnityEngine.UI.Text>().text = message;
        }
    }
}
