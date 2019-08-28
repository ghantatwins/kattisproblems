using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Probelms.Kattis.Com.Core;

namespace Probelms.Kattis.Com.Nested
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
                IEnumerable<List<IItem>> indexes = nested.GetTopChains(_collections);
                if (!_first) Printer.WriteLine("");
                _first = false;
                int loop = 0;
                foreach (var set in indexes)
                {
                    foreach (var item in set)
                    {
                        item.Print(Printer);
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

        public NodeBuilder(List<IItem> nestedItems, IFactory<IItem> factory)
        {
            _nestedItems = nestedItems;
            _factory = factory;
        }


        public IEnumerable<List<IItem>> GetTopChains(int collections)
        {
            var actualNodes = new List<IItem>(_nestedItems.Select(x => x));
            actualNodes.Sort(_factory.Comparer);
            for (int i = 0; i < actualNodes.Count; i++)
            {
                for (int j = i+1; j < actualNodes.Count; j++)
                {
                    if (actualNodes[i].Constraint(actualNodes[j]))
                    {
                        actualNodes[i].Children.Add(actualNodes[j]);
                    }
                }
            }

            IList<IItem> sorted = TopologicalSort.Sort(actualNodes, x => x.Children, _factory.EqualityComparer,true);
            
          
            return new List<List<IItem>>();
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






    }


}