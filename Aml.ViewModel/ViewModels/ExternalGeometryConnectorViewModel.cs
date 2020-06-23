using Aml.Contracts;
using Aml.Engine.CAEX;
using System;

namespace Aml.ViewModel.ViewModels
{
	public class ExternalGeometryConnectorViewModel : ExternalDataConnectorViewModel
	{
		internal const string ColladaClassPath = "AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/COLLADAInterfaceUrl";
		internal const string GenericGeometryClassPath = "AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/ExternalGeometryConnector";

		private const string FileNameName = "fileName";
		private const string FileIdName = "fileId";
		private const string MimeTypeName = "mimeType";

		public ExternalGeometryConnectorViewModel(IAmlProvider provider) : base(provider)
		{
		}

		public ExternalGeometryConnectorViewModel(ExternalInterfaceType model, IAmlProvider provider) : base(model, provider)
		{
		}

		public string FileId
		{
			get
			{
				var attribute = GetAttribute(FileIdName);
				return attribute == null ? null : attribute.Value;
			}
			set
			{
				var attribute = GetAttribute(FileIdName, true);
				attribute.Name = FileIdName;
				attribute.AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(string));
				attribute.Value = value;

				UpdateExternalInterface(value);
			}
		}

		public string FileName
		{
			get
			{
				var attribute = GetAttribute(FileNameName);
				return attribute == null ? null : attribute.Value;
			}
			set
			{
				var attribute = GetAttribute(FileNameName, true);
				attribute.Name = FileNameName;
				attribute.AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(string));
				attribute.Value = value;

				UpdateExternalInterface(value);
			}
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
