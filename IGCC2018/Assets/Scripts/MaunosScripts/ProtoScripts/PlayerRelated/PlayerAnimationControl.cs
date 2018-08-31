using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{

    Animator anim;
    bool useAnim;

    void Start()
    {
        anim.enabled = useAnim; 
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // do animation updating
    }
}
