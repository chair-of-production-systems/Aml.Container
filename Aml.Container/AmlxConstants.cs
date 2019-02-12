namespace Aml.Container
{
	public static class AmlxConstants
	{
		/// <summary>
		/// Content type of the main AML file to be used as entry point of the container.
		/// </summary>
		public const string MainEntryContentType = "model/vnd.automationml+xml";

		///// <summary>
		///// Content type of parts defining relationships.
		///// </summary>
		///// <remarks>Those files are auxiliary files, which become automatically created.</remarks>
		//public const string RelationshipContentType = "application/vnd.openxmlformats-package.relationships+xml";

		/// <summary>
		/// This relationship type should be used to identify the main AML file to be used as entry point of the container.
		/// </summary>
		/// <remarks>There should be exactly one relationship of this type related to the container.</remarks>
		public const string MainEntryRelationShipType = "http://schemas.automationml.org/container/relationship/rootdocument";

		/// <summary>
		/// This global relationship type may be used to mark a file as an incremental version file of a main entry file.
		/// </summary>
		public const string MainEntryIncrementalVersionRelationShipType = "http://schemas.conexing.de/package/2015/relationships/version/incremental";

		/// <summary>
		/// This relationship can be used to mark a file as previous version of another file.
		/// </summary>
		public const string PreviousVersionRelationShipType = "http://schemas.conexing.de/package/2015/relationships/version/previous";

		/// <summary>
		/// Relation to mark a file as a supplementary file of another file. Usually, an AML file refers additional files. All those
		/// files should become related to the AML file using this relationship type.
		/// </summary>
		/// replaced by AnyFileRelationShip type
		public const string FileRelationShipType = "http://schemas.conexing.de/package/2014/relationships/amlx/supplementaryFile";

		public const string ExternalReferenceRelationShipType = "http://schemas.automationml.org/container/relationship/library";
		public const string ExternalReferenceContentType = "model/vnd.automationml+xml";

		public const string ColladaRelationShipType = "http://schemas.automationml.org/container/relationship/collada";
		public const string ColladaContentType = "model/vnd.collada+xml";

		public const string PlcOpenRelationShipType = "http://schemas.automationml.org/container/relationship/plcopenxml";
		public const string PlcOpenContentType = "model/vnd.plcopen+xml";

		public const string AnyFileRelationShipType = "http://schemas.automationml.org/container/relationship/anycontent";
		public const string AnyFileContentType = "application/x-any";

		public const string RepositoryRelationShipType = "http://schemas.engine.amlx/repository";

		/// <summary>
		/// Relationship type for the CAEX scheme file.
		/// </summary>
		public const string CaexSchemeRelationShipType = "http://schemas.conexing.de/package/2014/relationships/aml/scheme";

		/// <summary>
		/// Relationship type for external references.
		/// </summary>
		public const string FormerExternalReferenceRelationShipType = "http://schemas.conexing.de/package/2014/relationships/aml/externalreference";

		/// <summary>
		/// The file extension for AML files
		/// </summary>
		public const string FileExtensionAml = ".aml";

		/// <summary>
		/// The file extension for AMLX files
		/// </summary>
		public const string FileExtensionAmlx = ".amlx";

		/// <summary>
		/// The AutomationML standard library alias.
		/// </summary>
		public const string AutomationMLStandardLibraryAlias = "AutomationMLStandardLibrary";

		internal const string GuidFormatFileOrDirectory = "D";
	}
}