using System;
using System.IO;

namespace Aml.Container.Files
{
	/// <summary>
	/// This interface represents a file, which was added to the AmlX document. The file may resist within
	/// the container itself or in a temporary folder, if the document was not saved yet.
	/// </summary>
	public interface IDocumentFile
	{
		/// <summary>
		/// Gets the relative location within the container.
		/// </summary>
		Uri Location { get; }

		/// <summary>
		/// Gets the type of the file.
		/// </summary>
		DocumentFileType Type { get; }

		/// <summary>
		/// Gets the stream to the file content.
		/// </summary>
		/// <returns>A stream to the data.</returns>
		/// <remarks>The stream must be disposed.</remarks>
		/// <remarks>The stream will be read-only.</remarks>
		Stream GetStream();
	}
}