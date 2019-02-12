using System;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;

namespace Aml.Container.Files
{
	public class PackageDocumentFile : IDocumentFile
	{
		private readonly PackagePart _part;
		private readonly Uri _location;

		/// <summary>
		/// Gets the relative location within the container.
		/// </summary>
		[Browsable(false)]
		public Uri Location => _location;

		/// <summary>
		/// Gets the relative path within the container.
		/// </summary>
		public string ContainerPath => _location.ToString();

		/// <summary>
		/// Gets the type of the file.
		/// </summary>
		[ReadOnly(true)]
		public DocumentFileType Type { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PackageDocumentFile"/> class.
		/// </summary>
		/// <param name="part">The part.</param>
		/// <exception cref="System.ArgumentNullException">part</exception>
		internal PackageDocumentFile(PackagePart part)
		{
			if (part == null) throw new ArgumentNullException(nameof(part));
			_part = part;
			_location = new Uri("." + _part.Uri, UriKind.Relative);
		}

		/// <summary>
		/// Gets the stream to the file content.
		/// </summary>
		/// <returns>A stream to the data.</returns>
		/// <remarks>The stream must be disposed.</remarks>
		/// <remarks>The stream will be read-only.</remarks>
		public Stream GetStream()
		{
			return _part.GetStream(FileMode.Open);
		}
	}
}