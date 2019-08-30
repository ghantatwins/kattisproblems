using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AvlTree;

namespace Terminal.Katttis.Com
{
    public class Solution
    {
      

        
        public List<List<Doll>> LongestChains(List<Doll> envelopes)
        {
            if (envelopes == null)
            {
                return new List<List<Doll>>();
            }

            if (!envelopes.Any()) return new List<List<Doll>>();

            envelopes.Sort();

            AvlTree<Doll> currTree = new AvlTree<Doll>();
            AvlTree<Doll>.Node root = null;
            foreach (var envelope in envelopes)
            {
                currTree.Insert(envelope);
                root = currTree.Find(envelope);
            }

            currTree.Print();
            
            return new List<List<Doll>>
            {

            };
        }

        private List<AvlTree<Doll>.Node> GetNodes(AvlTree<Doll>.Node d)
        {
            var root = d;
            List<AvlTree<Doll>.Node> nodes = new List<AvlTree<Doll>.Node>();
            while (root != null)
            {
                nodes.Add(root);
                if (root.left == null && root.right == null)
                    root = root.parent;
                else root = root.left ?? root.right;
            }

            return nodes;
        }
    }
    public abstract class ConsoleWorker
    {
        protected readonly IScanner Scanner;

        protected ConsoleWorker(IScanner scanner)
        {
            Scanner = scanner;
        }

        public abstract void DoWork();

    }
    public interface IScanner
    {
        int NextInt();
        long NextLong();
        float NextFloat();
        double NextDouble();
        bool HasNext();
        string NextString();
    }

    public class TestScanner : Tokenizer, IScanner
    {

        private Tokenizer _tempTokenizer;
        private int _i = 0;
        private readonly List<String> _streamToRead;
        public TestScanner(IEnumerable<string> payLoad)
        {
            _streamToRead = new List<string>(payLoad);
        }

        private void Setup(string text)
        {
            if (_tempTokenizer == null)
            {
                //using (Stream s = GetStream(text))
                _tempTokenizer = new Tokenizer(GetStream(text));

            }


        }

        private Stream GetStream(string text)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            return new MemoryStream(byteArray);

        }

        private string GetCurrent()
        {
            if (_i < _streamToRead.Count)
            {

                String toReturn = _streamToRead[_i];
                _i++;
                return toReturn;
            }

            return null;
        }

        void SetTokenizer()
        {
            if (_tempTokenizer != null)
            {
                if (!_tempTokenizer.HasNext())
                    _tempTokenizer = null;
            }
        }

        public string Ensure()
        {
            SetTokenizer();
            string token = null;
            if (_tempTokenizer == null)
            {
                token = GetCurrent();
                Setup(token);
                token = _tempTokenizer.Next();
            }
            else
            {
                token = _tempTokenizer.Next();
            }
            return token;
        }
        public int NextInt()
        {
            return int.Parse(Ensure());
        }

        public long NextLong()
        {
            return long.Parse(Ensure());
        }

        public float NextFloat()
        {
            return float.Parse(Ensure());
        }

        public double NextDouble()
        {

            return double.Parse(Ensure());
        }

