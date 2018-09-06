using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuController : MonoBehaviour
{

    public void Play()
    {
        LevelChanger.instance.FadeToLevel(1); // change to right level 1 index
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
