using System.Collections.Generic;

namespace BT
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE,
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        Dictionary<string, object> dataContext = new Dictionary<string, object>();

        public Node()
        {
            this.parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }

        void Attach(Node node)
        {
            node.parent = this;
            node.children.Add(node);
        }

        public virtual NodeState Run()
        {
            return NodeState.FAILURE;
        }

        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;

            if (dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node tempNode = parent;
            while (tempNode != null)
            {
                value = tempNode.GetData(key);
                if (value != null)
                {
                    return value;
                }

                tempNode = tempNode.parent;
            }

            return null;
        }
        public bool ClearData(string key)
        {
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
}