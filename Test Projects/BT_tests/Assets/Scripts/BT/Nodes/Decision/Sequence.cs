using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BT
{
    public class Sequence : INode
    {
        List<INode> children;

        public Sequence(List<INode> children)
        {
            this.children = children;
        }

        public bool Run()
        {
            foreach (INode child in children)
            {
                if (!child.Run())
                {
                    ResetChildren();
                    return false;
                }
            }

            return true;
        }

        void ResetChildren()
        {
            foreach (INode child in children)
            {
                child.OnReset();
            }
        }


        public void OnReset()
        {
            ResetChildren();
        }
    }
}