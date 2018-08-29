using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack : MonoBehaviour
{

    public Hackable hackable;
    public bool hacked;

    public void HackObject()
    {
        hackable.hacked = true;
        hacked = true;
    }

}
