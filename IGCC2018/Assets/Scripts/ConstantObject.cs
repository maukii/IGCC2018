using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
