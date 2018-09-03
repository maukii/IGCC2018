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
        if (hackable.name != "Door")
        {
            hacked = true;
        }

        if(gameObject.GetComponentInParent<Animator>() != null)
        {
            GetComponentInParent<Animator>().SetTrigger("IsActive");
        }

        AudioManager.instance.PlaySound(gameObject.tag);
    }

    public void UnhackObject()
    {
        hacked = false;
        if(gameObject.GetComponentInParent<Animator>() != null)
        {
            GetComponentInParent<Animator>().SetTrigger("Disable");
        }
        AudioManager.instance.StopSound(gameObject.tag);
    }

}
