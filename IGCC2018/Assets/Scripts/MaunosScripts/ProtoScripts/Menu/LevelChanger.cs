using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    public static LevelChanger instance = null;
    [SerializeField] Animator anim;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (instance == this)
            DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        anim.ResetTrigger("FadeOut");

        if (level != 0) // "" if level != mainmenu
            anim.SetTrigger("FadeIn");
    }

    public void FadeToLevel(int index)
    {
        anim.SetInteger("level", index);
        anim.SetTrigger("FadeOut");
    }

    public void FadeOutComplite()
    {
        SceneManager.LoadScene(anim.GetInteger("level"));
    }

}