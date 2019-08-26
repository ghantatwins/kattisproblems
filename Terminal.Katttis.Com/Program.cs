using System;
using Probelms.Kattis.Com;
using Probelms.Kattis.Com.Core;
using RussianDolls.Kattis.Com;

namespace Terminal.Katttis.Com
{
    class Program
    {
        static void Main(string[] args)
        {
            IScanner scanner = DollFactory.TestCase;
            IProblem<IItem> russianDolls = new NestedThread(2,new TerminalWriter());
            russianDolls.Solve(new DollFactory(scanner));
            Console.Read();
        }
    }
}
