using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CapGLib;

namespace RussianDolls
{
    public class Problem : ConsoleWorker
    {
        public class DollComparer : IComparer<Doll>
        {
            public int Compare(Doll x, Doll y)
            {
                if (x == null) return -1;
                if (y == null) return 1;
                return x.Compare(y);
            }
        }
        public class Doll
        {
            

            readonly int _h;
            readonly int _d;
            readonly int _w;
            


            public Doll(int h, int d, int w)
            {
                _h = h;
                _w = w;
                _d = d;
               
            }

            public int H
            {
                get { return _h; }
            }

            public int D
            {
                get { return _d; }
            }

            public int W
            {
                get { return _w; }

            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", H, D, W);
            }

            public int Compare(Doll d)
            {
                if (d == null) return -1;
                
                if (_h - 2 * _w >= d.H)
                {
                    return -1;
                }
                if (_d - 2 * _w >= d.D)
                {
                    return -1;
                }
                if (_h== d.H && _d==d.D&&_w==d.W)
                {
                    return 0;
                }
                if (_h > d.H)
                {
                    return -1;
                }
                if (_d > d.D)
                {
                    return -1;
                }
                return 1;
            }
            public bool CanContain(Doll doll)
            {
                return _h - 2 * _w >= doll._h && _d - 2 * _w >= doll._d;
            }






        }

        public Problem(IScanner scanner) : base(scanner)
        {
        }

        public override void DoWork()
        {
            bool first = true;

            while (true)
            {
                List<Doll> nestedDolls = new List<Doll>();
                int numDolls = Scanner.NextInt();
                if (numDolls == 0) break;
                var comparer = new DollComparer();


                for (int i = 0; i < 2 * numDolls; i++)
                {
                    int j = 0;
                    var height = Scanner.NextInt();
                    var diameter = Scanner.NextInt();
                    var width = Scanner.NextInt();
                    var newDoll = new Doll(height, diameter, width);

                    nestedDolls.Add(newDoll);


                }
                nestedDolls.Sort(comparer);
                foreach (var nested in nestedDolls)
                {
                    foreach (var next in nestedDolls)
                    {
                        if (next != nested)
                        {
                            nested.CanContain(next);
                        }

                    }
                }


                bool[,] relationships = CreateRelationships(nestedDolls, numDolls);
                IEnumerable<IEnumerable<Doll>> sets = LongestChains(nestedDolls, relationships, numDolls);
                if (!first) Console.WriteLine("");
                first = false;
                int loop = 0;
                foreach (var set in sets)
                {
                    foreach (var doll in set)
                    {
                        Console.WriteLine("{0} {1} {2}", doll.H, doll.D, doll.W);
                    }

                    if (loop == 0)
                    {
                        Console.WriteLine("-");
                        loop++;
                    }

                }
            }

        }



        private static bool[,] CreateRelationships(List<Doll> nestedDolls, int numDolls)
        {

            //empty inventory for all ball relations
            bool[,] relationShips = new bool[2 * numDolls, 2 * numDolls];
            for (int i = 0; i < numDolls * 2; i++)
            {
                for (int j = 0; j < numDolls * 2; j++)
                {
                    relationShips[i, j] = false;
                }
            }
            //filling with relationShips
            for (int i = 0; i < numDolls * 2; i++)
            {
                for (int j = 0; j < numDolls * 2; j++)
                {
                    if (i != j)
                    {
                        relationShips[i, j] = nestedDolls[i].CanContain(nestedDolls[j]);
                    }

                }
            }

            

            return relationShips;
        }

        IEnumerable<IEnumerable<Doll>> LongestChains(List<Doll> dolls, bool[,] relationships, int numDolls)
        {
            ChainComparer chain= new ChainComparer();
            Dictionary<Doll, List<Doll>> nested = new Dictionary<Doll, List<Doll>>();
            for (int i = 0; i < 2 * numDolls; i++)
            {
                for (int j = i + 1; j < 2 * numDolls; j++)
                {
                    if (relationships[i, j])
                    {
                        List<Doll> inner = new List<Doll> {dolls[i]};
                        inner.AddRange(GetNested(dolls, relationships, numDolls, j));
                        if (!nested.ContainsKey(dolls[i]))
                        {
                            if (inner.Count == numDolls)
                            {
                                if(CheckToAdd(nested,inner))
                                    nested.Add(dolls[i], inner);
                                
                            }
                               
                            else
                            {
                               
                                if (inner.Count > numDolls)
                                {
                                    for (int k = inner.Count-1; k >= numDolls; k--)
                                    {
                                        inner.Remove(inner[k]);
                                    }
                                    if (CheckToAdd(nested, inner))
                                        nested.Add(dolls[i], inner);
                                }
                                
                            }
                        }
                        else
                        {
                            if (inner.Count == numDolls)
                            {
                                if (chain.Compare(inner, nested[dolls[i]]) < 0)
                                {
                                    if (CheckToAdd(nested, inner))
                                        nested[dolls[i]] = inner;
                                }
                                    
                            }
                            
                           

                            
                        }
                    }
                }
            }

            return nested.Values;
        }

        public bool CheckToAdd(Dictionary<Doll, List<Doll>> nested,List<Doll> inner)
        {
            foreach (var currList in nested.Values)
            {
                foreach (var doll in inner)
                {
                    if (currList.Contains(doll)) return false;
                }
            }

            return true;
        }

        private List<Doll> GetNested(List<Doll> dolls, bool[,] relationships, int numDolls, int i)
        {
            List<Doll> nested = new List<Doll> { dolls[i] };
            for (int j = i; j < 2 * numDolls; j++)
            {
                for (int k = i + 1; k < 2 * numDolls; k++)
                {
                    if (relationships[j, k])
                    {
                        nested.AddRange(GetNested(dolls, relationships, numDolls, k));
                        return nested;
                    }
                }
            }
            return nested;
        }
    }

 

    internal class ChainFirstComparer : IComparer<List<Problem.Doll>>
    {
        public int Compare(List<Problem.Doll> x, List<Problem.Doll> y)
        {
            if (x == null) return -1;
            if (y == null) return 1;
            return x[0].Compare(y[0]);
        }
    }

    internal class ChainComparer : IComparer<List<Problem.Doll>>
    {
        
        public int Compare(List<Problem.Doll> x, List<Problem.Doll> y)
        {
            if (x == null) return -1;
            if (y == null) return 1;
            if (x.Count == y.Count)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i].Compare(y[i]) != 0) return x[i].Compare(y[i]);
                    
                }
            }
            
            
            return x.Count.CompareTo(y.Count);
        }
    }
}