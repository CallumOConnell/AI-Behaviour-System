using UnityEngine;

namespace BehaviourTree
{
    public class WaitNode : ActionNode
    {
        public float Duration = 1f;

        private float _startTime = 0f;

        protected override void OnStart()
        {
            _startTime = Time.time;

            Debug.Log("Started Wait");
        }

        protected override void OnStop()
        {
            Debug.Log($"Waited {Duration} seconds");
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time - _startTime > Duration)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}