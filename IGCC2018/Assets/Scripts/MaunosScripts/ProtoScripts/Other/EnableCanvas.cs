using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableCanvas : MonoBehaviour
{

    [SerializeField] Text candyCount;
    [SerializeField] GameObject canvas;

    [SerializeField] PlayerData player;

    private void Start()
    {
        canvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        candyCount.text = "Candy count: " + player.candyCount;
    }

}
