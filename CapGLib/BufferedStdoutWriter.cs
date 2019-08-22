using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CapGLib
{
    public class TestScanner : Tokenizer
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
        public override string NextLine()
        {
            return GetCurrent();
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

    public class Scanner : Tokenizer
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
}