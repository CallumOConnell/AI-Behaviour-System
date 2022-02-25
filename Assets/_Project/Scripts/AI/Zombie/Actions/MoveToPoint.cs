using BehaviourTree;
using UnityEngine;
using Utility;

namespace AI
{
    public class MoveToPoint : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart MoveToPoint");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop MoveToPoint");
        }

        protected override NodeState OnUpdate() => Utils.MoveAgent(Zombie.Agent, Blackboard.RoamPosition) ? NodeState.Success : NodeState.Failure;
    }
}