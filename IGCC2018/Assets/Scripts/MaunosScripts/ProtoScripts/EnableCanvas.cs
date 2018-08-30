using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCanvas : MonoBehaviour
{

    [SerializeField] Canvas canvas;

    private void Start()
    {
        canvas.gameObject.SetActive(true);
    }

}
