using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using Aml.Container.Exceptions;
using Aml.Contracts;

namespace Aml.Container.Files
{
	/// <summary>
	/// This class represents a collection of files. The files may resist either within an amlx-package
	/// or within a temporary working directory. Nevertheless where the file is located, the access should
	/// be the same.
	/// </summary>
	public class DocumentFileCollection : IDisposable, IEnumerable<IDocumentFile>, INotifyCollectionChanged
	{
		#region Fields

		private bool _disposed;
		private readonly List<IDocumentFile> _files;
		private string _workingDirectory;
		private readonly List<PackageDocumentFile> _removedFiles;

		#endregion // Fields

		#region Properties

		/// <summary>
		/// Gets the number of files within this collection.
		/// </summary>
		public int Count => _files.Count;

		/// <summary>
		/// Gets the element at the specified index. If using fixed size, an exception will be thrown
		/// in case of invalid index position.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Invalid index (fixed size mode)  + index.ToString(CultureInfo.InvariantCulture)</exception>
		public IDocumentFile this[int index]
		{
			get
			{
				if (index < 0 || index >= _files.Count) throw new ArgumentOutOfRangeException(nameof(index));
				return _files[index];
			}
		}

		#endregion // Properties

		#region Ctor & Dtor

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentFileCollection"/> class.
		/// </summary>
		public DocumentFileCollection()
		{
			_files = new List<IDocumentFile>();
			_removedFiles = new List<PackageDocumentFile>();
		}

		/// <summary>
		/// Implement IDisposable. Invoked when this object is being removed from 
		/// the application and will be subject to garbage collection.
		/// </summary>
		/// <remarks>
		/// <b>Do not make this method virtual.</b>
		/// A derived class should not be able to override this method.
		/// </remarks>
		public void Dispose()
		{
			Dispose(true);

			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SuppressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose(bool disposing) executes in two distinct scenarios.
		/// If disposing equals true, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// If disposing equals false, the method has been called by the
		/// runtime from inside the finalizer and you should not reference
		/// other objects. Only unmanaged resources can be disposed.
		/// 
		/// Child classes must override this method to perform 
		/// clean-up logic, such as removing event handlers.
		/// </summary>
		/// <param name="disposing">if set to <c>true</c>, releases managed resources.</param>
		/// <remarks>
		/// Override this method in inherited classes
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					if (_workingDirectory != null)
					{
						Directory.Delete(_workingDirectory, true);
						_workingDirectory = null;
					}
				}
				// Dispose unmanaged resources.

				// Call the appropriate methods to clean up unmanaged resources here.
				// If disposing is false, only the following code is executed.

