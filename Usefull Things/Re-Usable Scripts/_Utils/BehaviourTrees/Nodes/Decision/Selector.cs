using System.Collections.Generic;

namespace AI.BT {

    public class Selector: INode {

        private INode[] children;

        public Selector(INode[] children) {
            this.children = children;
        }

        public bool Run() {

            bool result = false;

            foreach (INode child in children) {

                if (child.Run()) {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}