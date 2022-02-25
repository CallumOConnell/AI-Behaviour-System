using BehaviourTree;
using Perception;
using UnityEngine;

namespace AI
{
    public class CanSeePlayerNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart CanSeePlayer");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop CanSeePlayer");
        }

        protected override NodeState OnUpdate() => Zombie.TargetTrackingManager.GetResponse() == TargetResponses.Chase ? NodeState.Success : NodeState.Failure;
    }
}