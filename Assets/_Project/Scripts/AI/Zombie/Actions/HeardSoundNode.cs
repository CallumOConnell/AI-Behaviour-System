using BehaviourTree;
using Perception;
using UnityEngine;

namespace AI
{
    public class HeardSoundNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart HeardSound");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop HeardSound");
        }

        protected override NodeState OnUpdate() => Zombie.TargetTrackingManager.GetResponse() == TargetResponses.Investigate ? NodeState.Success : NodeState.Failure;
    }
}