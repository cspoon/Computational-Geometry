using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class AVL<T> : BST<T>
    {
        public AVL(IComparer<T> comparer) : base(comparer){ }
        void Rotated(BinNode<T> n) {
            var type = n.NodeType;
            var currParent = n.Parent;
            var rotatedRoot = RotateAt(n.TallerChild().TallerChild());
            if (type == BinNode<T>.Type.Root)
                Root = rotatedRoot;
            else if (type == BinNode<T>.Type.LC)
                currParent.LC = rotatedRoot;
            else
                currParent.RC = rotatedRoot;
        }
        public override BinNode<T> Insert(T e) {
            var g = base.Insert(e);
            var curr = g;
            while(curr != null && !curr.IsRoot()) {
                curr = curr.Parent;
                if(!curr.AVLBalanced()) {
                    Rotated(curr);
                } else {
                    UpdateHeight(curr);
                }
            }
            return g;
        }
        public override BinNode<T> Remove(T e) {
            var p = Search(e);
            if(p != null && p.data.Equals(e)) {
                return Remove(p);
            }
            return null;
        }
        public BinNode<T> Remove(BinNode<T> n) {
            if (n != null) {
                var g = RemoveAt(n);
                var curr = g;
                Size -= 1;
                while (curr != null) {
                    if (!curr.AVLBalanced())
                        Rotated(curr);
                    UpdateHeight(curr);
                    curr = curr.Parent;
                }
                return g;
            }
            return null;
        }
    }
}
