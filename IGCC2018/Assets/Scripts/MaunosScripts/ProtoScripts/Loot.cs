using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Lootable lootable;
    public PlayerData player;

    [SerializeField] float value;
    [SerializeField] int candyLeft;

    private void Awake()
    {
        SetOrginalValues();

        player.candyCount = 0;
        lootable.candyLeft = 5;
    }

    private void SetOrginalValues()
    {
        value = lootable.value;
        candyLeft = lootable.candyLeft;
    }

    public void GiveCandy()
    {
        player.candyCount += lootable.value;
        candyLeft--;

        if(candyLeft <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
