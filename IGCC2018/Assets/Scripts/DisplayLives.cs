using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLives : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image hpIcon;

    List<UnityEngine.UI.Image> hpList = new List<UnityEngine.UI.Image>();

    [SerializeField]
    TempPlayer player;

    // Use this for initialization
    void Start ()
    {
        hpList.Add(hpIcon);
        float offset = 110.0f;

        for (int i = 1; i < player.getLives(); ++i)
        {
            print("loop");

            UnityEngine.UI.Image dupe = GameObject.Instantiate(hpIcon, transform);

            dupe.transform.Translate(new Vector3(offset, 0.0f, 0.0f));

            hpList.Add(dupe);

            offset += offset;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If player loses a life
        if (player.getLives() < hpList.Count)
        {
            Destroy(hpList[hpList.Count - 1]);
            hpList.RemoveAt(hpList.Count - 1);
        }
    }
}
