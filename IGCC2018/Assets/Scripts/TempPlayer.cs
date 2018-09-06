using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempPlayer : MonoBehaviour
{
    public static bool playerIsDead;
    Animator anim;
    CharacterController cc;


    #region Other variables

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

    // Invulnerability on respawn
    float invulnDuration = 3.0f;

    float invulnTick = 3.0f;

    #endregion

    #region Player variables

    public static bool useKeyboardInput;
    Gyroscope gyro;

    [Header("Use if no device connected")]
    public bool DEBUG_useKeyboard;

    [Header("-- Variables --")]
    [SerializeField] bool useDebug = false;
    [SerializeField] bool isFlat = true;
    bool hacking = false;
    public bool closeCandy;

    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float wantedPhoneScreenAngle = 45f;
    [SerializeField] float minTiltRequired = 0.35f;
    float currentSpeed;
    float velocityY;

    [HideInInspector] public Vector3 tilt;

    GameObject candyPot;

    #endregion

    #region UI stuff

    [SerializeField] Sprite[] actionSprites = new Sprite[2];
    [SerializeField] Image activeImage;

    #endregion



    private void OnEnable()
    {
        playerIsDead = false;
    }

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


        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);


        Debug.Log(SystemInfo.supportsGyroscope ? "Supports gyroscope" : "No gyroscope support");
        activeImage.sprite = actionSprites[0];

        AudioManager.instance.PlaySound("Music");
    }

    private void OnLevelWasLoaded(int level)
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    } // this is absolite

    void Update()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // actually more responsive if does movement before anything else O.o
        if (!playerIsDead)
        {
            DoMovement();
            UpdateUI();

            detectObjects();

            selectIoT();

            // Hack the selected IoT, if possible
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (reachableIoT.Count > 0)
                {
                /// so many ifs oh god
                    if (gameObject.GetComponent<BatteryCharge>().CanHack(20.0f))
                        if (reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Hack())
                        {   
                            gameObject.GetComponent<BatteryCharge>().DrainBattery(20.0f);
                            anim.SetTrigger("Hack");
                        }

                //foreach (GameObject obj in reachableIoT)
                //    obj.GetComponent<IoTBaseObj>().Hack();
                }
            }

            if (invulnTick > 0.0f)
                invulnTick -= Time.deltaTime;
        }
    }

    private void UpdateUI()
    {
        if(closeCandy && candyPot.GetComponent<CandyPot>().remaindingCandy > 0)
        {
            activeImage.sprite = actionSprites[1];
        }
        else
        {
            activeImage.sprite = actionSprites[0]; 
        }
    }

    public void Action()
    {
        if (reachableIoT.Count > 0 && !closeCandy)
        {
            /// so many ifs oh god
            if (gameObject.GetComponent<BatteryCharge>().CanHack(20.0f))
            {
                if (reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Hack())
                {
                    gameObject.GetComponent<BatteryCharge>().DrainBattery(20.0f);
                    anim.SetTrigger("Hack");
                }
            }
        }

        if (closeCandy && candyPot != null)
        {
            candyPot.GetComponent<CandyPot>().Loot(this);
        }
    }

    public void NextHackable()
    {
        if (reachableIoT.Count <= 0)
        {
            return;
        }

        ++selectedIndex;
        selectedIndex = Mathf.Repeat(selectedIndex, reachableIoT.Count);
    }

    public void PreviousHackable()
    {
        if (reachableIoT.Count <= 0)
        {
            return;
        }

        --selectedIndex;
        selectedIndex = Mathf.Repeat(selectedIndex, reachableIoT.Count);
    }

    private void DoMovement()
    {
        if(!playerIsDead)
        {
            if (useKeyboardInput)
                KeyboardMovement();
            else
                MobileMovement();
        }
    }

    Vector3 velocity;

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

        velocityY += Time.deltaTime * gravity;
        velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        cc.Move(velocity * Time.deltaTime);
        //currentSpeed = new Vector2(cc.velocity.x, cc.velocity.z).magnitude;

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

    public void Die()
    {
        Debug.Log("player die");
        anim.SetTrigger("Die");
        anim.SetBool("dead", true);
        StartCoroutine(DieLoop());
    }

    private IEnumerator DieLoop()
    {
        --numLives;

        if (numLives > 0)
        {
            AudioManager.instance.PlaySound("Howl");
            AudioManager.instance.PlaySound("Scream");
        }
        else
        {
            AudioManager.instance.PlaySound("ScreamLong"); // mby don't use this
        }

        yield return new WaitForSeconds(2);

        if (numLives <= 0)
        {
            // dead screen or something??
            // TODO: load to main menu/death screen
            AudioManager.instance.StopSound("Music");
            print("Actually dead");
            LevelChanger.instance.FadeToLevel(5);
        }
        else
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CandyPot"))
        {
            closeCandy = true;
            candyPot = other.gameObject;
        }

        //if (other.CompareTag("Ghost"))
        //{
        //    if (invulnTick > 0.0f)
        //        return;
        //
        //    --numLives;
        //
        //    if (numLives <= 0)
        //    {
        //        // dead screen or something??
        //        playerIsDead = true;
        //        UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScreen");
        //    }
        //
        //    Respawn();
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            if (invulnTick > 0.0f)
                return;

            --numLives;

            if (numLives <= 0)
            {
                // dead screen or something??
                playerIsDead = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScreen");
            }

            Respawn();
        }

        if (other.CompareTag("CandyPot") && Input.GetKey(KeyCode.F))
        {
            //float distance = other.ClosestPoint(transform.position).magnitude;

            //if (distance > 2.5f)
            //    return;

            other.GetComponentInParent<CandyPot>().Loot(this);
            
            if(other.GetComponentInParent<CandyPot>().remaindingCandy > 0)
                anim.SetTrigger("Loot");

            if (candyPoints >= candyRequirement)            
            {
                // blast confetti or something
                // TODO: WIN
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CandyPot"))
        {
            closeCandy = false;
            candyPot = null;
        }
    }

    public void addCandyPoints(int points)
    {
        candyPoints += points;
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

        if (reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>())
            reachableIoT[Mathf.FloorToInt(selectedIndex)].GetComponent<IoTBaseObj>().Selected();
    }

    public String getSelectedIoT()
    {
        if (reachableIoT.Count <= 0)
            return "Nothing Selected";

        return reachableIoT[Mathf.FloorToInt(selectedIndex)].gameObject.name;
    }

    public void Respawn()
    {
        invulnTick = invulnDuration;

        // Set respawn point
        //transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        anim.SetBool("dead", false);
        playerIsDead = false;
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        GameObject.Find("Enemy").GetComponentInChildren<EnemyKillPlayer>().Respawn();
    }

    public int getLives()
    {
        return numLives;
    }

    public int getCandies()
    {
        return candyPoints;
    }

    public int getRequirement()
    {
        return candyRequirement;
    }

    public Vector3 GetTilt()
    {
        return tilt;
    }

    // this... isnt used at all
    private void Reset()
    {
        // Total candies
        candyPoints = 0;

        // For raycasting the last object
        lastHitObject = null;

        // Last object's material
        lastObjectMaterial = null;

        // player lives
        numLives = 3;

        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        playerIsDead = false;
    }

    void OnGUI()
    {
        if(useDebug)
        {
            GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + gyro.rotationRate);
            GUI.Label(new Rect(500, 350, 200, 40), "Gyro attitude" + gyro.attitude);
            GUI.Label(new Rect(500, 400, 200, 40), "Gyro enabled : " + gyro.enabled);
            GUI.Label(new Rect(500, 450, 200, 40), "Tilt : " + tilt);
            GUI.Label(new Rect(500, 500, 200, 40), "Velocity : " + velocity);
        }
    }

}
