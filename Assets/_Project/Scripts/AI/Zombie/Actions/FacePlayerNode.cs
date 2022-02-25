using BehaviourTree;
using UnityEngine;

namespace AI
{
    public class FacePlayerNode : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("OnStart FacePlayer");
        }

        protected override void OnStop()
        {
            Debug.Log("OnStop FacePlayer");
        }

        protected override NodeState OnUpdate()
        {
            var lookRotation = Quaternion.LookRotation(Blackboard.Target.transform.position - Zombie.transform.position).eulerAngles;

            lookRotation.x = 0;
            lookRotation.z = 0;

            var lookAtRotation = Quaternion.Euler(lookRotation);
            var moveRotation = Quaternion.Slerp(Zombie.transform.rotation, lookAtRotation, 2f * Time.deltaTime).eulerAngles;

            moveRotation.x = 0;
            moveRotation.z = 0;

            Zombie.transform.rotation = Quaternion.Euler(moveRotation);

            return NodeState.Success;
        }
    }
}