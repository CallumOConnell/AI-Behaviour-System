namespace BehaviourTree
{
    public class InvertorNode : DecoratorNode
    {
        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override NodeState OnUpdate()
        {
            switch (Child.Update())
            {
                case NodeState.Running:
                {
                    return NodeState.Running;
                }

                case NodeState.Success:
                {
                    return NodeState.Failure;
                }

                case NodeState.Failure:
                {
                    return NodeState.Success;
                }
            }

            return NodeState.Failure;
        }
    }
}