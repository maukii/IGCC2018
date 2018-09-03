using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack : MonoBehaviour
{
    public PlayerData player;
    public Hackable hackable;

    public bool hacked;

    private void Awake()
    {
        hackable.hacked = false;
        hacked = hackable.hacked;
    }

    public void HackObject()
    {
        hacked = true;
        //transform.Rotate(0,-45,0);
        GetComponentInParent<Animator>().SetTrigger("Open");
    }

}
