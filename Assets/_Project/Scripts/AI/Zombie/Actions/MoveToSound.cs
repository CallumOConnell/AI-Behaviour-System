using BehaviourTree;
using UnityEngine;
using Utility;

namespace AI
{
    public class MoveToSound : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart MoveToSound");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop MoveToSound");
        }

        protected override NodeState OnUpdate() => Utils.MoveAgent(Zombie.Agent, Blackboard.SoundOrigin) ? NodeState.Success : NodeState.Failure;
    }
}