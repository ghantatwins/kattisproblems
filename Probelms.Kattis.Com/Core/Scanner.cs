using System;

namespace Probelms.Kattis.Com.Core
{
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
}