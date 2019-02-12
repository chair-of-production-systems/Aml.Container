using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Aml.Container.Exceptions;
using Aml.Container.Files;
using Aml.Engine.CAEX;

namespace Aml.Container
{
	/// <summary>
	/// This class represents the main class to be used for access to an AutomationML container. 
	/// </summary>
	public class AmlxDocument : IDisposable, INotifyPropertyChanged
	{
		#region Consts

		private const string AmlSchemeResourceName = "VCTK.AutomationML.Container.Resources.CAEX_ClassModel_V2.15.xsd";
		private const string AmlLibraryResourceName = "VCTK.AutomationML.Container.Resources.AutomationMLStandardLibrary.aml";

		#endregion // Consts

		#region Fields

		private bool _disposed;
		private Uri _location;
		private Package _package;
		private string _packageFullPath; // local full path to the package file

		private CAEXDocument _caexDocument;
		private readonly DocumentFileCollection _files;
		//private readonly VersionCollection _versions;

		#endregion // Fields

		#region Properties

		/// <summary>
		/// Gets the filename of this document or <c>null</c> if not set (e.g. in case of newly created documents).
		/// </summary>
		public string Filename => Path.GetFileName(_location?.ToString());

		/// <summary>
		/// Gets or sets the current working CAEX document.
		/// </summary>
		public CAEXDocument CaexDocument
		{
			get => _caexDocument;
			set
			{
				if (_caexDocument == value) return;
				_caexDocument = value;
				RaisePropertyChanged("CAEX");
			}
		}

		/// <summary>
		/// Gets the collection of files, included within this document.
		/// </summary>
		public DocumentFileCollection Files => _files;

		///// <summary>
		///// Gets the collection of available versions.
		///// </summary>
		//public VersionCollection Versions => _versions;

		/// <summary>
		/// Gets or sets the certificate.
		/// </summary>
		public X509Certificate2 Certificate { get; set; }

		#endregion // Properties

		#region Ctor & Dtor

		/// <summary>
		/// Initializes a new instance of the <see cref="AmlxDocument"/> class.
		/// </summary>
		public AmlxDocument()
		{
			//var document = CAEXDocument.New_Document();
			//var myIH = document.CAEXFile.InstanceHierarchy.Append("myIH");
			//var myIE = myIH.InternalElement.Append("myIE");

			//_caexDocument = CAEXDocument.New_Document();
			_files = new DocumentFileCollection();
			//_versions = new VersionCollection();
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
					if (_package != null)
					{
						_package.Close();
						_package = null;
					}
					_files.Dispose();
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
		/// Do not provide a destructor in types derived from this class.
		/// </summary>
		~AmlxDocument()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}

		#endregion // Ctor & Dtor

		public static AmlxDocument Open(string fullPath)
		{
			var location = new Uri(fullPath, UriKind.Absolute);
			var document = Open(location);
			return document;
		}

		public static AmlxDocument Open(Uri location)
		{
			if (location == null) throw new ArgumentNullException(nameof(location));

			var fullPath = location.LocalPath;
			var document = new AmlxDocument
			{
				_location = location,
				_packageFullPath = fullPath
			};

			if (location.Scheme != Uri.UriSchemeFile) throw new NotImplementedException("Only file scheme allowed at the moment.");
			if (!location.IsAbsoluteUri) throw new Exception("Only absolute locations are allowed.");

			if (!System.IO.File.Exists(fullPath)) throw new FileNotFoundException("File not found: " + fullPath);

			document._package = Package.Open(fullPath, FileMode.Open, FileAccess.ReadWrite);

			var root = document.GetAmlEntryPart();
			if (root == null) throw new AmlxCorruptedDocumentException("No main entry part found within container.");
			using (var stream = root.GetStream())
			{
				document._caexDocument = CAEXDocument.LoadFromStream(stream);
			}

			document._files.AddPackageFiles(document._package, root);
			//document._versions.InspectRootPart(document._package, root);

			return document;
		}

		public static AmlxDocument Create()
		{
			var document = new AmlxDocument();
			document.AddCaexScheme();
			return document;
		}

