using System;
using System.IO;
using System.Net.Http;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.Container
{
	public class AmlDocument : IAmlProvider
	{
		/// <summary>
		/// Reference to the CAEX document
		/// </summary>
		public CAEXDocument CaexDocument { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="AmlDocument"/> class.
		/// </summary>
		public AmlDocument()
		{
			CaexDocument = CAEXDocument.New_CAEXDocument(CAEXDocument.CAEXSchema.CAEX3_0);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="AmlDocument"/> class with a given <see cref="CaexDocument"/> instance.
		/// </summary>
		public AmlDocument(CAEXDocument document)
		{
			CaexDocument = document;
		}

		/// <summary>
		/// Gets a reference to a <see cref="Stream"/> of the document accessible by the specified location.
		/// </summary>
		/// <param name="location"></param>
		/// <returns>New <see cref="Stream"/> instance.</returns>
		public Stream GetStream(Uri location)
		{
			if (!location.IsAbsoluteUri)
			{
				// search relative to the CAEX document
				throw new NotImplementedException();
			}

			if (location.Scheme == Uri.UriSchemeFile)
			{
				return File.Open(location.AbsolutePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}

			var handler = new HttpClientHandler
			{
				UseDefaultCredentials = true
			};
			using (var client = new HttpClient(handler))
			{
				return location.GetStreamForAbsoluteUri(client);
			}
		}
	}
}