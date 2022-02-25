using BehaviourTree;
using UnityEngine;

namespace AI
{
    public class ChaseNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart Chase");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop Chase");
        }

        protected override NodeState OnUpdate()
        {
            Zombie.Agent.SetDestination(Blackboard.Target.transform.position);

            Debug.Log("Zombie Moving To Player Position");

            return NodeState.Success;
        }
    }
}
