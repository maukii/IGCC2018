using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameOver
{
    public static void GameEnded()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
