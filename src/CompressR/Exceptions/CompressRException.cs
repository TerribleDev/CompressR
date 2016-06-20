using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompressR.Exceptions
{
    public class CompressRException: Exception
    {
		public CompressRException()
		{
		}

		public CompressRException(string message) : base(message) { }

		public CompressRException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected CompressRException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