		public static AmlxDocument OpenOrCreate(string fullPath)
		{
			if (System.IO.File.Exists(fullPath)) return Open(fullPath);

			var document = Create();
			return document;
		}

		/// <summary>
		/// Saves the document.
		/// </summary>
		public void Save()
		{
			if (_location == null) throw new Exception("Cannot save to unknown location (null). Use SaveAs().");
			if (_package == null) throw new AmlxCorruptedDocumentException("Cannot save: use SaveAs().");

			// get or create root file part
			var root = GetAmlEntryPart();
			if (root == null)
			{
				// create new root part
				var uri = new Uri(GetNewAmlPath(), UriKind.Relative);
				uri = PackUriHelper.CreatePartUri(uri);
				root = _package.CreatePart(uri, AmlxConstants.MainEntryContentType);
				if (root == null) throw new AmlxException("Cannot create package vor mimetype " + AmlxConstants.MainEntryContentType);

				// create main entry file relationship
				_package.CreateRelationship(root.Uri, TargetMode.Internal, AmlxConstants.MainEntryRelationShipType);
			}
			Debug.Assert(root != null);

			// save CAEX
			SaveCaexFile(root, _caexDocument);
			_package.Flush();
		}

		/// <summary>
		/// Saves the document to a new location.
		/// </summary>
		/// <param name="fullPath">The full path to save the document to.</param>
		/// <param name="overwrite">If set to <c>true</c>, an existing file at the specified location becomes overwritten.</param>
		public void SaveAs(string fullPath, bool overwrite = true)
		{
			SaveAs(new Uri(fullPath, UriKind.Absolute), overwrite);
		}

		/// <summary>
		/// Saves the document to a new location.
		/// </summary>
		/// <param name="location">The location to save the document to.</param>
		/// <param name="overwrite">If set to <c>true</c>, an existing file at the specified location becomes overwritten.</param>
		public void SaveAs(Uri location, bool overwrite = true)
		{
			if (location == null) throw new ArgumentNullException(nameof(location));
			if (location.Scheme != Uri.UriSchemeFile) throw new Exception("Cannot save to non-file locations (" + location + ").");
			if (!location.IsAbsoluteUri) throw new Exception("Cannot save to relative location (" + location + ").");

			// if location has not changed, just save
			if (_package != null && location == _location)
			{
				Save();
				return;
			}

			// determine local path to save package to
			var fullPath = location.LocalPath;

			if (System.IO.File.Exists(fullPath))
			{
				if (!overwrite) throw new AmlxException("File already exists.");
				System.IO.File.Delete(fullPath);
			}

			// copy current package if existing
			if (_package != null)
			{
				Debug.Assert(_packageFullPath != null);
				if (!System.IO.File.Exists(_packageFullPath)) throw new AmlxCorruptedDocumentException("Original document not found at " + _packageFullPath);

				_package.Close();
				System.IO.File.Copy(_packageFullPath, fullPath);
			}

			// create or open target package
			_package = Package.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			// update location
			_location = location;
			_packageFullPath = fullPath;

			// save document
			Save();

			RaisePropertyChanged("Filename");
		}

		///// <summary>
		///// Commits the current changes made to the document. 
		///// </summary>
		///// <param name="author">Name of the author.</param>
		///// <param name="comment">Comment for this commit.</param>
		//public AmlxVersion Commit(string author, string comment)
		//{
		//	if (_location == null) throw new Exception("Cannot save to unknown location (null).");
		//	Debug.Assert(_package != null);

		//	var root = GetAmlEntryPart();
		//	if (root == null) throw new AmlxCorruptedDocumentException("Cannot commit to a never saved file.");
		//	Debug.Assert(root != null);

		//	// create revision information
		//	var revision = new CAEXBasicObjectRevision
		//	{
		//		OldVersion = root.GetRelativePath(),
		//		NewVersion = GetNewAmlPath(),
		//		RevisionDate = DateTime.UtcNow,
		//		Comment = comment,
		//		AuthorName = author,
		//		ChangeMode = ChangeMode.create
		//	};

