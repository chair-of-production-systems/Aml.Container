using System;
using System.IO;

namespace Aml.Contracts
{
	public interface ILocationResolver
	{
		/// <summary>
		/// Gets a <see cref="Stream"/> to read data from the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>A <see cref="Stream"/> instance that is read-only.</returns>
		Stream GetStream(Uri location);
	}
}