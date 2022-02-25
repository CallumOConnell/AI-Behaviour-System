namespace BehaviourTree
{
    public class RootNode : Node
    {
        public Node Child;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override NodeState OnUpdate() => Child.Update();
    }
}