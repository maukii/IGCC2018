using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField]
    string nextLevel;

    [SerializeField]
    bool noCollide = false;

    [SerializeField]
    float delay = 10.0f;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (noCollide)
        {
            delay -= Time.deltaTime;

            if (delay <= 0.0f)
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        print(other);

        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
