namespace AI.BT
{
    public abstract class Decorator : INode
    {
        protected INode child;

        public Decorator(INode child)
        {
            this.child = child;
        }

        public virtual bool Run() { return false; }

        public void OnReset()
        {
            //nop...
        }
    }
}