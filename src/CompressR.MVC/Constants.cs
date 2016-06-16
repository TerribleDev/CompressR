using System;
using System.Collections.Generic;
using System.Text;

namespace CompressR.MVC
{
    public struct Constants
    {
        public const string Gzip = "gzip";
        public const string Deflate = "deflate";
        public const string ContentEncoding = "Content-Encoding";
        public const string AcceptEncoding = "Accept-Encoding";
        public static readonly string[] Compressors = { Gzip, Deflate };
    }
}