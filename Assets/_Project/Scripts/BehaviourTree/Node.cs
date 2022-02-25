using AI;

namespace BehaviourTree
{
    public abstract class Node
    {
        public Blackboard Blackboard;

        public Zombie Zombie;

        public NodeState State = NodeState.Running;

        public bool Started = false;

        public NodeState Update()
        {
            if (!Started)
            {
                OnStart();

                Started = true;
            }

            State = OnUpdate();

            if (State == NodeState.Failure || State == NodeState.Success)
            {
                OnStop();

                Started = false;
            }

            return State;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();
    }
}