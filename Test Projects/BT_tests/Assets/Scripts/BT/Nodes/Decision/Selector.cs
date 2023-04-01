using System.Collections.Generic;

namespace AI.BT
{
    public class Selector : INode
    {
        private List<INode> children;

        public Selector(List<INode> children)
        {
            this.children = children;
        }

        public bool Run()
        {
            bool result = false;

            foreach (INode child in children)
            {
                if (child.Run())
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void OnReset()
        {
            //nop...
        }
    }
}