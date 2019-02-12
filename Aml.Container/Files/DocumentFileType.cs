using System.IO.Packaging;

namespace Aml.Container.Files
{
	/// <summary>
	/// Enumeration of different type of files.
	/// </summary>
	public enum DocumentFileType
	{
		/// <summary>
		/// Default (i.e. no specific file type).
		/// </summary>
		Default,

		/// <summary>
		/// File is a CAEX scheme file.
		/// </summary>
		CaexSchemeFile,

		/// <summary>
		/// File is a reference to an external AML library.
		/// </summary>
		ExternalReference,

		/// <summary>
		/// File is a COLLADA file.
		/// </summary>
		Collada,

		/// <summary>
		/// File is a PLCopen file.
		/// </summary>
		PlcOpen
	}

	public static class DocumentFileTypeExtensions
	{
		public static bool TryParseDocumentFileType(this PackageRelationship relationship, out DocumentFileType type)
		{
			switch (relationship.RelationshipType)
			{
				// obsolete
				case AmlxConstants.FileRelationShipType:
					type = DocumentFileType.Default;
					return true;

				case AmlxConstants.AnyFileRelationShipType:
					type = DocumentFileType.Default;
					return true;

				case AmlxConstants.CaexSchemeRelationShipType:
					type = DocumentFileType.CaexSchemeFile;
					return true;

				case AmlxConstants.ExternalReferenceRelationShipType:
					type = DocumentFileType.ExternalReference;
					return true;

				case AmlxConstants.ColladaRelationShipType:
					type = DocumentFileType.Collada;
					return true;

				case AmlxConstants.PlcOpenRelationShipType:
					type = DocumentFileType.PlcOpen;
					return true;
			}

			type = DocumentFileType.Default;
			return false;
		}

		public static string GetRelationshipType(this DocumentFileType type)
		{
			switch (type)
			{
				case DocumentFileType.CaexSchemeFile: return AmlxConstants.CaexSchemeRelationShipType;
				case DocumentFileType.ExternalReference: return AmlxConstants.ExternalReferenceRelationShipType;
				case DocumentFileType.Collada: return AmlxConstants.ColladaRelationShipType;
				case DocumentFileType.PlcOpen: return AmlxConstants.PlcOpenRelationShipType;
				default: return AmlxConstants.AnyFileRelationShipType;
			}
		}
	}
}