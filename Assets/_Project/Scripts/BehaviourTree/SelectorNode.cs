namespace BehaviourTree
{
    public class SelectorNode : CompositeNode
    {
        private int _current = 0;

        protected override void OnStart() => _current = 0;

        protected override void OnStop() { }

        protected override NodeState OnUpdate()
        {
            var child = Children[_current];

            switch (child.Update())
            {
                case NodeState.Running:
                {
                    return NodeState.Running;
                }

                case NodeState.Success:
                {
                    return NodeState.Success;
                }

                case NodeState.Failure:
                {
                    _current++;
                    break;
                }
            }

            return _current == Children.Count ? NodeState.Failure : NodeState.Running;
        }
    }
}