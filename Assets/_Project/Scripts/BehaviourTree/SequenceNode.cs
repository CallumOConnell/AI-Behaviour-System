using UnityEngine;

namespace BehaviourTree
{
    public class SequenceNode : CompositeNode
    {
        private int _current;

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
                    _current++;
                    break;
                }
                
                case NodeState.Failure:
                {
                    return NodeState.Failure;
                }
            }

            return _current == Children.Count ? NodeState.Success : NodeState.Running;
        }
    }
}