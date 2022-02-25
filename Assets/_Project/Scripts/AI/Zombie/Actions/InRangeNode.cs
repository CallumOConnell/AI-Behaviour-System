using BehaviourTree;
using UnityEngine;

namespace AI
{
    public class InRangeNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart InRange");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop InRange");
        }

        protected override NodeState OnUpdate()
        {
            var target = Blackboard.Target;

            var zombie = Zombie.gameObject;

            if (target != null && zombie != null)
            {
                var distanceToTarget = Vector3.Distance(target.transform.position, zombie.transform.position);

                return Zombie.Stats.AttackRange >= distanceToTarget ? NodeState.Success : NodeState.Failure;
            }

            return NodeState.Failure;
        }
    }
}