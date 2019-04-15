using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using Aml.Container.Exceptions;
using Aml.Contracts;

namespace Aml.Container.Files
{
	public class IntermediateDocumentFile : IDocumentFile
	{
		#region Fields

		private readonly string _fullPath;
		private readonly Uri _location;
		private PackagePart _part;

		#endregion // Fields

		#region Properties

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
		public DocumentFileType Type { get; set; }

		internal PackagePart Part => _part;

		#endregion // Properties

		#region Ctor & Dtor

		/// <summary>
		/// Initializes a new instance of the <see cref="IntermediateDocumentFile"/> class.
		/// </summary>
		/// <param name="workingDirectory">The working directory.</param>
		/// <param name="relativePath">The relative path.</param>
		/// <exception cref="System.ArgumentNullException">
		/// workingDirectory
		/// or
		/// relativePath
		/// </exception>
		/// <exception cref="System.IO.FileNotFoundException">File not found within working directory.</exception>
		internal IntermediateDocumentFile(string workingDirectory, string relativePath)
		{
			if (workingDirectory == null) throw new ArgumentNullException(nameof(workingDirectory));
			if (relativePath == null) throw new ArgumentNullException(nameof(relativePath));

			// get root path and ensure it ends with a path separator
			var root = Path.GetFullPath(workingDirectory);
			if (!root.EndsWith(Path.DirectorySeparatorChar.ToString())) root += Path.DirectorySeparatorChar;

			// determine full path to file
			_fullPath = Path.GetFullPath(Path.Combine(root, relativePath));
			if (!System.IO.File.Exists(_fullPath)) throw new FileNotFoundException("File not found within working directory.", relativePath);

			// set location
			var uri = new Uri(root).MakeRelativeUri(new Uri(_fullPath));
			_location = DocumentFileCollection.ConvertUri(uri);
		}

		#endregion // Ctor & Dtor

		#region Public API

		/// <summary>
		/// Gets the stream to the file content.
		/// </summary>
		/// <returns>A stream to the data.</returns>
		/// <remarks>The stream must be disposed.</remarks>
		/// <remarks>The stream will be read-only.</remarks>
		public Stream GetStream()
		{
			if (_part != null) return _part.GetStream(FileMode.Open);

			Debug.Assert(_fullPath != null);
			if (!System.IO.File.Exists(_fullPath)) throw new AmlxCorruptedDocumentException("File not existing any more: " + _location);

			var stream = new FileStream(_fullPath, FileMode.Open);
			return stream;
		}

		#endregion // Public API

		#region Internal Methods

		/// <summary>
		/// Embeds the related file into the specified package.
		/// </summary>
		/// <param name="package">The package.</param>
		/// <exception cref="AmlxCorruptedDocumentException">File not existing any more:  + _location</exception>
		internal void Embed(Package package)
		{
			// check if the file was already embedded
			if (_part != null) return;

			if (!System.IO.File.Exists(_fullPath)) throw new AmlxCorruptedDocumentException("File not existing any more: " + _location);

			// create part and add content
			var mimetype = XMLMimeTypeMapper.GetMimeType(_fullPath);
			var uri = PackUriHelper.CreatePartUri(_location);
			_part = package.CreatePart(uri, mimetype, CompressionOption.Normal);
			if(_part == null) throw new AmlxException("Cannot create part for mimetype " + mimetype);

			using (var stream = _part.GetStream(FileMode.Create))
			{
				using (var reader = new FileStream(_fullPath, FileMode.Open, FileAccess.Read))
				{
					reader.CopyStreamTo(stream);
				}
			}

			// remove origin
			System.IO.File.Delete(_fullPath);
		}

		#endregion // Internal Methods
	}
}