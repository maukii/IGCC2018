using UnityEngine;
using UnityEngine.AI;

public class Chase : BotState
{
    Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    // Execute chaseState
    public override void Execute(Bot bot)
    {
        // Update target transform
        NavMeshAgent agent = bot.Agent;
        agent.destination = _target.position;
    }

}
