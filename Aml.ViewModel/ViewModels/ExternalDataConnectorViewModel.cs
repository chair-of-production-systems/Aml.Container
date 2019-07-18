using System;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class ExternalDataConnectorViewModel : InterfaceViewModel
	{
		private readonly ExternalInterfaceType _interface;

		// TODO: move constant to a better place
		private const string RefUriName = "refURI";

		public Uri Location
		{
			get
			{
				var attribute = GetAttribute(RefUriName);
				return attribute == null ? null : new Uri(attribute.Value, UriKind.RelativeOrAbsolute);
			}
			set
			{
				var attribute = GetAttribute(RefUriName, true);

				// TODO: strings of RefAttributeType must be defined somewhere
				attribute.Name = RefUriName;
				attribute.AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(Uri));
				attribute.RefAttributeType = "AutomationMLBaseAttributeTypeLib/refURI";
				attribute.Value = value.ToString();
			}
		}

		public ViewModelCollection<BasePropertyViewModel> Properties { get; set; }

		public ExternalDataConnectorViewModel(IAmlProvider provider)
			: base(provider)
		{
			_interface = provider.CaexDocument.Create<ExternalInterfaceType>();
			Initialize();
		}

		public ExternalDataConnectorViewModel(ExternalInterfaceType model, IAmlProvider provider)
			: base(model, provider)
		{
			_interface = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _interface;
			Properties = new ViewModelCollection<BasePropertyViewModel>(_interface.Attribute, this);
		}

		protected AttributeType GetAttribute(string name, bool create = false)
		{
			var attribute = _interface.Attribute.GetCAEXAttribute(name);
			if (attribute == null && create)
			{
				attribute = _interface.New_Attribute(name);
			}
			return attribute;
		}
	}

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