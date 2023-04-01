using System.Collections.Generic;

namespace AI.BT
{
    public class Parallel : INode
    {
        List<INode> children;

        public Parallel(List<INode> children)
        {
            this.children = children;
        }

        public bool Run()
        {
            foreach (INode child in children)
            {
                return child.Run();
            }

            return false;
        }

        public void OnReset()
        {
            //nop...
        }
    }
}