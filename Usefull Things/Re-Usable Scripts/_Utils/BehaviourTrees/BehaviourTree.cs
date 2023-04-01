namespace AI.BT {

    public class BehaviourTree {

        private INode root;

        public BehaviourTree(INode root) {
            this.root = root;
        }

        public bool Run() {
            return root.Run();
        }
    }
}
