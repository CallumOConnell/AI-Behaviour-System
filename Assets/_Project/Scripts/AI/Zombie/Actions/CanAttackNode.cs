using BehaviourTree;
using UnityEngine;

namespace AI
{
    public class CanAttackNode : ActionNode
    {
        private float _timeToNextAttack = 0f;

        protected override void OnStart()
        {
            Debug.Log("OnStart CanAttack");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop CanAttack");
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time >= _timeToNextAttack)
            {
                _timeToNextAttack = Time.time + 1f / Zombie.Stats.AttackRate;

                Debug.Log("Zombie Can Attack");

                return NodeState.Success;
            }

            Debug.Log("Zombie Can't Attack");

            return NodeState.Failure;
        }
    }
}