		//	// determine difference; if no root file exists, the difference is the file itselfe
		//	var oldCaex = _versions.OpenVersion(_versions.GetLastVersion());
		//	var difference = CaexDifferenceVisitor.GetDifference(_caex, oldCaex);
		//	if (difference == null) throw new AmlxEmptyDifferenceException("Empty commit (no change).");

		//	// add revision information
		//	difference.Revision.Add(revision);

		//	// save difference to old file
		//	SaveCaexFile(root, difference);

		//	// remove old main entry file relation
		//	var relationship = _package.GetRelationshipsByType(AmlxConstants.MainEntryRelationShipType).Single();
		//	_package.DeleteRelationship(relationship.Id);

		//	// create new root file part
		//	var uri = PackUriHelper.CreatePartUri(new Uri(revision.NewVersion, UriKind.Relative));
		//	var newRoot = _package.CreatePart(uri, AmlxConstants.MainEntryContentType);
		//	if (newRoot == null) throw new AmlxException("Cannot create package vor mimetype " + AmlxConstants.MainEntryContentType);

		//	// create relationships
		//	_package.CreateRelationship(newRoot.Uri, TargetMode.Internal, AmlxConstants.MainEntryRelationShipType);
		//	_package.CreateRelationship(root.Uri, TargetMode.Internal, AmlxConstants.MainEntryIncrementalVersionRelationShipType);
		//	newRoot.CreateRelationship(root.Uri, TargetMode.Internal, AmlxConstants.PreviousVersionRelationShipType);

		//	// add version
		//	var version = new AmlxVersion(root);
		//	_versions.Add(version);

		//	// save CAEX
		//	SaveCaexFile(newRoot, _caex);
		//	_package.Flush();

		//	return version;
		//}

		public void AddCaexScheme()
		{
			// determine filename
			var prefix = typeof(AmlxDocument).Namespace + ".Resources";
			Debug.Assert(prefix != null);
			var relativePath = AmlSchemeResourceName.Substring(prefix.Length + 1);

			_files.AddResourceFile(_package, AmlSchemeResourceName, relativePath, DocumentFileType.CaexSchemeFile);
		}

		//public void AddStandardLibraries()
		//{
		//	// determine filename
		//	var prefix = typeof(CaexDocument).Namespace;
		//	Debug.Assert(prefix != null);
		//	var relativePath = AmlLibraryResourceName.Substring(prefix.Length + 1);

		//	var doc = _files.AddResourceFile(_package, AmlLibraryResourceName, relativePath, DocumentFileType.ExternalReference);

		//	var reference = _caexDocument.ExternalReferences.FirstOrDefault(x => x.Alias == AmlxConstants.AutomationMLStandardLibraryAlias);
		//	if (reference == null)
		//	{
		//		reference = new ExternalReference { Alias = AmlxConstants.AutomationMLStandardLibraryAlias };
		//		_caexDocument.ExternalReferences.Add(reference);
		//	}
		//	reference.Path = doc.Location.ToString();
		//}

		public void Fix()
		{
			if (_package == null) return;

			AddCaexScheme();
			//AddStandardLibraries();

			//// get all supplementary files related to the root file
			//var rootFile = GetAmlEntryPart();
			//Debug.Assert(rootFile != null);
			//var relations = rootFile.GetRelationshipsByType(AmlxConstants.FileRelationShipType);

			//// get all parts
			//var parts = _package.GetParts();

			//foreach (var reference in CAEX.ExternalReference)
			//{
			//	var uri = PackUriHelper.CreatePartUri(new Uri(reference.Path, UriKind.Relative));
			//	var part = parts.FirstOrDefault(x => x.Uri == uri);
			//	if (part == null) throw new AmlxCorruptedDocumentException("External reference not found: " + reference.Alias);

			//	var rel = relations.FirstOrDefault(x => x.TargetUri == uri);
			//	if (rel != null) continue;

			//	// TODO: we need to change to content type of the external reference
			//	_files.Add(new PackageDocumentFile(part));
			//}
		}

		//public void AddRepository(string repositoryFullPath, string stylesheetFullPath)
		//{
		//	if (repositoryFullPath == null) throw new ArgumentNullException(nameof(repositoryFullPath));
		//	if (stylesheetFullPath == null) throw new ArgumentNullException(nameof(stylesheetFullPath));
		//	if (_package == null) throw new AmlxException("Package not initialized (null).");