				// Note disposing has been done.
				_disposed = true;
			}
		}

		/// <summary>
		/// Use C# destructor syntax for finalization code.
		/// This destructor will run only if the Dispose method
		/// does not get called.
		/// It gives your base class the opportunity to finalize.
		/// Do not provide destructors in types derived from this class.
		/// </summary>
		~DocumentFileCollection()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}

		#endregion // Ctor & Dtor

		#region Public API

		public static Uri ConvertUri(Uri uri)
		{
			if (uri == null) return null;
			var partUri = PackUriHelper.CreatePartUri(uri);

			var relative = partUri.ToString();
			if (relative.StartsWith("/")) relative = "." + relative;
			if (!relative.StartsWith("./")) relative = "./" + relative;

			return new Uri(relative, UriKind.Relative);
		}

		/// <summary>
		/// Add the file at the specified location to this collection.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="packagePath"></param>
		/// <exception cref="System.NotImplementedException"></exception>
		/// <exception cref="AmlxLocationException">Relative uri not allowes:  + location</exception>
		public IDocumentFile Add(Uri location, string packagePath = null)
		{
			if (location.Scheme != Uri.UriSchemeFile) throw new NotImplementedException();
			if (!location.IsAbsoluteUri) throw new AmlxLocationException("Relative uri not allowes: " + location);

			var file = AddLocalFile(location, packagePath);
			return file;
		}

		/// <summary>
		/// Add the file at the specified file to this collection.
		/// </summary>
		/// <param name="fullPath">The full path.</param>
		/// <param name="packagePath">The package path.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.IO.FileNotFoundException">File not found</exception>
		public IDocumentFile Add(string fullPath, string packagePath = null)
		{
			if (fullPath == null) throw new ArgumentNullException(nameof(fullPath));
			if (!System.IO.File.Exists(fullPath)) throw new FileNotFoundException("File not found", fullPath);
			return Add(new Uri(fullPath, UriKind.Absolute), packagePath);
		}

		/// <summary>
		/// Removes the specified file.
		/// </summary>
		/// <param name="file">The file.</param>
		public void Remove(IDocumentFile file)
		{
			_files.Remove(file);

			var packageDocumentFile = file as PackageDocumentFile;
			if (packageDocumentFile != null) _removedFiles.Add(packageDocumentFile);

			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, file));
		}

		public IEnumerator<IDocumentFile> GetEnumerator()
		{
			return _files.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _files.GetEnumerator();
		}

		#endregion // Public API

		#region Private Methods

		internal List<PackageDocumentFile> GetRemovedFiles()
		{
			return _removedFiles;
		}

		internal void AddPackageFiles(Package package, PackagePart rootFile)
		{
			if (package == null) throw new ArgumentNullException(nameof(package));

			// get all supplementary files related to the root file
			var relations = rootFile.GetRelationships();

			// get all parts
			var parts = package.GetParts();

			// add all supplementary files
			foreach (var relation in relations)
			{
				DocumentFileType type;
				if (!relation.TryParseDocumentFileType(out type)) continue;

				var part = parts.FirstOrDefault(p => PackUriHelper.ComparePartUri(p.Uri, relation.TargetUri) == 0);
				if (part != null) _files.Add(new PackageDocumentFile(part) { Type = type });
			}
		}

		/// <summary>
		/// Internal method to be used to add a file to this collection.
		/// </summary>
		/// <param name="file">The file.</param>
		internal void Add(IDocumentFile file)
		{
			_files.Add(file);
			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, file));
		}

		internal IDocumentFile AddResourceFile(Package package, string resourceName, string relativePath, DocumentFileType type)
		{
			Debug.Assert(resourceName != null);

			// return if part already exists
			var uri = new Uri("./" + relativePath, UriKind.Relative);
			var file = _files.FirstOrDefault(x => x.Location == uri);
			if (file != null) return file;

			if (package != null)
			{
				uri = PackUriHelper.CreatePartUri(uri);
				if (package.PartExists(uri))
				{
					var part = package.GetPart(uri);
					file = new PackageDocumentFile(part) { Type = type };
				}
			}

			if (file == null)
			{
				// copy file to working folder and add to list
				var targetFullPath = ResolveRelativePath(relativePath);

				var assembly = typeof(AmlxDocument).Assembly;
				using (var stream = assembly.GetManifestResourceStream(resourceName))
				{
					if (stream == null) throw new AmlxException("Could not extract resource file");
					using (var writer = new FileStream(targetFullPath, FileMode.Create, FileAccess.Write))
					{
						stream.CopyTo(writer);
					}
				}

				file = new IntermediateDocumentFile(_workingDirectory, relativePath) { Type = type };
			}

			_files.Add(file);
			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, file));

			return file;
		}

		private IDocumentFile AddLocalFile(Uri location, string relativePath = null)
		{
			Debug.Assert(location != null);
			Debug.Assert(location.Scheme == Uri.UriSchemeFile);
			Debug.Assert(location.IsAbsoluteUri);

			// check file
			var sourceFullPath = Path.GetFullPath(location.LocalPath);
			if (!System.IO.File.Exists(sourceFullPath)) throw new FileNotFoundException("File not found.", sourceFullPath);

			// copy file to working folder and add to list
			if (string.IsNullOrEmpty(relativePath))
			{
				relativePath = Path.Combine(Guid.NewGuid().ToString(AmlxConstants.GuidFormatFileOrDirectory), Path.GetFileName(sourceFullPath));
			}
			var targetFullPath = ResolveRelativePath(relativePath);

			System.IO.File.Copy(sourceFullPath, targetFullPath);
			var file = new IntermediateDocumentFile(_workingDirectory, relativePath);
			_files.Add(file);
			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, file));

			return file;
		}

		/// <summary>
		/// Adds a copy of the specified file to this collection.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="package"></param>
		/// <returns></returns>
		internal IDocumentFile AddCopy(IDocumentFile file, Package package)
		{
			var partUri = PackUriHelper.CreatePartUri(file.Location);
			var mimetype = XMLMimeTypeMapper.GetMimeType(partUri.ToString());

			if (package.PartExists(partUri)) package.DeletePart(partUri);

			var part = package.CreatePart(partUri, mimetype, CompressionOption.Normal);
			if (part == null) throw new AmlxCorruptedDocumentException("Cannot create part (null).");
			using (var source = file.GetStream())
			{
				using (var target = part.GetStream(FileMode.Create, FileAccess.Write))
				{
					source.CopyTo(target);
				}
			}
				var copy = new PackageDocumentFile(part);
			Add(copy);
			return copy;

			//var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			//using (var stream = file.GetStream())
			//{
			//	using (var writer = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
			//	{
			//		stream.CopyTo(writer);
			//	}
			//}
			//var copy = Add(tempPath, file.Location.ToString());
			//return copy;
		}

		/// <summary>
		/// Resolves a relative path within the working directory. If the corresponding 
		/// directory is not created yet, it will become created. This method will also
		/// ensure, that the working directory is defined properly.
		/// </summary>
		/// <param name="relativePath">Relative path within the working directory.</param>
		/// <returns>The full path to the working directory</returns>
		/// <exception cref="AmlxLocationException">Cannot create working directory.</exception>
		private string ResolveRelativePath(string relativePath)
		{
			if (_workingDirectory == null) _workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Debug.Assert(_workingDirectory != null);

			var fullPath = Path.GetFullPath(Path.Combine(_workingDirectory, relativePath));
			var directoryFullPath = Path.GetDirectoryName(fullPath);
			Debug.Assert(directoryFullPath != null);

			if (!Directory.Exists(directoryFullPath)) Directory.CreateDirectory(directoryFullPath);
			if (!Directory.Exists(directoryFullPath)) throw new AmlxLocationException("Cannot create directory.");

			return fullPath;
		}

		//private void AddDocumentFile(IDocumentFile file)
		//{
		//	var relationShip = _package.CreateRelationship(file.Location, TargetMode.Internal, "http://schemas.conexing.de/package/2014/relationships/removedFile");
		//}

		#endregion // Private Methods

		#region INotifyCollectionChanged

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			var handler = CollectionChanged;
			if (handler != null) handler(this, args);
		}

		#endregion // INotifyCollectionChanged
	}
}