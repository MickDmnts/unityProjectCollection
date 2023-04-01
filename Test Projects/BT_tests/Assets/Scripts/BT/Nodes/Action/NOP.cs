namespace AI.BT
{

    public class NOP : INode
    {

        public bool Run()
        {

            return true;
        }

        public void OnReset()
        {
            //nop...
        }
    }
}