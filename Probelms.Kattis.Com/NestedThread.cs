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
        private bool _first;

        public IPrinter Printer { get; }
        public void Solve(IFactory<IItem> factory)
        {
            
            
            while (true)
            {
                int items = 2 * factory.TotalItems;
                List<IItem> nestedItems = new List<IItem>();
                if (items == 0) break;
                for (int i = 0; i < items; i++)
                {
                    var item = factory.Create();
                    nestedItems.Add(item);
                }
                nestedItems.Sort(factory.Comparer);
                NodeBuilder nested = new NodeBuilder(nestedItems, factory);
                IEnumerable<List<int>> indexes = nested.GetTopChains(_collections);
                if (!_first) Printer.WriteLine("");
                _first = false;
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
                factory.Reset();
            }

            
        }

        public NestedThread(int collections, IPrinter printer)
        {
            _collections = collections;

            Printer = printer;
            _first = true;
        }

        public NestedThread(int collections) : this(collections, new TerminalWriter())
        {

        }




    }

    public class NodeBuilder
    {
        private readonly List<IItem> _nestedItems;
        private readonly IFactory<IItem> _factory;
        private readonly NodeEqualityComparer _comparer;
        private readonly List<Node> _nodes;
        private WeightedComparer _weightedComparer;
        private NodeComparer _nodeComparer;
        private readonly NodeItemComparer _nodeItemComparer;

        public NodeBuilder(List<IItem> nestedItems, IFactory<IItem> factory)
        {
            _nestedItems = nestedItems;
            _factory = factory;
            _comparer = new NodeEqualityComparer();
            _nodeComparer = new NodeComparer();
            _nodes = new List<Node>();
            _weightedComparer=new WeightedComparer();
            _nodeItemComparer = new NodeItemComparer(_factory.Comparer);
        }

        public IEnumerable<List<int>> GetTopChains(int collections)
        {
            if (_nodes.Count == 0) BuildNodes(collections);
            
            var collection= GetIndexes().ToList();
            collection.Sort(_weightedComparer);
            return collection.Take(collections);
        }

        public class WeightedComparer : IComparer<List<int>>
        {
            public int Compare(List<int> x, List<int> y)
            {
                if (x == y) return 0;
                if (x == null) return 1;
                if (y == null) return -1;
                return y.Count - x.Count;
            }
        }

        private IEnumerable<List<int>> GetIndexes()
        {
            Dictionary<IItem,List<int>> indexers = new Dictionary<IItem, List<int>>();
            _nodes.Sort(_nodeItemComparer);
            foreach (var node in _nodes)
            {
                List< int> indexs=new List<int>();
                int currSize = node.ChainSize;
                var temp = node;
                while (temp != null)
                {
                    int index = _nestedItems.IndexOf(temp.Item);
                    if (index != -1) indexs.Add(index);
                    currSize--;
                    var temps = new List<Node>(temp.Childs.Where(x => x.ChainSize == currSize));
                    temps.Sort(_nodeComparer);
                    Dictionary<Node,List<int>> subIndexes = new Dictionary<Node, List<int>>();
                    foreach (var subTemp in temps)
                    {
                        
                        var key = subTemp.Item;
                        if (indexers.ContainsKey(key))
                        {
                            subIndexes.Add(subTemp,indexers[key]);
                        }
                    }

                    var pair = subIndexes.SingleOrDefault(x => x.Value.Count == subIndexes.Max(y => y.Value.Count));
                    if (pair.Key != null)
                    {
                        indexs.AddRange(pair.Value);
                        indexers.Remove(pair.Key.Item);
                    }
                    temp = pair.Key;
                }
                indexs = indexs.Distinct().ToList();
                indexers.Add(node.Item,indexs);
            }
            

            return indexers.Values;
        }

        private void BuildNodes(int collections)
        {
            int total = _factory.TotalItems;
            for (int i = collections * _factory.TotalItems - 1; i >= 0; i--)
            {
                var searchNode = new Node(_nestedItems[i], _factory.EqualityComparer);
                Node node1 = _nodes.SingleOrDefault(x => _comparer.Equals(x, searchNode));
                if (node1 == null)
                {
                    node1 = searchNode;
                    node1.ChainSize = total;
                    _nodes.Add(node1);
                }
            }
            for (int i = 0; i < collections * _factory.TotalItems; i++)
            {
                var searchNode = new Node(_nestedItems[i], _factory.EqualityComparer);
                Node node1 = _nodes.SingleOrDefault(x => _comparer.Equals(x, searchNode));

                for (int j = 0; j < collections * _factory.TotalItems; j++)
                {
                    searchNode = new Node(_nestedItems[j], _factory.EqualityComparer);
                    Node node2 = _nodes.SingleOrDefault(x => _comparer.Equals(x, searchNode));
                    if (node2 == null)
                    {
                        node2 = searchNode;
                        _nodes.Add(node2);
                    }
                    if (_nestedItems[i].Constraint(_nestedItems[j]))
                    {
                        if (node1 != null)
                        {
                            int temp = node1.ChainSize - 1;
                            node2.ChainSize = temp;
                            if (!node1.Childs.Contains(node2, new NodeEqualityComparer()))
                                node1.Childs.Add(node2);
                        }


                    }

                }
            }
        }
    }

    public class NodeItemComparer:IComparer<Node>
    {
        private readonly IComparer<IItem> _factoryComparer;

        public NodeItemComparer(IComparer<IItem> factoryComparer)
        {
            _factoryComparer = factoryComparer;
        }

        public int Compare(Node x, Node y)
        {
            if (x == null & y == null) return 0;
            if (x == y) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return _factoryComparer.Compare(y.Item, x.Item);
        }
    }

    public class NodeComparer:IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if (x == null & y == null) return 0;
            if (x == y) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return y.ChainSize - x.ChainSize;
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
        public int ChainSize { get; set; }


        public IList<Node> Childs { get; private set; }



        public Node(IItem item, IEqualityComparer<IItem> comparer)
        {
            Item = item;

            _comparer = comparer;


            Childs = new List<Node>();
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
            return Equals((Node)obj);
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