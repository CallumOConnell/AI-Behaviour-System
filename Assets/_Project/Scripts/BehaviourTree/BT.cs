using AI;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class BT
    {
        public Node RootNode;

        public Blackboard Blackboard = new Blackboard();

        public NodeState TreeState = NodeState.Running;

        public NodeState Update()
        {
            if (RootNode.State == NodeState.Running)
            {
                TreeState = RootNode.Update();
            }

            return TreeState;
        }

        public void Traverse(Node node, System.Action<Node> vistitor)
        {
            if (node != null)
            {
                vistitor.Invoke(node);

                var children = GetChildren(node);

                children.ForEach((childNode) => Traverse(childNode, vistitor));
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            var children = new List<Node>();

            if (parent is RootNode rootNode && rootNode.Child != null)
            {
                children.Add(rootNode.Child);
            }

            if (parent is DecoratorNode decorator && decorator.Child != null)
            {
                children.Add(decorator.Child);
            }

            if (parent is CompositeNode composite)
            {
                return composite.Children;
            }

            return children;
        }

        public void Bind(Zombie zombie)
        {
            Traverse(RootNode, node =>
            {
                node.Blackboard = Blackboard;
                node.Zombie = zombie;
            });
        }
    }
}