using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Probelms.Kattis.Com.Core;
using Probelms.Kattis.Com.Nested;

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
                List<List<int>> indexes = nested.GetTopChains(_collections);
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

    public class Node:IComparable<Node>,IEquatable<Node>
    {
        public IEnumerable<Node> Children
        {
            get { return _children; }
        }
      
        private readonly IEqualityComparer<IItem> _itemEqualityComparer;
        private readonly IComparer<IItem> _itemComparer;
        private List<Node> _children;

        public Node(IItem item,IEqualityComparer<IItem> itemEqualityComparer, IComparer<IItem> itemComparer)
        {
            Item = item;
            _itemEqualityComparer = itemEqualityComparer;
            _itemComparer = itemComparer;
            _children=new List<Node>();
        }

        public IItem Item { get; }


        public int CompareTo(Node other)
        {
            return _itemComparer.Compare(Item, other.Item);
        }

        public bool Equals(Node other)
        {
            return _itemEqualityComparer.Equals(Item, other.Item);
        }

        public void Add(Node actualNode)
        {
            _children.Add(actualNode);
        }
    }
    public class NodeBuilder
    {
        private readonly List<IItem> _nestedItems;
        private readonly IFactory<IItem> _factory;

        public NodeBuilder(List<IItem> nestedItems, IFactory<IItem> factory)
        {
            _nestedItems = nestedItems;
            _factory = factory;
        }


        public List<List<int>> GetTopChains(int collections)
        {
            _nestedItems.Sort(NestedComparater);
            var actualNodes = new List<Node>(_nestedItems.Select(x=>new Node(x,_factory.EqualityComparer,_factory.Comparer)));
            for (int i = 0; i < actualNodes.Count; i++)
            {
                for (int j = 0; j < actualNodes.Count; j++)
                {
                    if (actualNodes[i].Item.Constraint(actualNodes[j].Item))
                    {
                        actualNodes[i].Add(actualNodes[j]);
                    }
                }
            }
           actualNodes=new List<Node>(actualNodes.TopoSort(x => x, x => x.Children).ToArray());
            return new List<List<int>>();
        }





        /*
        public IEnumerable<List<int>> GetTopChains(int collections)
        {
            List<List<int>> indexes = new List<List<int>>();

            //if (_nestedItems == null || _nestedItems.Count == 0)
            //    return indexes;
            //_nestedItems.Sort(NestedComparater);
            //int max = 1;
            //int[] arr = new int[_nestedItems.Count];
            //for (int i = 0; i < _nestedItems.Count; i++)
            //{
            //    arr[i] = 1;
            //    int[] set=new int[_nestedItems.Count];
            //    set[0]=-1;
            //    for (int j = i - 1; j >= 0; j--)
            //    {
            //        if (_nestedItems[j].Constraint(_nestedItems[i]))
            //        {
            //            arr[i] = Math.Max(arr[i], arr[j] + 1);
            //            set[i]= Math.Max(set[i], set[j] + 1);
            //        }
            //    }

            //    max = Math.Max(max, arr[i]);
            //    indexes.Add(set.ToList());


            //}
            int i = MaxNestedItems(_nestedItems.ToArray());
            return indexes;
        }


*/




        public int MaxNestedItems(IItem[] nestedItems)
        {
            if (nestedItems == null || nestedItems.Length == 0)
                return 0;
            Array.Sort(nestedItems, NestedComparater);
            int max = 1;
            int[] arr = new int[nestedItems.Length];
            List<List<int>> indexes = new List<List<int>>();
            for (int i = 0; i < nestedItems.Length; i++)
            {
                List<int> index = new List<int> { i };
                arr[i] = 1;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (nestedItems[j].Constraint(nestedItems[i]))
                    {
                        arr[i] = Math.Max(arr[i], arr[j] + 1);
                        index.Insert(0, j);
                    }
                }


                max = Math.Max(max, arr[i]);
                indexes.Add(index);

            }

            indexes = indexes.Where(x => x.Count >= _factory.TotalItems).ToList();
            indexes.Sort(new ListAggComparator());

            return max;
        }

        private int NestedComparater(IItem x, IItem y)
        {
            return _factory.Comparer.Compare(x, y);
        }

        //private int NestedComparater(int[] a, int[] b)
        //{
        //    if (a[0] != b[0])
        //    {
        //        return a[0] - b[0];
        //    }
        //    return a[1] - b[1];
        //}
       
    }

    public class ListAggComparator : IComparer<List<int>>
    {
        public int Compare(List<int> x, List<int> y)
        {
            return y.Count - x.Count;
        }
    }
}