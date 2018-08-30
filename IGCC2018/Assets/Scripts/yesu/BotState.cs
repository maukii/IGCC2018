using UnityEngine;
using UnityEngine.AI;


// Bot State Base Class
public abstract class BotState
{
    public virtual void Execute(Bot bot) { }
}
