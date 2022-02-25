using BehaviourTree;
using UnityEngine;

namespace AI
{
    public class IdleNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart Idle");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop Idle");
        }

        protected override NodeState OnUpdate()
        {
            // Have multiple idle animations that are randomly selected from?

            return NodeState.Success;
        }
    }
}
