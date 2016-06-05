/*
 * Property of Binary Search Tree
 * The values in a binary search tree are always stored in such a way as to satisfy the
    binary-search-tree property:
        Let x be a node in a binary search tree. If y is a node in the left subtree
        of x, then y.value <= x.value. If y is a node in the right subtree of x, then
        y.value >= x.value.
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo
{
    class Node
    {
        public Node Parent { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Value { get; set; }

        public static void PrintSingleNodeInfo(Node n)
        {
            if (n == null)
            {
                Console.WriteLine("Null");
                return;
            }

            Console.WriteLine("Node({0}).Parent = {1}", n.Value, (n.Parent == null) ? "Null" : n.Parent.Value.ToString());
            Console.WriteLine("Node({0}).Left = {1}", n.Value, (n.Left == null) ? "Null" : n.Left.Value.ToString());
            Console.WriteLine("Node({0}).Right = {1}", n.Value, (n.Right == null) ? "Null" : n.Right.Value.ToString());
            Console.WriteLine("-----------------");
        }

    }

    class BinarySearchTree
    {
        #region fields
        private List<Node> _tree = new List<Node>();
        private Node _root;
        #endregion

        #region ctors
        public BinarySearchTree()
        {
        }

        public BinarySearchTree(Node root)
        {
            _root = root;
        }

        public BinarySearchTree(int value)
        {
            _root.Value = value;
        }
        #endregion

        #region properties
        public Node Root 
        { 
            get { return _root; }
            set { _root = value; }
        }  

        public List<Node> Tree { get { return _tree; } }
        #endregion

        #region BST manipulations
        #region insert a node - O(h) time on a tree of height h
        /*
         * TREE-INSERT(T,  z)
            1 y = NIL
            2 x = T.root
            3 while x != NIL                  // while loop locates the parent position for z
            4      y = x                        // and store in y
            5      if z.Value < x.Value
            6          x = x.left
            7      else x = x.right
            8 z.parent = y                   // get the parent now
            9 if y == NIL
          10    T.root = z // tree T was empty
          11 elseif z.Value < y.Value
          12    y.left = z
          13 else y.right = z
         */
        public void Insert(Node z)
        {
            Insert(this, z);
        }

        public void Insert(BinarySearchTree T, Node z)
        {
            Node y = null;   // z's potential parent
            Node x = T.Root;
            while( x != null)
            {
                y = x;
                if (z.Value < x.Value)
                    x = x.Left;
                else
                    x = x.Right;
            }
            
            z.Parent = y;

            if (y == null)
                T.Root = z;    // tree was empty
            else if (z.Value < y.Value)
                y.Left = z;
            else
                y.Right = z;

            T.Tree.Add(z);
        }
        #endregion

        #region delete - O(h) time on a tree of height h.
        /*
         *  If z has no left child (part (a) of the figure), then we replace z by its right child,
            which may or may not be NIL. When z's right child is NIL, this case deals with
            the situation in which z has no children. When z's right child is non-NIL, this
            case handles the situation in which z has just one child, which is its right child.
            
         *  If z has just one child, which is its left child (part (b) of the figure), then we
            replace z by its left child.
            
         *  Otherwise, z has both a left and a right child. We find z's successor y, which
            lies in z's right subtree and has no left child (see Exercise 12.2-5). We want to
            splice y out of its current location and have it replace z in the tree.
                 If y is z's right child (part (c)), then we replace z by y, leaving y's right child alone.
                 Otherwise, y lies within z's right subtree but is not z's right child (part (d)).
                 In this case, we first replace y by its own right child, and then we replace z by y.
         * 
         * 
         * TREE-DELETE (T, z)
            1 if z.left == NIL
            2     TRANSPLANT(T, z, z.right)
            3 elseif z.right == NIL
            4     TRANSPLANT(T, z, z.left)
            5 else 
              {
            5     y = TREE-MINIMUM(z.right)
            6     if y.parent != z
                  {
            7        TRANSPLANT(T, y, y.right)
            8        y.right = z.right
            9        y.right.parent = y
                  }
                
            10    TRANSPLANT(T, z, y)
            11    y.left = z.left
            12    y.left.parent = y
               }
         */
        public void Delete(Node z)
        {
            Delete(this, z);
        }

        public static void Delete(BinarySearchTree T, Node z)
        {
            if (z.Left == null)
                Transplant(T, z, z.Right);
            else if (z.Right == null)
                Transplant(T, z, z.Left);
            else
            {
                Node y = T.Min(z.Right);

                if(y.Parent != z)
                {
                    Transplant(T, y, y.Right);
                    // update y.right
                    y.Right = z.Right;
                    y.Right.Parent = y;
                }

                Transplant(T, z, y);
                // update y.left 
                y.Left = z.Left;
                y.Left.Parent = y;
 
            }

            // physically remove the node
            T.Tree.RemoveAll( n => n.Value == z.Value);
        }

        /*
         *  In order to move subtrees around within the binary search tree, we define a
            subroutine TRANSPLANT, which replaces one subtree as a child of its parent Node with
            another subtree. When TRANSPLANT replaces the subtree rooted at node m with
            the subtree rooted at node n, node m's parent becomes node n's parent, and m's
            parent ends up having n as its appropriate child.
         * 
         * TRANSPLANT (T, m, n )
         *  1 if m.parent == NIL
            2     T.root = n;
            3 elseif m == m.parent.left
            4     m.parent.left = n
            5 else 
                  m.parent.right = n
            6 if n != NIL
            7     n.parent = m.parent
         * 
         * Note that TRANSPLANT does not attempt to update n.left and n.right; 
         * doing so, or not doing so, is the responsibility of TRANSPLANT’s caller.
         * 
         */
        public static void Transplant(BinarySearchTree T, Node m, Node n)   // m is to be replaced by n
        {
            if (m.Parent == null)
                T.Root = n;
            else if (m == m.Parent.Left)
                m.Parent.Left = n;
            else
                m.Parent.Right = n;

            if (n != null)
                n.Parent = m.Parent;
        }
        #endregion

        #region inorder tree walk - O(n) n is number of nodes
        /*
         * This algorithm is so named because it prints the key of the root of a subtree
            between printing the values in its left subtree and printing those in its right subtree.
         * 
         * INORDER-TREE-WALK (x)
            1 if x ¤ NIL
            2     INORDER-TREE-WALK(x: left)
            3     print x:key
            4     INORDER-TREE-WALK(x:right)
         */
        public void InorderTreeWalk()
        {
            Recursive_InorderTreeWalk(this.Root);
        }

        private void Recursive_InorderTreeWalk(Node n)
        {
            if (n != null)
            {
                Recursive_InorderTreeWalk(n.Left);
                Node.PrintSingleNodeInfo(n);
                Recursive_InorderTreeWalk(n.Right);
            }
        }
        #endregion

        #region preorder tree walk -  - O(n) n is number of nodes
        /*
         * This algorithm is so named because it prints the root before the values in either subtree
         * 
         * INORDER-TREE-WALK (x)
            1 if x ¤ NIL
            2     print x:key
            3     INORDER-TREE-WALK(x: left)
            4     INORDER-TREE-WALK(x:right)
         */
        public void PreorderTreeWalk()
        {
            Recursive_PreorderTreeWalk(this.Root);
        }

        private void Recursive_PreorderTreeWalk(Node n)
        {
            if (n != null)
            {
                Node.PrintSingleNodeInfo(n);
                Recursive_PreorderTreeWalk(n.Left);
                Recursive_PreorderTreeWalk(n.Right);
            }
        }
        #endregion

        #region postorder tree walk - O(n) n is number of nodes
        /*
         * This algorithm is so named because it prints the root after the values in its subtrees
         * 
         * INORDER-TREE-WALK (x)
            1 if x ¤ NIL
            2     INORDER-TREE-WALK(x: left)
            3     INORDER-TREE-WALK(x:right)
            4     print x:key
         */
        public void PostorderTreeWalk()
        {
            Recursive_PostorderTreeWalk(this.Root);
        }

        private void Recursive_PostorderTreeWalk(Node n)
        {
            if (n != null)
            {
                Recursive_PostorderTreeWalk(n.Left);
                Recursive_PostorderTreeWalk(n.Right);
                Node.PrintSingleNodeInfo(n);
            }
        }
        #endregion

        #region search - O(h)
        public Node Search(int target)
        {
            //return Recursive_Search(this.Root, target);
            return Iterative_Search(target);
        }

        private Node Recursive_Search(Node n, int target)
        {
            if ((n==null) || (n.Value == target))
                return n;

            Node ret;
            if (target < n.Value)
                ret = Recursive_Search(n.Left, target);
            else
                ret = Recursive_Search(n.Right, target);

            return ret;
        }

        private Node Iterative_Search(int target)
        {
            Node x = this.Root;

            while ((x != null) && (x.Value != target))
            {
                if (target < x.Value)
                    x = x.Left;
                else
                    x = x.Right;
            }

            return x;
        }
        #endregion

        #region Min - O(h)
        public Node Min(Node x)
        {
            //Node x = this.Root;
            while (x.Left != null)
                x = x.Left;

            return x;
        }

        #endregion

        #region Max - O(h)
        public Node Max(Node x)
        {
            //Node x = this.Root;
            while (x.Right != null)
                x = x.Right;

            return x;
        }
        #endregion

        #region Successor - O(h)
        /*
         * If all keys are distinct, the successor of a node x is the node with the smallest key greater than x:key.
         */
        public Node Successor(Node n)
        {
            if (n.Right != null)
                return Min(n.Right);

            Node y = n.Parent;
                                    // n.Value >= y.Value
            while ((y != null) && (n == y.Right))   // stop when n is y's left child, which means n.Value < y.Value
            {
                n = y;
                y = y.Parent;
            }

            return y;
        }
        #endregion

        #region Predecessor - O(h)
        /*
         * If all keys are distinct, the predecessor of a node x is the node with the greatest key smaller than x:key.
         */
        public Node Predecessor(Node n)
        {
            if (n.Left != null)
                return Max(n.Left);

            Node y = n.Parent;
                                  // n.Value <= y.Value
            while ((y != null) && (n == y.Left))   // stop when n is y's right child, which means n.Value > y.Value
            {
                n = y;
                y = y.Parent;
            }

            return y;
        }
        #endregion

        #region print nodes
        public void PrintNodes()
        {
            foreach (Node n in _tree)
                Node.PrintSingleNodeInfo(n);
        }
        #endregion
        #endregion

        #region Try methods
        public static void TryInsert()
        {
            /*
             *                          12
             *            5                            18     
             *        2       9               15             19
             *                                      17 
             */ 

            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            bst.PrintNodes();

            Console.WriteLine("-----insert Node(13)-----");
            Node n13 = new Node { Value = 13};
            bst.Insert(n13);
            Node.PrintSingleNodeInfo(n13);

        }

        public static void TryInorderTreeWalk()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            bst.InorderTreeWalk();

            Console.WriteLine("-----insert Node(13)-----");
            Node n13 = new Node { Value = 13 };
            bst.Insert(n13);

            bst.InorderTreeWalk();
 
        }

        public static void TryPreorderTreeWalk()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            bst.PreorderTreeWalk();

            Console.WriteLine("-----insert Node(13) and Node(14)-----");
            bst.Insert(new Node { Value = 13 });
            bst.Insert(new Node { Value = 14 });

            bst.PreorderTreeWalk();

        }

        public static void TryPostorderTreeWalk()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            bst.PostorderTreeWalk();

            Console.WriteLine("-----insert Node(13) and Node(14)-----");
            bst.Insert(new Node { Value = 13 });
            bst.Insert(new Node { Value = 14 });

            bst.PostorderTreeWalk();

        }

        public static void TrySearch()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            Node n17 = bst.Search(5);
            Node.PrintSingleNodeInfo(n17);

        }

        public static void TryMinMax()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            Node.PrintSingleNodeInfo(bst.Min(nodes[4]));
            Node.PrintSingleNodeInfo(bst.Max(bst.Root));

        }

        public static void TrySuccessor()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            Node.PrintSingleNodeInfo(bst.Successor(nodes[3]));

        }

        public static void TryPredecessor()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            Node.PrintSingleNodeInfo(bst.Predecessor(nodes[5]));

        }

        public static void TryTransplant()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            BinarySearchTree.Transplant(bst, nodes[1], nodes[3]);   // replace 5 by 9

            bst.PrintNodes();
        }

        public static void TryDelete()
        {
            Node[] nodes = new Node[] { new Node { Value = 12 }, new Node { Value = 5 }, new Node { Value = 2 },
                                                     new Node{ Value=9}, new Node{ Value=18}, new Node{ Value=15},
                                                     new Node{ Value=17}, new Node{ Value=19}};
            BinarySearchTree bst = new BinarySearchTree();
            foreach (Node n in nodes)
                bst.Insert(n);

            bst.Delete(nodes[0]);   // delete 5

            bst.PrintNodes();
        }
        #endregion
    }
}