		//	// add stylesheet to the container
		//	var stylesheet = _files.Add(stylesheetFullPath) as IntermediateDocumentFile;
		//	if (stylesheet == null) throw new AmlxException("Error adding stylesheet");

		//	// file must be embedded in order to receive a valid package part
		//	stylesheet.Embed(_package);

		//	// add a relation ship from the stylesheet to the (external) repository
		//	var repositoryUri = PackUriHelper.Create(new Uri(repositoryFullPath, UriKind.Absolute));
		//	stylesheet.Part.CreateRelationship(repositoryUri, TargetMode.External, AmlxConstants.RepositoryRelationShipType);
		//}

		//public void RemoveRepository(Uri stylesheetPartUri)
		//{
		//	if (stylesheetPartUri == null) throw new ArgumentNullException(nameof(stylesheetPartUri));
		//	var file = _files.FirstOrDefault(x => x.Location == stylesheetPartUri);
		//	if (file != null) _files.Remove(file);
		//	_package.DeletePart(stylesheetPartUri);
		//}

		//public IEnumerable<Uri> GetRepositories()
		//{
		//	if (_package == null) throw new AmlxException("Package not initialized (null).");
		//	// ReSharper disable once LoopCanBeConvertedToQuery
		//	foreach (var part in _package.GetParts())
		//	{
		//		if (PackUriHelper.IsRelationshipPartUri(part.Uri)) continue;
		//		var relationships = part.GetRelationshipsByType(AmlxConstants.RepositoryRelationShipType);
		//		if (relationships.Any()) yield return part.Uri;
		//	}
		//}

		//public void MergeRepository(Uri stylesheetPartUri)
		//{
		//	var stylesheetPart = _package.GetPart(stylesheetPartUri);
		//	var relationShips = stylesheetPart.GetRelationshipsByType(AmlxConstants.RepositoryRelationShipType);
		//	var relationShip = relationShips.Single();
		//	var uri = PackUriHelper.GetPackageUri(relationShip.TargetUri);

		//	using (var repository = Open(uri))
		//	{
		//		// create revision
		//		AmlxVersion version;
		//		try
		//		{
		//			var asm = typeof (AmlxDocument).Assembly;
		//			var author = $"{asm.FullName}, {asm.ImageRuntimeVersion}";
		//			version = repository.Commit(author, "automatic commit before merge");
		//		}
		//		catch (AmlxEmptyDifferenceException)
		//		{
		//			// use last version
		//			version = repository.Versions.GetLastVersion();
		//		}

		//		// update relationship with new version.Part;
		//		stylesheetPart.DeleteRelationship(relationShip.Id);
		//		var versionUri = PackUriHelper.Create(uri, version.Part.Uri);
		//		stylesheetPart.CreateRelationship(versionUri, TargetMode.External, AmlxConstants.RepositoryRelationShipType);

		//		// get current version
		//		var currentVersion = repository.CAEX;
		//		if (currentVersion != null)
		//		{
		//			currentVersion = XstlTransform.Load(currentVersion, stylesheetPart.GetStream(FileMode.Open, FileAccess.Read), true);
		//		}

		//		// get previously merged CAEX
		//		CAEXFile previousVersion = null;
		//		var partUri = PackUriHelper.GetPartUri(relationShip.TargetUri);
		//		version = repository.Versions.FirstOrDefault(x => x.Part.Uri == partUri);
		//		if (version != null)
		//		{
		//			var caex = repository.Versions.OpenVersion(version);
		//			previousVersion = XstlTransform.Load(caex, stylesheetPart.GetStream(FileMode.Open, FileAccess.Read), true);
		//		}

		//		// build delta
		//		var delta = CaexDifferenceVisitor.GetDifference(currentVersion, previousVersion);

		//		// merge with current CAEX
		//		CaexMergeVisitor.Merge(CAEX, delta);

