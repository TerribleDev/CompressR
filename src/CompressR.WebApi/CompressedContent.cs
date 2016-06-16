using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CompressR.WebApi
{
    public class CompressedContent : HttpContent
    {
        private readonly string _encodingType;
        private readonly HttpContent _originalContent;

        public CompressedContent(HttpContent content, string encodingType = "gzip")
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            _originalContent = content;
            _encodingType = encodingType.ToLowerInvariant();

            foreach (var header in _originalContent.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            Headers.ContentEncoding.Add(encodingType);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;
            switch (_encodingType)
            {
                case Constants.Gzip:
                    compressedStream = new GZipStream(stream, CompressionMode.Compress, true);
                    break;

                case Constants.Deflate:
                    compressedStream = new DeflateStream(stream, CompressionMode.Compress, true);
                    break;

                default:
                    compressedStream = stream;
                    break;
            }

            return _originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }
    }
}