using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Probelms.Kattis.Com.Core
{
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
}