		//		// merge files
		//		var serializer = new CaexSerializer<AmlColladaInterface>();
		//		var visitor = new AllCaexObjectsVisitor();
		//		delta.Accept(visitor);
		//		var copyiedFiles = new List<IDocumentFile>();
		//		foreach (var iface in visitor.Items.OfType<InterfaceClassType>())
		//		{
		//			var collada = serializer.Deserialize(iface);
		//			if (collada?.Uri == null) continue;

		//			var colladaUri = DocumentFileCollection.ConvertUri(collada.Uri);
		//			var file = repository.Files.FirstOrDefault(x => x.Location == colladaUri);
		//			if (file == null) continue;

		//			if (copyiedFiles.Contains(file)) continue;
		//			copyiedFiles.Add(file);
		//			_files.AddCopy(file, _package);
		//		}
		//	}
		//}

		#region Private Methods

		/// <summary>
		/// Creates a new relative path targeting to an AML file.
		/// </summary>
		/// <returns>Relative path.</returns>
		private static string GetNewAmlPath()
		{
			return "./" + Guid.NewGuid().ToString(AmlxConstants.GuidFormatFileOrDirectory) + AmlxConstants.FileExtensionAml;
		}

		/// <summary>
		/// Searches for the main AML entry part within the currently opened package.
		/// </summary>
		/// <returns>The <see cref="PackagePart"/> with the main AML entry file or <c>null</c> if no such
		/// file exists.</returns>
		private PackagePart GetAmlEntryPart()
		{
			var relationship = _package?.GetRelationshipsByType(AmlxConstants.MainEntryRelationShipType).SingleOrDefault();
			if (relationship == null) return null;

			var part = _package.GetPart(relationship.TargetUri);
			if (part == null) throw new AmlxCorruptedDocumentException("Main entry part not found.");

			return part;
		}

		/// <summary>
		/// Save the CAEX content to the specified package part.
		/// </summary>
		/// <param name="part">The package part.</param>
		/// <param name="file">The CAEX file.</param>
		private void SaveCaexFile(PackagePart part, CAEXDocument file)
		{
			Debug.Assert(part != null);

			// write CAEX file
			using (var partStream = part.GetStream(FileMode.Create))
			using (var caexStream = file.SaveToStream(true))
			{
				caexStream.CopyTo(partStream);
			}

			// supplementary files
			FlushPackage();
			UpdateSupplementaryFileRelationShips(part);
		}

		/// <summary>
		/// Embed all files stored in the working directory to the package.
		/// </summary>
		/// <remarks>The working directory should not contain any files afterwards. Even though, empty directories
		/// may remain within the directory.</remarks>
		private void FlushPackage()
		{
			foreach (var file in _files.GetRemovedFiles())
			{
				var uri = PackUriHelper.CreatePartUri(file.Location);
				_package.DeletePart(uri);
			}

			foreach (var file in _files.OfType<IntermediateDocumentFile>())
			{
				file.Embed(_package);
			}
		}

		/// <summary>
		/// Updates the relationships to the supplementary files. The current AML root
		/// file will have a relation to each file located in the <see cref="DocumentFileCollection"/>.
		/// </summary>
		/// <param name="part">CAEX package part to associate the files to.</param>
		private void UpdateSupplementaryFileRelationShips(PackagePart part)
		{
			Debug.Assert(part != null);

			// all current relations
			var relations = part.GetRelationships().ToArray();

			// remove relations to removed files
			foreach (var relation in relations)
			{
				DocumentFileType type;
				if (!relation.TryParseDocumentFileType(out type)) continue;

				var uri = new Uri("." + relation.TargetUri, UriKind.Relative);
				var file = _files.FirstOrDefault(f => f.Location == uri);
				if (file == null) part.DeleteRelationship(relation.Id);
			}

			// add all missing relations
			foreach (var file in _files)
			{
				var type = file.Type.GetRelationshipType();
				var targetUri = PackUriHelper.CreatePartUri(file.Location);
				var relation = relations.FirstOrDefault(r => r.TargetUri == targetUri);
				if (relation != null && relation.RelationshipType != type)
				{
					part.DeleteRelationship(relation.Id);
					relation = null;
				}
				if (relation == null) part.CreateRelationship(targetUri, TargetMode.Internal, type);
			}
		}

		#endregion // Private Methods

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion // INotifyPropertyChanged
	}
}
