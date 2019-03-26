using System;
using System.IO;
using Aml.Engine.CAEX;

namespace Aml.Contracts
{
	/// <summary>
	/// Interface to provide general interactions with AutomationML document.
	/// </summary>
	public interface IAmlProvider
	{
		/// <summary>
		/// Reference to the CAEX document
		/// </summary>
		CAEXDocument CaexDocument { get; }

		/// <summary>
		/// Gets a reference to a <see cref="Stream"/> of the document accessible by the specified location.
		/// </summary>
		/// <param name="location"></param>
		/// <returns>New <see cref="Stream"/> instance.</returns>
		Stream GetStream(Uri location);
	}
}