        public string NextString()
        {
            return Ensure();
        }
    }

    public class Tokenizer
    {
        string[] tokens = new string[0];
        private int pos;
        StreamReader reader;

        public Tokenizer(Stream inStream)
        {
            var bs = new BufferedStream(inStream);
            reader = new StreamReader(bs);
        }

        public Tokenizer() : this(Console.OpenStandardInput())
        {
            // Nothing more to do
        }

        private string PeekNext()
        {
            if (pos < 0)
                // pos < 0 indicates that there are no more tokens
                return null;
            if (pos < tokens.Length)
            {
                if (tokens[pos].Length == 0)
                {
                    ++pos;
                    return PeekNext();
                }
                return tokens[pos];
            }
            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                // There is no more data to read
                pos = -1;
                return null;
            }
            // Split the line that was read on white space characters
            tokens = line.Split(null);
            pos = 0;
            return PeekNext();
        }

        public bool HasNext()
        {
            return (PeekNext() != null);
        }

        public string Next()
        {
            string next = PeekNext();
            if (next == null)
                throw new NoMoreTokensException();
            ++pos;
            return next;
        }

        public virtual string NextLine()
        {
            return reader.ReadLine();
        }
    }

    public class Scanner : Tokenizer, IScanner
    {

        public int NextInt()
        {
            return int.Parse(Next());
        }

        public long NextLong()
        {
            return long.Parse(Next());
        }

        public float NextFloat()
        {
            return float.Parse(Next());
        }

        public double NextDouble()
        {
            return double.Parse(Next());
        }

        public string NextString()
        {
            return Next();
        }
    }

    public class NoMoreTokensException : Exception
    {
    }

    public class BufferedStdoutWriter : StreamWriter
    {
        public BufferedStdoutWriter() : base(new BufferedStream(Console.OpenStandardOutput()))
        {
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
    public class Doll:IComparable<Doll>,IEquatable<Doll>
    {


        readonly int _h;
        readonly int _d;
        readonly int _w;
        private List<Doll> _children;


        public Doll(int h, int d, int w)
        {
            _h = h;
            _w = w;
            _children = new List<Doll>();
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

        public IEnumerable<Doll> Children
        {
            get { return _children; }
        }


        public int CompareTo(Doll d)
        {
            if (d == null) return -1;
            if (Equals(d))
            {
                return 0;
            }
            return CanContain(this, d)?-1:1;
        }

        public bool Equals(Doll d)
        {
            if (d == null) return false;
            return _h == d.H && _d == d.D && _w == d.W;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", H, D, W);
        }

        


        private bool CanContain(Doll d1, Doll d2)
        {
            return d1.H < d2.H || d1.H == d2.H && d1.D > d2.D;
        }

        public void Add(Doll next)
        {
            _children.Add(next);
        }

        
    }

    public class Problem : ConsoleWorker
    {




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


                for (int i = 0; i < 2 * numDolls; i++)
                {
                    int j = 0;
                    var H = Scanner.NextInt();
                    var D = Scanner.NextInt();
                    var W = Scanner.NextInt();
                    var newDoll = new Doll(H, D, W);

                    nestedDolls.Add(newDoll);


                }
                Solution solution = new Solution();


                IEnumerable<IEnumerable<Doll>> sets = solution.LongestChains(nestedDolls);
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



        IEnumerable<IEnumerable<Doll>> LongestChains(List<Doll> dolls, int numDolls)
        {
            return Sort(dolls, numDolls);

        }

       

        private static List<Doll> SubListSorted(int collection, List<Doll> reverse, int i,
            IComparer<Doll> dollComparer)
        {
            var toSort = reverse.GetRange(i, Math.Min(collection, reverse.Count - i));
            toSort.Sort(dollComparer);
            return toSort;
        }


        IList<List<Doll>> Sort(IEnumerable<Doll> actual, int numDolls)
        {
            List<Doll> source = new List<Doll>(actual);
            var visited = new Dictionary<Doll, List<Doll>>();
            List<List<Doll>> dolls = new List<List<Doll>>();
            var listComparer = new ListEqualityComparer();
            for (int i = 0; i < source.Count;)
            {

                Visit(source[i], ref visited, numDolls);
                var temp = visited[source[i]];
                if (visited[source[i]].Count == numDolls)
                {
                    if (dolls.Count == 0)
                    {
                        dolls.Add(new List<Doll>(temp));
                        visited.Clear();
                        visited[source[i]] = temp;
                        source.Remove(source[i]);
                    }
                    else
                    {
                        List<Doll> innerTemp = new List<Doll>();
                        foreach (var doll in dolls)
                        {
                            if (!dolls.Contains(temp, listComparer) && doll.Count == numDolls)
                            {
                                if (!doll.Intersect(temp).Any())
                                {
                                    innerTemp.AddRange(temp);
                                    break;
                                }



                            }

                        }


                        if (innerTemp.Count != 0)
                        {
                            dolls.Add(new List<Doll>(innerTemp));
                        }
                        else
                        {
                            visited.Clear();
                            visited[source[i]] = temp;
                        }
                        source.Remove(source[i]);
                    }
                }
                else i++;


            }

            return dolls;
        }
        void Visit(Doll item, ref Dictionary<Doll, List<Doll>> visited, int numDolls)
        {
            if (!visited.ContainsKey(item) && numDolls > 0)
            {
                visited[item] = new List<Doll> { item };
                var dependencies = new List<Doll>(item.Children);
                dependencies.Sort();
                if (dependencies.Count != 0)
                {
                    Dictionary<Doll, List<Doll>> children = new Dictionary<Doll, List<Doll>>();
                    foreach (var dependency in dependencies)
                    {

                        Visit(dependency, ref visited, numDolls - 1);
                        if (visited.ContainsKey(dependency) && visited[dependency].Count == numDolls - 1)
                            children[dependency] = visited[dependency];
                    }
                    if (children.Keys.Count != 0)
                    {
                        visited[item].AddRange(children[children.Keys.First()]);
                    }

                }

            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            ConsoleWorker worker = CreateTestScanner();
            worker.DoWork();
            Console.ReadKey();
#else
                                        ConsoleWorker worker = CreateScanner();
                                        worker.DoWork();
#endif



        }



        private static Problem CreateTestScanner()
        {
            return new Problem(new TestScanner(new List<string>
            {
                "3",
                "100 100 3",
                "97 97 3",
                "94 94 3",
                "91 91 3",
                "88 88 3",
                "85 85 3",
                "5",
                "100 100 1",
                "97 97 3",
                "98 98 1",
                "96 96 1",
                "94 94 1",
                "92 92 1",
                "90 90 1",
                "88 88 1",
                "86 86 1",
                "84 84 1",
                "0"
            }));

        }
        private static Problem CreateScanner()
        {
            return new Problem(new Scanner());
        }
    }
    internal class ListEqualityComparer : IEqualityComparer<List<Doll>>
    {
       

        public ListEqualityComparer()
        {
            
        }
        public bool Equals(List<Doll> x, List<Doll> y)
        {
            if (x == y) return true;
            if (x == null) return false;
            if (y == null) return false;
            return y.SequenceEqual(x);
        }

        public int GetHashCode(List<Doll> obj)
        {
            throw new NotImplementedException();
        }

    }
}



