using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public bool fadeOut;
    RaycastHit hit;

    public float fadeSpeed = 0.05f;

    [SerializeField] GameObject wallParent;
    public Renderer rend;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                wallParent = hit.collider.gameObject;
                rend = wallParent.GetComponent<Renderer>();

                if(!fadeOut && wallParent != null && rend != null)
                {
                    fadeOut = true;
                    wallParent.GetComponent<FadeBack>().fading = true;
                    StartCoroutine(StartFadeOut());
                }
            }
            else
            {
                if (wallParent != null)
                {
                    wallParent.GetComponent<FadeBack>().FadeWallBack();
                }
                wallParent = null;
                rend = null;
                fadeOut = false;
            }
        }
    }

    IEnumerator StartFadeOut()
    {
        for (float f = 1f; f >= -fadeSpeed; f -= fadeSpeed)
        {
            if(wallParent != null && rend != null)
            {
                Color c = rend.material.color;
                c.a = f;
                rend.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        }

        if(wallParent != null)
        {
            wallParent.GetComponent<FadeBack>().fading = false;
        }
    }

}
