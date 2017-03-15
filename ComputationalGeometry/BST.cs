using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class BST<T> : BinTree<T>
    {
        public IComparer<T> comparer;
        public BST(IComparer<T> comparer){ this.comparer = comparer; }
        public BinNode<T> Search(T e) {
            return SearchIn(e, Root);
        }
        public BinNode<T> SearchIn(T e, BinNode<T> node) {
            while(node != null && comparer != null) {
                int comRet = comparer.Compare(e, node.data);
                if (comRet == 0)
                    return node;
                else if (comRet < 0 && node.HasLC())
                    node = node.LC;
                else if (comRet > 0 && node.HasRC())
                    node = node.RC;
                else
                    return node;
            }
            return null;
        }
        public virtual BinNode<T> Insert(T e) {
            var n = Search(e);
            if (n == null) {
                return InsertAsRoot(e);
            } else {
                BinNode<T> ret = null;
                int comRet = comparer.Compare(e, n.data);
                if (comRet == 0)
                    return n;
                else if (comRet < 0)
                    ret = InsertAsLC(n, e);
                else
                    ret = InsertAsRC(n, e);
                UpdateHeightAbove(ret);
                return ret;
            }
        }
        public virtual BinNode<T> Remove(T e) {
            var n = Search(e);
            if(n != null && comparer.Compare(n.data, e) == 0) {
                var ret = RemoveAt(n);
                UpdateHeightAbove(ret);
                Size -= 1;
                return ret;
            }
            return null;
        }
        public BinNode<T> RemoveAt(BinNode<T> n) {
            BinNode<T> ret = null;
            if (n.HasRC() && n.HasLC()) {
                var succ = n.Succ;
                ret = succ.Parent;
                var temp = succ.data;
                succ.data = n.data;
                n.data = temp;
                var succParent = succ.Parent;
                if (succParent == n)
                    succParent.RC = succ.RC;
                else
                    succParent.LC = succ.RC;
                if (succ.RC != null)
                    succ.RC.Parent = succParent;
            }else {
                ret = n.Parent;
                var child = n.HasLC() ? n.LC : n.RC;
                if (child != null)
                    child.Parent = n.Parent;
                if (n.IsRoot())
                    this.Root = child;
                else if (n.IsLC())
                    n.Parent.LC = child;
                else
                    n.Parent.RC = child;
            }
            return ret;
        }

        public BinNode<T> Connect34(BinNode<T> a, BinNode<T> b, BinNode<T> c,
                    BinNode<T> st1, BinNode<T> st2, BinNode<T> st3, BinNode<T> st4) {
            a.LC = st1;
            if (st1 != null)
                st1.Parent = a;
            a.RC = st2;
            if (st2 != null)
                st2.Parent = a;
            UpdateHeight(a);
            c.LC = st3;
            if (st3 != null)
                st3.Parent = c;
            c.RC = st4;
            if (st4 != null)
                st4.Parent = c;
            UpdateHeight(c);
            b.LC = a;
            a.Parent = b;
            b.RC = c;
            c.Parent = b;
            UpdateHeight(b);
            return b;
        }

        public BinNode<T> RotateAt(BinNode<T> n) {
            var p = n.Parent;
            var g = p.Parent;
            if (p.IsLC()) {//zig
                if(n.IsLC()) {//zig-zag
                    p.Parent = g.Parent;
                    return Connect34(n, p, g, n.LC, n.RC, p.RC, g.RC);
                }else {
                    n.Parent = g.Parent;
                    return Connect34(p, n, g, p.LC, n.LC, n.RC, g.RC);
                }
            }else {//zag
                if(n.IsRC()) {//zag-zig
                    p.Parent = g.Parent;
                    return Connect34(g, p, n, g.LC, p.LC, n.LC, n.RC);
                }else {
                    n.Parent = g.Parent;
                    return Connect34(g, n, p, g.LC, n.LC, n.RC, p.RC);
                }
            }
        }
        public BinNode<T> RotateRight(BinNode<T> n) {
            if (n == null)
                return null;
            bool isRoot = n.IsRoot();
            var p = n.RotateRight();
            if (isRoot)
                Root = p;
            return p;
        }
        public BinNode<T> RotateLeft(BinNode<T> n) {
            if (n == null)
                return null;
            bool isRoot = n.IsRoot();
            var p = n.RotateLeft();
            if (isRoot)
                Root = p;
            return p;
        }
    }
}
