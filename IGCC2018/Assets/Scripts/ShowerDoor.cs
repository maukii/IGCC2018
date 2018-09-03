using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerDoor : MonoBehaviour
{
    // Open state
    bool isOpen = false;

    // Default rotation
    float defaultRotation = 0.0f;

    // is rotating
    bool isRot = false;

    // rotate direction
    float rotDir = 0.0f;

    // open duration
    float openDuration = 3.0f;

    // duration ticking
    float openTick = 0.0f;

	// Use this for initialization
	void Start ()
    {
        defaultRotation = transform.rotation.eulerAngles.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isOpen)
        {
            openTick -= Time.deltaTime;

            if (openTick <= 0.0f)
            {
                openTick = 0.0f;
                isOpen = false;
                isRot = true;
                rotDir = 90.0f;
            }
        }

		if (isRot)
        {
            rotateDoor();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (openTick <= 0.0f)
        {
            openTick = openDuration;

            isOpen = true;
            isRot = true;

            rotDir = -90.0f;
        }
    }

    void rotateDoor()
    {
        transform.Rotate(Vector3.forward * rotDir * Time.deltaTime);

        isRot = (rotDir > 0) ?
            !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, defaultRotation)) <= 1.0f) :
            !(Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, (defaultRotation - 90.0f))) <= 1.0f);
    }
}
