using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBack : MonoBehaviour
{
    public bool fading = false;
    Renderer rend;

    public float fadeSpeed = 0.05f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void FadeWallBack()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (float f = fadeSpeed; f <= 1; f += fadeSpeed)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        fading = false;
    }

}
