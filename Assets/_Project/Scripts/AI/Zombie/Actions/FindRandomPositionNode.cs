using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class FindRandomPositionNode : ActionNode
    {
        public float Radius = 5f;

        public int Attempts = 30;

        protected override void OnStart()
        {
            Debug.Log("OnStart FindRandomPosition");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop FindRandomPosition");
        }

        protected override NodeState OnUpdate()
        {
            if (FindRandomPosition(Zombie.Agent.transform.position, Radius, out Vector3 temp))
            {
                Debug.Log("Found random position in world");

                Blackboard.RoamPosition = temp;

                return NodeState.Success;
            }

            return NodeState.Failure;
        }

        private bool FindRandomPosition(Vector3 center, float range, out Vector3 result)
        {
            for (var i = 0; i < Attempts; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * range;

                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    result = hit.position;

                    return true;
                }
            }

            result = center;

            return false;
        }
    }
}