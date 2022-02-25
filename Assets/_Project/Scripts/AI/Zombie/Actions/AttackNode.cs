using BehaviourTree;
using Player;
using UnityEngine;

namespace AI
{
    public class AttackNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart Attack");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop Attack");
        }

        protected override NodeState OnUpdate()
        {
            var playerStats = Blackboard.Target.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(Zombie.Stats.AttackDamage);

                Debug.Log("Zombie Attacked");

                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}
