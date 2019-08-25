using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Probelms.Kattis.Com.Core
{
    public class TerminalWriter : StreamWriter, IPrinter
    {
        public TerminalWriter(Stream stream) : base(stream)
        {
        }

        public TerminalWriter() : this(Console.OpenStandardError())
        {

        }

        public TerminalWriter(string filePath) : this(new FileStream(filePath, FileMode.CreateNew))
        {

        }
    }
}
