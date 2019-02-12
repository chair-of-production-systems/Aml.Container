using System;
using System.Runtime.Serialization;

namespace Aml.Container.Exceptions
{
	[Serializable]
	public class AmlxException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public AmlxException()
		{
		}

		public AmlxException(string message) : base(message)
		{
		}

		public AmlxException(string message, Exception inner) : base(message, inner)
		{
		}

		protected AmlxException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}