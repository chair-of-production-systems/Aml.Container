using Aml.Contracts;
using Aml.Engine.CAEX;
using System;

namespace Aml.ViewModel.ViewModels
{
	public class ExternalGeometryConnectorViewModel : ExternalDataConnectorViewModel
	{
		internal const string ColladaClassPath = "AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/COLLADAInterfaceUrl";
		internal const string GenericGeometryClassPath = "AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/ExternalGeometryConnector";

		private const string MimeTypeName = "mimeType";

		public ExternalGeometryConnectorViewModel(IAmlProvider provider, string url) : base(provider)
		{
			Location = new Uri(url, UriKind.RelativeOrAbsolute);
		}

		public ExternalGeometryConnectorViewModel(ExternalInterfaceType model, IAmlProvider provider, string url) : base(model, provider)
		{
			Location = new Uri(url, UriKind.RelativeOrAbsolute);
		}

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

		private void UpdateExternalInterface(string mimeType)
		{
			if (mimeType == XMLMimeTypeMapper.GetMimeType(".dae"))
			{
				// COLLADA interface
				((ExternalInterfaceType)CaexObject).RefBaseClassPath = ColladaClassPath;
			}
			else
			{
				// Generic class path
				((ExternalInterfaceType)CaexObject).RefBaseClassPath = GenericGeometryClassPath;
			}
		}
	}
}
