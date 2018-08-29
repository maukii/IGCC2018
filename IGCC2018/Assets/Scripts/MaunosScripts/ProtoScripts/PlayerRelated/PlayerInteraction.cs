using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerData player;

    public KeyCode action;
    public KeyCode changeTarget;

    public List<GameObject> nearObjects = new List<GameObject>();
    [SerializeField] GameObject targetedObject = null;
    float nearRange;

    public GameObject GetTargetedObject()
    {
        if (targetedObject != null)
        {
            return targetedObject;
        }
        else
        {
            Debug.Log("No targets close by :c");
            return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(action) && !player.hacking)
        {
            if (targetedObject != null)
            {
                if(targetedObject.GetComponent<Hack>() != null)
                {
                    Debug.Log("Hacked " + targetedObject.name);
                    player.hacking = true;
                    targetedObject.GetComponent<Hack>().HackObject();
                    nearObjects.Remove(targetedObject);

                    CheckNearest();
                }
                else if(targetedObject.GetComponent<Loot>() != null)
                {
                    Debug.Log("Looted " + targetedObject.name);
                    targetedObject.gameObject.SetActive(false);
                    // give player points

                    nearObjects.Remove(targetedObject);
                    CheckNearest();
                }
            }
        }

        if (Input.GetKeyUp(action))
        {
            player.hacking = false;
            Debug.Log("Not hacking");
        }

        if (Input.GetKeyDown(changeTarget) && !player.hacking)
        {
            ChangeTarget();
        }

    }

    private void CheckNearest()
    {
        if(nearObjects.Count != 0)
        {
            foreach (GameObject a in nearObjects)
            {
                if(a.gameObject.GetComponent<Hack>() && !a.gameObject.GetComponent<Hack>().hacked)
                {
                    targetedObject = a;
                    break;
                }
                else if(a.gameObject.GetComponent<Loot>() && a.gameObject.activeSelf)
                {
                    targetedObject = a;
                    break;
                }
            }
        }
        else
        {
            targetedObject = null;
        }
    }

    private void ChangeTarget()
    {
        if (nearObjects.Count >= 1)
        {
            for (int i = 0; i < nearObjects.Count; i++)
            {
                if(targetedObject != nearObjects[i])
                {
                    targetedObject = nearObjects[i];
                    break;
                }
            }
            Debug.Log("Changed targer");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.GetComponent<Loot>() != null || (other.gameObject.GetComponent<Hack>() != null && !other.gameObject.GetComponent<Hack>().hacked)) && !nearObjects.Contains(other.gameObject) && other.gameObject.activeSelf)
        {
            nearObjects.Add(other.gameObject);

            if (targetedObject == null)
                targetedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.GetComponent<Hack>() != null || other.gameObject.GetComponent<Loot>() != null) && nearObjects.Contains(other.gameObject))
        {
            nearObjects.Remove(other.gameObject);
        }

        if (nearObjects.Count == 0)
            targetedObject = null;
        else if (nearObjects.Count == 1)
            targetedObject = nearObjects[0];
    }
}
