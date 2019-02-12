using System;
using System.Runtime.Serialization;

namespace Aml.Container.Exceptions
{
	[Serializable]
	public class AmlxCorruptedDocumentException : AmlxException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public AmlxCorruptedDocumentException()
		{
		}

		public AmlxCorruptedDocumentException(string message) : base(message)
		{
		}

		public AmlxCorruptedDocumentException(string message, Exception inner) : base(message, inner)
		{
		}

		protected AmlxCorruptedDocumentException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}