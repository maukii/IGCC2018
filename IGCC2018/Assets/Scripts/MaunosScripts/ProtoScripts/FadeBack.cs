using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBack : MonoBehaviour
{
    public bool fading = false;

    Renderer rend;
    Color color;
    public float timeBetweenTicks = 0.05f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void FadeWallBack()
    {
        color = rend.material.color;
        Debug.Log("fade back");
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        fading = false;
    }

}
