using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPot : MonoBehaviour
{
    // how much candy this object has in total
    [SerializeField]
    int totalCandy = 30;

    // how much candy is left
    [HideInInspector]
    public int remaindingCandy = 0;

    // Duration needed to loot a single candy
    [SerializeField] float lootDuration = 1f;

    // Reduce this one
    float lootTick = 0.0f;

	// Use this for initialization
	void Start ()
    {
        remaindingCandy = totalCandy;
        lootTick = lootDuration;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (remaindingCandy == 0)
        {
            if (gameObject.GetComponent<AudioSource>())
                gameObject.GetComponent<AudioSource>().Stop();

            if (gameObject.GetComponentInChildren<ParticleSystem>())
                gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    public void Loot(TempPlayer player)
    {
        if (remaindingCandy <= 0)
            return;

        if (TempPlayer.useKeyboardInput)
        {
            if (lootTick > 0.0f)
            {
                lootTick -= Time.deltaTime;
            }
            else
            {
                if (gameObject.GetComponent<AudioSource>())
                    gameObject.GetComponent<AudioSource>().Play();
                else
                    print("No audio detected");

                lootTick = lootDuration;

                --remaindingCandy;

                print("1 candy taken");
                print(remaindingCandy + " candies are left");

                player.addCandyPoints(1);
            }
        }
        else
        {
            if (gameObject.GetComponent<AudioSource>())
                gameObject.GetComponent<AudioSource>().Play();
            else
                print("No audio detected");

            lootTick = lootDuration;

            --remaindingCandy;

            print("1 candy taken");
            print(remaindingCandy + " candies are left");

            player.addCandyPoints(1);
        }
    }
}
