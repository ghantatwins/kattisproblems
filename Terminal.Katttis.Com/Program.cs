using System;
using System.Collections.Generic;
using System.IO;



namespace Terminal.Katttis.Com
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        IScanner scanner = DollFactory.TestCase;
    //        IProblem<IItem> russianDolls = new NestedThread(2,new TerminalWriter());
    //        russianDolls.Solve(new DollFactory(scanner));
    //        Console.Read();
    //    }
    //}
    using System.Text;


    namespace Terminal
    {
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

        public class Problem : ConsoleWorker
        {
            private DollComparer _comparer;
            private int _numDolls;

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
                    if (_h == d.H && _d == d.D && _w == d.W)
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


                public void Add(Doll next)
                {
                    _children.Add(next);
                }
            }

            public Problem(IScanner scanner) : base(scanner)
            {
                _comparer = new DollComparer();

            }

            public override void DoWork()
            {
                bool first = true;

                while (true)
                {
                    List<Doll> nestedDolls = new List<Doll>();
                    _numDolls = Scanner.NextInt();
                    if (_numDolls == 0) break;


                    for (int i = 0; i < 2 * _numDolls; i++)
                    {
                        int j = 0;
                        var H = Scanner.NextInt();
                        var D = Scanner.NextInt();
                        var W = Scanner.NextInt();
                        var newDoll = new Doll(H, D, W);

                        nestedDolls.Add(newDoll);


                    }
                    nestedDolls.Sort(_comparer);
                    foreach (var nested in nestedDolls)
                    {
                        foreach (var next in nestedDolls)
                        {
                            if (nested.CanContain(next))
                            {
                                nested.Add(next);
                            }

                        }
                    }


                    IEnumerable<IEnumerable<Doll>> sets = LongestChains(nestedDolls, _numDolls, _comparer);
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



            IEnumerable<IEnumerable<Doll>> LongestChains(List<Doll> dolls, int numDolls, IComparer<Doll> dollComparer)
            {
                return Sort(dolls, new DollEqualityComparer(), true);

            }

            private IEnumerable<List<Doll>> ReverseSplit(IList<Doll> sorted, int collection, IComparer<Doll> dollComparer)
            {
                var reverse = new List<Doll>(sorted);
                for (int i = 0; i < reverse.Count; i += collection)
                {
                    yield return SubListSorted(collection, reverse, i, dollComparer);
                }
            }

            private static List<Doll> SubListSorted(int collection, List<Doll> reverse, int i,
                IComparer<Doll> dollComparer)
            {
                var toSort = reverse.GetRange(i, Math.Min(collection, reverse.Count - i));
                toSort.Sort(dollComparer);
                return toSort;
            }


            IList<List<Doll>> Sort(IEnumerable<Doll> source, IEqualityComparer<Doll> comparer = null, bool ignoreCycles = false)
            {

                var visited = new Dictionary<Doll, List<Doll>>(comparer);
                List<List<Doll>> dolls = new List<List<Doll>>();
                foreach (var item in source)
                {
                    Visit(item, ref visited, ignoreCycles);
                    if (visited[item].Count>_numDolls)
                    {
                        while (visited[item].Count > _numDolls)
                            visited[item].Remove(visited[item][visited[item].Count - 1]);
                            
                        dolls.Add(visited[item]);
                    }
                    
                    visited = new Dictionary<Doll, List<Doll>>();
                }

                return dolls;
            }
            void Visit(Doll item, ref Dictionary<Doll, List<Doll>> visited, bool ignoreCycles)
            {
                List<Doll> inProcess;
                var alreadyVisited = visited.TryGetValue(item, out inProcess);

                if (alreadyVisited)
                {
                    if (inProcess.Count != 0)
                    {
                        if (!inProcess.Contains(item) && inProcess.Count < _numDolls)
                        {
                            inProcess.Insert(0, item);
                        }


                    }
                }
                else
                {
                    visited[item] = new List<Doll> { item };
                    var dependencies = item.Children;
                    if (dependencies != null)
                    {
                        foreach (var dependency in dependencies)
                        {
                            Visit(dependency, ref visited, ignoreCycles);
                            var temp = visited[item];
                            visited[item].AddRange(visited[dependency]);
                            if (visited[item].Count > _numDolls)
                            {
                                visited[item] = temp;
                            }
                            visited.Remove(dependency);
                        }

                    }



                }
            }


        }

        public sealed class DollEqualityComparer : IEqualityComparer<Problem.Doll>
        {
            public bool Equals(Problem.Doll x, Problem.Doll y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.D == y.D && x.H == y.H && x.W == y.W;
            }

            public int GetHashCode(Problem.Doll obj)
            {
                unchecked
                {
                    var hashCode = obj.D;
                    hashCode = (hashCode * 397) ^ obj.H;
                    hashCode = (hashCode * 397) ^ obj.W;
                    return hashCode;
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
    }
}