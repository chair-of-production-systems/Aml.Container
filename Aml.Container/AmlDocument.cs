using System;
using System.IO;
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

			throw new NotImplementedException();
		}
	}
}