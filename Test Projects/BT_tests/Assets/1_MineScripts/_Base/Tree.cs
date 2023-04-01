using UnityEngine;

namespace BT
{
    public abstract class Tree : MonoBehaviour
    {
        Node root = null;

        // Start is called before the first frame update
        void Start()
        {
            root = SetupTree();
        }

        // Update is called once per frame
        void Update()
        {
            if (root != null)
            {
                root.Run();
            }
        }

        protected abstract Node SetupTree();
    }
}