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
                NestedSolution nested= new NestedSolution(nestedItems,Factory);
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

    public class NestedSolution
    {
        
        private readonly List<IItem> _nestedItems;
        private readonly IFactory<IItem> _factory;

        public NestedSolution( List<IItem> nestedItems, IFactory<IItem> factory)
        {
            
            _nestedItems = nestedItems;
            _factory = factory;
        }

        public IEnumerable<List<int>> GetTopChains(int collections)
        {
           
           SortedDictionary<IItem,List<int>> solutions= new SortedDictionary<IItem, List<int>>(_factory.Comparer);
            for (int i = _nestedItems.Count- 1; i >= 0; i--)
            {
                var subList = new List<int>();
                for (int j = i + 1; j  < _nestedItems.Count; j++)
                {
                    if (_nestedItems[i].Constraint(_nestedItems[j]))
                    {
                        subList.Add(j);
                    }
                }

                foreach (var subItem in subList)
                {
                    if (!solutions.ContainsKey(_nestedItems[subItem]))
                    {
                        solutions[_nestedItems[subItem]] = new List<int>();
                        solutions[_nestedItems[subItem]].Add(subItem);
                    }
                    if (!solutions.ContainsKey(_nestedItems[i]))
                    {
                        solutions[_nestedItems[i]] = new List<int>();
                        solutions[_nestedItems[i]].Add(subItem);
                    }
                    //else
                    {
                        if (!solutions[_nestedItems[i]].Contains(subItem))
                        {
                            solutions[_nestedItems[i]].Add(subItem);
                        }
                            
                    }
                }
                
                
            }
            return GetRelevant(solutions,collections);
        }

        private IEnumerable<List<int>> GetRelevant(IDictionary<IItem, List<int>> solutionsValues, int collections)
        {
            var expected= new List<List<int>>(solutionsValues.Values);
            expected.Sort(new ListComparer());
            for (int i = 0; i < expected.Count; i++)
            {
                if (expected[i].Count > _factory.TotalItems)
                {
                    for (int j = i + 1; j < expected.Count; j++)
                    {
                        var intersect = expected[i].Except(expected[j]).ToList();
                        foreach (var intersected in intersect.ToList())
                        {
                            expected[i].Remove(intersected);
                        }

                    }
                }
                
            }
            return expected.Take(collections);
        }
    }

    internal class ListComparer : IComparer<List<int>>
    {
        public int Compare(List<int> x, List<int> y)
        {
            if (x == null && y == null) return 0;
            if (x == y) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return y.Count - x.Count;

        }
    }
}