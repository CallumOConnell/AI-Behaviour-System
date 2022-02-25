using System.Collections.Generic;

namespace BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children = new List<Node>();
    }
}