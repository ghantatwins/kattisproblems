using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Probelms.Kattis.Com.Core;

namespace Probelms.Kattis.Com
{
    public class NestedThread : IProblem<IItem>
    {
        private readonly int _collections;
        public IFactory<IItem> Factory { get; }
        public IPrinter Printer { get; }
        public void Solve()
        {
            bool first = true;
            while (true)
            {
                List<IItem> nestedItems = new List<IItem>();
                int items = 2*Factory.TotalItems;
                if (items == 0) break;
                for (int i = 0; i < items; i++)
                {
                    var item = Factory.Create();
                    nestedItems.Add(item);
                }
                nestedItems.Sort(Factory.Comparer);
                NodeBuilder nested= new NodeBuilder(nestedItems,Factory);
                IEnumerable<List<int>> indexes = nested.GetTopChains(_collections);
                if (!first) Printer.WriteLine("");
                first = false;
                int loop = 0;
                foreach (var set in indexes)
                {
                    foreach (var itemIndex in set)
                    {
                        nestedItems[itemIndex].Print(Printer);
                    }

                    if (loop == 0)
                    {
                        Printer.WriteLine("-");
                        loop++;
                    }

                }
            }
        }

        public NestedThread(int collections, IFactory<IItem> factory, IPrinter printer)
        {
            _collections = collections;

            Factory = factory;
            Printer = printer;
        }

        public NestedThread(int collections, IFactory<IItem> threadFactory) : this(collections, threadFactory, new TerminalWriter())
        {

        }

        


    }

    public class NodeBuilder
    {
        private readonly List<IItem> _nestedItems;
        private readonly IFactory<IItem> _factory;
        private readonly NodeEqualityComparer _comparer;
        private readonly List<Node> _nodes;

        public NodeBuilder(List<IItem> nestedItems, IFactory<IItem> factory)
        {
            _nestedItems = nestedItems;
            _factory = factory;
            _comparer = new NodeEqualityComparer();
              _nodes  = new List<Node>();
        }

        public IEnumerable<List<int>> GetTopChains(int collections)
        {
            List<List<int>> indexes = new List<List<int>>();
            if (_nodes.Count == 0) BuildNodes();
            foreach (var node in _nodes)
            {
                if (node.Depth.Count == _factory.TotalItems)
                {
                    List<int> index= GetIndexes(node,3);
                    indexes.Add(index);
                }
            }

            return indexes;
        }

        private List<int> GetIndexes(Node node,int size)
        {
           List<int> indexs= new List<int>();
            var temp = node;
            int currSize = size;
            while (temp != null)
            {
               int index= _nestedItems.IndexOf(temp.Item);
                if(index!=-1) indexs.Add(index);
                currSize--;
                temp = temp.Childs.SingleOrDefault(x => x.Depth.Count == currSize);
            }

            return indexs;
        }

        private void BuildNodes()
        {
            for (int i = 0; i < _factory.TotalItems; i++)
            {
                for (int j = 0; j < _factory.TotalItems; j++)
                {
                    if (_nestedItems[i].Constraint(_nestedItems[j]))
                    {
                        var searchNode = new Node(_nestedItems[i], _factory.EqualityComparer);
                        Node node1 = _nodes.SingleOrDefault(x => _comparer.Equals(x, searchNode));
                        if (node1 == null)
                        {
                            node1 = searchNode;
                            _nodes.Add(node1);
                        }

                        searchNode = new Node(_nestedItems[j], _factory.EqualityComparer);
                        Node node2 = _nodes.SingleOrDefault(x => _comparer.Equals(x, searchNode));
                        if (node2 == null)
                        {
                            node2 = searchNode;
                            _nodes.Add(node2);
                        }

                        if (node1 != null && node2 != null)
                            node1.Childs.Add(node2);
                    }
                }
            }
        }
    }

    public class NodeEqualityComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node x, Node y)
        {
            if (x == null & y == null) return true;
            if (x == y) return true;
            if (x != y) return false;
            return x.Equals(y);
        }

        public int GetHashCode(Node obj)
        {
            return obj.GetHashCode();
        }
    }

    public class Node : IEquatable<Node>
    {
        public IItem Item { get; }

        private readonly IEqualityComparer<IItem> _comparer;


        public IList<Node> Childs { get; private set; }

        

        public Node(IItem item, IEqualityComparer<IItem> comparer)
        {
            Item = item;

            _comparer = comparer;

            Childs = new List<Node>();
        }

        public List<Node> Depth
        {
            get
            {
                List<Node> path = new List<Node>();
                foreach (Node node in Childs)
                {
                    List<Node> tmp = node.Depth;
                    if (tmp.Count > path.Count)
                        path = tmp;
                }
                path.Insert(0, this);
                return path;
            }
        }

        public override string ToString()
        {
            return Item.ItemName;
        }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _comparer.Equals(Item, other.Item);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            return (Item != null ? Item.GetHashCode() : 0);
        }

        public static bool operator ==(Node left, Node right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !Equals(left, right);
        }
    }
}