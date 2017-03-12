using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public static class BSTUtils
    {
        public static int Stature<T>(this BinNode<T> node) {
            return node != null ? node.Height : -1;
        }
    }

    public class BinNode<T>
    {
        public enum Type
        {
            Root = 0,
            LC = 1,
            RC = -1
        }
        public T data;
        private BinNode<T> parent;
        private BinNode<T> lc;
        private BinNode<T> rc;
        int height;

        public int Height {get {return height;}set { height = value; } }

        public BinNode<T> LC {
            get {return lc;}
            set {lc = value;}
        }
        public BinNode<T> RC {
            get {return rc;}
            set {rc = value;}
        }
        public BinNode<T> Parent {
            get {return parent;}
            set {parent = value;}
        }
        public BinNode<T> Succ {
            get {
                BinNode<T> ret = this;
                if (HasRC()) {
                    ret = ret.RC;
                    while (ret.HasLC())
                        ret = ret.LC;
                } else {
                    while (ret.IsRC())
                        ret = ret.Parent;
                    ret = ret.Parent;
                }
                return ret;
            }
        }
        public BinNode<T> Pred {
            get {
                BinNode<T> ret = this;
                if (HasLC()) {
                    ret = ret.LC;
                    while (ret.HasRC())
                        ret = ret.RC;
                } else {
                    while (ret.IsLC())
                        ret = ret.Parent;
                    ret = ret.Parent;
                }
                return ret;
            }
        }

        public BinNode<T>.Type NodeType {get {return this.IsLC() ? Type.LC : IsRC() ? Type.RC : Type.Root;}}


        public BinNode(T d, BinNode<T> p = null, BinNode<T> lc = null, BinNode<T> rc = null, int h = 0) {
            this.data = d; this.Parent = p; this.lc = lc; this.rc = rc; this.height = h;
        }
        public int Size() {
            int ret = 1;
            if (lc != null)
                ret += lc.Size();
            if (rc != null)
                ret += rc.Size();
            return ret;
        }
        public int BalnceFactor() {
            return this.lc.Stature() - rc.Stature();
        }
        public bool AVLBalanced() { return -2 < BalnceFactor() && BalnceFactor() < 2; }
        public BinNode<T> InsertAsLC(T d) {return this.lc = new BinNode<T>(d, this);}
        public BinNode<T> InsertAsRC(T d) {return this.rc = new BinNode<T>(d, this);}
        public bool HasLC() { return lc != null; }
        public bool HasRC() { return rc != null; }
        public bool IsLC() { return Parent != null && Parent.LC == this; }
        public bool IsRC() { return Parent != null && Parent.RC == this; }
        public bool HasChild() { return HasLC() || HasRC(); }
        public bool IsRoot() { return Parent == null; }
        public BinNode<T> TallerChild() {
            if (lc.Stature() > rc.Stature())
                return lc;
            else if (lc.Stature() < rc.Stature())
                return rc;
            else
                return IsLC() ? lc : rc;
        }
        public int UpdateHeight() { height = 1 + Math.Max(lc.Stature(), rc.Stature()); return height; }
        public BinNode<T> RotateRight() {
            if (LC == null)
                return null;
            var lc = LC;
            lc.Parent = Parent;
            if (IsRC())
                parent.RC = lc;
            else if (IsLC())
                parent.LC = lc;
            LC = lc.RC;
            if (LC != null)
                LC.Parent = this;
            lc.RC = this;
            Parent = lc;
            return lc;
        }
        public BinNode<T> RotateLeft() {
            if (RC == null)
                return null;
            var rc = RC;
            rc.Parent = Parent;
            if (IsRC())
                parent.RC = rc;
            else if (IsLC())
                parent.LC = rc;
            RC = rc.LC;
            if (RC != null)
                RC.Parent = this;
            rc.LC = this;
            Parent = rc;
            return rc;
        }
        public override string ToString() {
            return data.ToString();
        }
        public static void GetDatasInRecursive(BinNode<T> n, List<BinNode<T>> lst) {
            if (n == null)
                return;
            GetDatasInRecursive(n.lc, lst);
            lst.Add(n);
            GetDatasInRecursive(n.rc, lst);
        }
    }

    public class BinTree<T>
    {
        int size;
        BinNode<T> root;
        Dictionary<int, int> bTypes = new Dictionary<int, int>();

        public int Size { get { return size; } set { size = value; } }
        public BinNode<T> Root {
            get { return root; }
            set {
                root = value;
                if (root != null)
                    root.Parent = null;
            }
        }
        public bool IsEmpty() { return root == null; }
        public static void UpdateHeight(BinNode<T> node) {
            if (node == null)
                return;
            node.UpdateHeight();
        }
        public static void UpdateHeightAbove(BinNode<T> node) {
            while(node != null) {
                node.UpdateHeight();
                node = node.Parent;
            }
        }
        public BinNode<T> InsertAsRoot(T e) {
            size = 1;
            root = new BinNode<T>(e);
            return root;
        }
        public BinNode<T> InsertAsLC(BinNode<T> node, T e) {
            size += 1;
            node.InsertAsLC(e);
            UpdateHeightAbove(node);
            return node.LC;
        }
        public BinNode<T> InsertAsRC(BinNode<T> node, T e) {
            size += 1;
            node.InsertAsRC(e);
            UpdateHeightAbove(node);
            return node.RC;
        }
        void AddTypes(int depth, BinNode<T>.Type type) {
            if (!bTypes.ContainsKey(depth))
                bTypes.Add(depth, (int)type);
            bTypes[depth] = (int)type;
        }
        public void Print() {
            Console.WriteLine(string.Format("****************** tree's size == {0}******************", size.ToString()));
            bTypes.Clear();
            PrintTree(root, 0, BinNode<T>.Type.Root);
        }
        void PrintTree(BinNode<T> node, int depth, BinNode<T>.Type type) {
            if (node == null)
                return;
            AddTypes(depth, type);
            PrintTree(node.RC, depth + 1, BinNode<T>.Type.RC);
            Console.Write(node.data.ToString());
            for(int i=0; i<depth; i++) {
                if (bTypes[i] + bTypes[i + 1] == 1)
                    Console.Write("      ");
                else
                    Console.Write("|     ");
            }
            if (type == BinNode<T>.Type.RC)
                Console.Write("┌──");
            else if (type == BinNode<T>.Type.LC)
                Console.Write("└──");
            else
                Console.Write("───");
            Console.WriteLine(node.data.ToString());
            PrintTree(node.LC, depth + 1, BinNode<T>.Type.LC);
        }
    }
}
