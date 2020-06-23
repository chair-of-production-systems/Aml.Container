using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	/// <summary>
	/// Defines an interface to a geometry location.
	/// </summary>
	public class GeometryDataConnectorViewModel : ExternalDataConnectorViewModel
	{
		internal const string ColladaClassPath = "AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/COLLADAInterface";
		internal const string GenericGeometryClassPath = "AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/GeometryDataConnector";

		private const string MimeTypeName = "mimeType";

		public string MimeType
		{
			get
			{
				var attribute = GetAttribute(MimeTypeName);
				return attribute == null ? null : attribute.Value;
			}
			set
			{
				var attribute = GetAttribute(MimeTypeName, true);
				attribute.Name = MimeTypeName;
				attribute.AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(string));
				attribute.Value = value;

				UpdateExternalInterface(value);
			} 
		}

		public GeometryDataConnectorViewModel(IAmlProvider provider) : base(provider)
		{
		}

		public GeometryDataConnectorViewModel(ExternalInterfaceType model, IAmlProvider provider) : base(model, provider)
		{ }

		private void UpdateExternalInterface(string mimeType)
		{
			if (mimeType == XMLMimeTypeMapper.GetMimeType(".dae"))
			{
				// COLLADA interface
				((ExternalInterfaceType) CaexObject).RefBaseClassPath = ColladaClassPath;
			}
			else
			{
				// Generic class path
				((ExternalInterfaceType)CaexObject).RefBaseClassPath = GenericGeometryClassPath;
			}
		}
	}
}