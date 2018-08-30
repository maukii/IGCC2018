using UnityEngine;
using UnityEngine.AI;

public class Search : BotState
{
    Transform _target;
    const float DISTANCE_TO_KEEP = 1.0f;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    // Execute searchState
    public override void Execute(Bot bot)
    {
        NavMeshAgent agent = bot.Agent;

        agent.destination = _target.position;

        if (!agent.pathPending && agent.remainingDistance < DISTANCE_TO_KEEP)
        {

        }

    }

}
