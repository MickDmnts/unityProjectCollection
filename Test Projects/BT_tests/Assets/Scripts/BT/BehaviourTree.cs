namespace AI.BT
{
    public class BehaviourTree : INode
    {
        private INode root;

        public BehaviourTree(INode root)
        {
            this.root = root;
        }

        public bool Run()
        {
            return root.Run();
        }

        public void OnReset()
        {
            root.OnReset();
        }
    }
}
