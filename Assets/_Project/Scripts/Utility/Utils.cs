using UnityEngine;
using UnityEngine.AI;

namespace Utility
{
    public static class Utils
    {
        public static bool MoveAgent(NavMeshAgent agent, Vector3 moveToPosition)
        {
            if (agent != null)
            {
                var navMeshPath = new NavMeshPath();

                var foundPath = agent.CalculatePath(moveToPosition, navMeshPath);

                if (foundPath)
                {
                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.SetDestination(moveToPosition);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}