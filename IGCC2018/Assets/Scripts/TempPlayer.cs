using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public bool dead { get; set; }
    float gravity = -12;

    private int candyPoints = 0;

    // For raycasting the last object
    GameObject lastHitObject = null;

    // Last object's material
    Material lastObjectMaterial = null;

    // IoT Objects within hacking range
    List<GameObject> reachableIoT = new List<GameObject>();

    // Selected IoT to hack
    float selectedIndex = 0.0f;

    // Player respawn/spawn location
    Transform spawnPoint;

    // player lives
    int numLives = 3;

    // Total candy needed
    [SerializeField]
    int candyRequirement = 40;


    public static bool useKeyboardInput;

    [Header("Use if no device connected")]
    public bool DEBUG_useKeyboard;

    Gyroscope gyro;

    [Header("-- Variables --")]
    [SerializeField]
    bool useDebug = false;
    [SerializeField] bool hacking = false;
    [SerializeField] bool isFlat = true;

    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float wantedPhoneScreenAngle = 45f;
    [SerializeField] float minTiltRequired = 0.35f;
    public float currentSpeed;
    public float velocityY;

    [HideInInspector] public Vector3 tilt;

    Animator anim;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        gyro = Input.gyro;

        if (SystemInfo.supportsGyroscope && !DEBUG_useKeyboard)
        {
            Input.gyro.enabled = true;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            DEBUG_useKeyboard = true;
            useKeyboardInput = true;
        }

        Debug.Log(SystemInfo.supportsGyroscope ? "Supports gyroscope" : "No gyroscope support");
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject[] spawnArray = GameObject.FindGameObjectsWithTag("Respawn");
        spawnPoint = spawnArray[0].transform;
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    //private void Awake()
    //{
    //    if (pInstance == null)
    //    {
    //        DontDestroyOnLoad(this);
    //        pInstance = this;
    //    }
    //    else
    //    {
    //        DestroyObject(gameObject);
    //    }
    //}

    public Vector3 GetTilt()
    {
        return tilt;
    }


    void Update()
    {
        // actually more responsive if does movement before anything else O.o
        DoMovement();

        // This turns the material of objects that are in the way of the camera viewing the player to translucent
        //hideObjects();

        // Detects for IoT objects within a sphere radius
        detectObjects();

        // Player selects which IoT he wants to hack here
        selectIoT();

        // Hack the selected IoT, if possible
        if (Input.GetKeyDown("space"))
        {
            if (reachableIoT.Count > 0)
            {
                /// so many ifs oh god
                if (gameObject.GetComponent<BatteryCharge>().CanHack(20.0f))
                    if (reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Hack())
                        gameObject.GetComponent<BatteryCharge>().DrainBattery(20.0f);

                //foreach (GameObject obj in reachableIoT)
                //    obj.GetComponent<IoTBaseObj>().Hack();
            }
        }


        if (useDebug)
        {
            Debug.DrawRay(transform.position + Vector3.up, tilt, Color.green);
        }
    }

    private void DoMovement()
    {
        if (useKeyboardInput)
            KeyboardMovement();
        else
            MobileMovement();
    }

    private void UpdateAnimator()
    {
        float animSpeed;
        animSpeed = currentSpeed;
        animSpeed = Mathf.Clamp(animSpeed, 0, 1);
        anim.SetFloat("speed", animSpeed);
    }

    private void KeyboardMovement()
    {
        tilt = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        #region Acceleration calculations
        
        if (Mathf.Abs(tilt.magnitude) != 0)
        {
            if (currentSpeed <= walkSpeed)
            {
                currentSpeed += Time.deltaTime * walkSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= Time.deltaTime * walkSpeed;
            }
            else
            {
                currentSpeed = 0f;
            }
        }
        #endregion

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        cc.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(cc.velocity.x, cc.velocity.z).magnitude;

        if (cc.isGrounded)
        {
            velocityY = 0f;
        }

        UpdateAnimator();

    }

    private void MobileMovement()
    {
        tilt = Input.acceleration;

        if (isFlat)
            tilt = Quaternion.Euler(wantedPhoneScreenAngle, 0, 0) * tilt;

        #region Acceleration calculations
        if (Mathf.Abs(tilt.y) > minTiltRequired || Mathf.Abs(tilt.x) > minTiltRequired - 0.15)
        {
            if (currentSpeed < walkSpeed)
            {
                currentSpeed += Time.deltaTime * walkSpeed;
            }
            if (cc.isGrounded)
            {
                velocityY = 0f;
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= Time.deltaTime * walkSpeed;
            }
            else if (currentSpeed <= 0)
            {
                currentSpeed = 0;
            }
        }
        #endregion

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        cc.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(cc.velocity.x, cc.velocity.z).magnitude;

        if (cc.isGrounded)
        {
            velocityY = 0f;
        }

        UpdateAnimator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            --numLives;

            if (numLives <= 0)
            {
                // dead screen or something??
                // TODO: load to main menu/death screen
                print("Actually dead");
            }

            Respawn();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CandyPot") && Input.GetKey(KeyCode.F))
        {
            //float distance = other.ClosestPoint(transform.position).magnitude;

            //if (distance > 2.5f)
            //    return;

            other.GetComponentInParent<CandyPot>().Loot(this);

            if (candyPoints >= candyRequirement)
            {
                // blast confetti or something
                // TODO: WIN
            }
        }
    }

    public void addCandyPoints(int points)
    {
        candyPoints += points;

        print("Gained " + points + " candies");
        print("Total of: " + candyPoints + " candies");
    }

    void hideObjects()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.Equals(lastHitObject) || !hit.collider.gameObject.GetComponent<MeshRenderer>())
                return;

            // If hitting something new or the player, set the last hit object back to its original material
            if (!hit.collider.gameObject.Equals(lastHitObject))
            {
                // Null check
                if (lastHitObject)
                {
                    lastHitObject.GetComponent<MeshRenderer>().material = lastObjectMaterial;
                    lastHitObject = null;
                }

                if (hit.collider.gameObject.CompareTag("Player"))
                    return;
            }

            lastHitObject = hit.collider.gameObject;
            lastObjectMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;

            Material newMat = Resources.Load("Materials/Translucent", typeof(Material)) as Material;
            hit.collider.gameObject.GetComponent<MeshRenderer>().material = newMat;
        }
    }

    void detectObjects()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 4);

        List<Collider> detectedList = new List<Collider>(detectedObjects);

        // Add IoT objects into hack list
        foreach (Collider coll in detectedList)
        {
            // If it is not an IoT (hackable object), ignore it
            if (!coll.gameObject.CompareTag("IoT"))
                continue;

            // If the list already contains the object, don't add it.
            if (!reachableIoT.Contains(coll.gameObject))
                reachableIoT.Add(coll.gameObject);
        }

        // List of things to remove
        List<GameObject> toRemove = new List<GameObject>();

        // Remove IoT objects that are no longer range from hack list
        foreach (GameObject obj in reachableIoT)
        {
            if (!detectedList.Contains(obj.GetComponent<Collider>()))
                toRemove.Add(obj);
        }

        // Remove objects that are no longer in range
        foreach (GameObject obj in toRemove)
        {
            reachableIoT.Remove(obj);
        }

        // Empty the lists
        detectedList.Clear();
        toRemove.Clear();
    }

    void selectIoT()
    {
        // If there's nothing to hack...
        if (reachableIoT.Count <= 0)
        {
            //print("Nothing to hack");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            --selectedIndex;

        if (Input.GetKeyDown(KeyCode.E))
            ++selectedIndex;

        selectedIndex = Mathf.Repeat(selectedIndex, reachableIoT.Count);

        //print(Mathf.FloorToInt(selectedIndex));

        //print(reachableIoT[Mathf.FloorToInt(selectedIndex)]);

        // NOTE: delete this if check later.
        if (reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>())
            reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Selected();
    }

    public String getSelectedIoT()
    {
        if (reachableIoT.Count <= 0)
            return "Nothing Selected";

        return reachableIoT[Mathf.FloorToInt(selectedIndex)].gameObject.name;
    }

    void Respawn()
    {
        // Set respawn point
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    public int getLives()
    {
        return numLives;
    }
}
