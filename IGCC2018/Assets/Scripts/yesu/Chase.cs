using UnityEngine;
using UnityEngine.AI;

public class Chase : BotState
{
    Transform _target;

    private float _detectingRange;

    IoTBaseObj _iotTrget = null;

    Bot _botRef;

    private const int RAY_HEIGHT = 5;



    public void Init(Transform target, float detecingtrange)
    {
        _detectingRange = detecingtrange;
        _target = target;
    }

    // Execute chaseState
    public override void Execute(Bot bot)
    {
        _botRef = bot;

        // Update target transform
        NavMeshAgent agent = bot.Agent;
        agent.destination = _target.position;


        // Open the door the bot found during tracking the player
        SearchDoor(agent);

        //if (_iotTrget)
        //{
        //    Vector3 distance = _iotTrget.transform.position - agent.transform.position;
        //    if (Mathf.Abs(distance.magnitude) <= 1.0f)
        //    {
        //    }
        //}
    }


    void SearchDoor(NavMeshAgent agent)
    {

        // ray reset
        Vector3 rayPos = agent.transform.position;
        rayPos.y = rayPos.y + RAY_HEIGHT;
        Ray ray = new Ray(rayPos, agent.transform.up * -1);

        // ray casting to around Objects
        RaycastHit[] hitinfos = Physics.SphereCastAll(ray, _detectingRange/4);

        // if detect hackable object change target
        foreach (var hitinfo in hitinfos)
        {
            // detect "IoT" object by tag
            GameObject hitObj = hitinfo.transform.gameObject;
            _iotTrget = hitObj.GetComponent<IoTBaseObj>();

            if (_iotTrget != null && _iotTrget.GetActivated())
            {
                // Check the type of the IoT.
                if (_iotTrget.GetIoTType() == "Door")
                {
                    _iotTrget.Disable();
                }
            }
        }
    }

}
