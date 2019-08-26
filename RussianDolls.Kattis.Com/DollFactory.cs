using System.Collections.Generic;
using Probelms.Kattis.Com.Core;

namespace RussianDolls.Kattis.Com
{
    public class DollFactory:IFactory<IItem>
    {
        private readonly IScanner _scanner;
        private int _totalTimes;


        public DollFactory(IScanner scanner)
        {
            _scanner = scanner;
            _totalTimes = -999999999;
        }

        public int TotalItems
        {
            get
            {
                if(_totalTimes== -999999999)_totalTimes= _scanner.NextInt();
                return _totalTimes;
            }
        }

        public IItem Create()
        {
            var height = _scanner.NextInt();
            var diameter = _scanner.NextInt();
            var width = _scanner.NextInt();
            return new Doll(height, diameter, width);

        }

        

        public IEqualityComparer<IItem> EqualityComparer
        {
            get
            {
                return new DollEqualityComparer();
            }
        }

        public IComparer<IItem> Comparer
        {
            get { return new DollComparer(); }
        }

        public void Reset()
        {
            _totalTimes = -999999999;
        }

        public static IScanner TestCase
        {
            get
            {
                return new TestScanner(new List<string>
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
                });
            }
        }
    }
}