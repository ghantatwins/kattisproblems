using System;
using System.Collections.Generic;
using CapGLib;
using RussianDolls;


namespace Terminal
{
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
