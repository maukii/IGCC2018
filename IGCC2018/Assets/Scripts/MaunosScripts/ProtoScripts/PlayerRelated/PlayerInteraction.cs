using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerData player;
    [SerializeField] bool hacking;

    // PC inputs
    public bool useKeyboard = false;
    public KeyCode action;
    public KeyCode changeTargetUp, changeTargetDown;

    [Header("-- Variables --")]
    public List<GameObject> nearObjects = new List<GameObject>();
    [SerializeField] GameObject targetedObject = null;
    [SerializeField] int targetIndex;
    [SerializeField] float timeTakesToHack = 2f;
    [SerializeField] Material defaultMat;
    [SerializeField] Material highlightMat;


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
        hacking = player.hacking;

        if(/* TempPlayer.useKeyboardInput */ useKeyboard)
        {
            if (Input.GetKeyDown(changeTargetUp) && !player.hacking)
            {
                ChangeTargetUp();
            }

            if(Input.GetKeyDown(changeTargetDown) && !player.hacking)
            {
                ChangeTargetDown();
            }

            if(Input.GetKeyDown(action) && !player.hacking)
            {
                Action();
            }
        }

        Highlighting();

        if(nearObjects.Count == 0)
            player.hacking = false;
    }

    private void Highlighting()
    {
        for (int i = 0; i < nearObjects.Count; i++)
        {
            if (nearObjects[i].gameObject == targetedObject)
            {
                nearObjects[i].GetComponent<MeshRenderer>().material = highlightMat;
            }
            else
            {
                nearObjects[i].GetComponent<MeshRenderer>().material = defaultMat;
            }
        }
    }

    public void Action() 
    {
        if (targetedObject != null)
        {
            if (targetedObject.GetComponent<Hack>() != null)
            {
                player.hacking = true;
                StartCoroutine(StartHack());
            }
            else if (targetedObject.GetComponent<Loot>() != null)
            {
                Debug.Log("Looted " + targetedObject.name);
                targetedObject.GetComponent<Loot>().GiveCandy();
                if(!targetedObject.activeSelf)
                {
                    nearObjects.Remove(targetedObject);
                    targetedObject = null;
                    CheckNearest();
                }
            }
        }
    }

    private IEnumerator StartHack()
    {
        GameObject hackingObject = targetedObject;
        Debug.Log("Started hacking " + hackingObject.name);
        yield return new WaitForSeconds(timeTakesToHack);

        if(hacking)
        {
            if(hacking == targetedObject)
            {
                Debug.Log("Finished hacking");
                targetedObject.GetComponent<Hack>().HackObject();
                targetedObject.GetComponent<MeshRenderer>().material = defaultMat;
                player.hacking = false;
                nearObjects.Remove(targetedObject);
                CheckNearest();
            }
        }
        else if(!hacking || targetedObject.GetComponent<Hack>() == null || targetedObject == null || hackingObject != targetedObject)
        {
            player.hacking = false;
            Debug.Log("Player stopped hacking for some reason");
            CheckNearest();
        }

    }

    private void CheckNearest()
    {
        if(nearObjects.Count != 0)
        {
            foreach (GameObject a in nearObjects)
            {
                if(a.gameObject.GetComponent<Hack>() != null)
                {
                    if(!a.gameObject.GetComponent<Hack>().hacked)
                    {
                        targetedObject = a;
                        break;
                    }
                }
                if(a.gameObject.GetComponent<Loot>() != null && a.gameObject.activeSelf)
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

    public void ChangeTargetUp()
    {
        if (nearObjects.Count > 0)
        {
            targetIndex++;
            if (targetIndex >= nearObjects.Count)
            {
                targetIndex = 0;
                targetedObject = nearObjects[targetIndex];
            }

            targetedObject = nearObjects[targetIndex];
        }
    }

    public void ChangeTargetDown()
    {
        if (nearObjects.Count > 0)
        {
            targetIndex--;
            if (targetIndex < 0)
            {
                targetIndex = nearObjects.Count - 1;
                targetedObject = nearObjects[targetIndex];
            }

            targetedObject = nearObjects[targetIndex];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.GetComponent<Loot>() != null || (other.gameObject.GetComponent<Hack>() != null && !other.gameObject.GetComponent<Hack>().hacked)) && !nearObjects.Contains(other.gameObject) && other.gameObject.activeSelf)
        {
            nearObjects.Add(other.gameObject);

            if (targetedObject == null)
                targetedObject = other.gameObject;

            CheckNearest();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.GetComponent<Hack>() != null || other.gameObject.GetComponent<Loot>() != null) && nearObjects.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            nearObjects.Remove(other.gameObject);
            CheckNearest();
        }

        if (nearObjects.Count == 0)
            targetedObject = null;
    }
}
