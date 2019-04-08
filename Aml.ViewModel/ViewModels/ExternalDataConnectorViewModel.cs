using System;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class ExternalDataConnectorViewModel : InterfaceViewModel
	{
		private ExternalInterfaceType _interface;

		// TODO: move constant to a better place
		private const string RefUriName = "refURI";

		public Uri Location
		{
			get
			{
				var attribute = _interface.Attribute.GetCAEXAttribute(RefUriName);
				if (attribute == null) return null;
				return new Uri(attribute.Value, UriKind.RelativeOrAbsolute);
			}
			set
			{
				var attribute = _interface.Attribute.GetCAEXAttribute(RefUriName);
				if (attribute == null)
				{
					attribute = _interface.New_Attribute(RefUriName);
				}

				// TODO: strings of AttributeDataType and RefAttributeType must be defined somewhere
				attribute.Name = RefUriName;
				attribute.AttributeDataType = "xs:anyURI";
				attribute.RefAttributeType = "AutomationMLBaseAttributeTypeLib/refURI";
				attribute.Value = value.ToString();
			}
		}

		public ExternalDataConnectorViewModel(IAmlProvider provider)
			: base(provider)
		{
			Initialize(provider.CaexDocument.Create<ExternalInterfaceType>());
		}

		public ExternalDataConnectorViewModel(ExternalInterfaceType model, IAmlProvider provider)
			: base(model, provider)
		{
			Initialize(model);
		}

		private void Initialize(ExternalInterfaceType model)
		{
			_interface = model;
			CaexObject = model;
		}
	}

	public class StepDataConnectorViewModel : ExternalDataConnectorViewModel
	{
		public StepDataConnectorViewModel(IAmlProvider provider) : base(provider)
		{ }

		public StepDataConnectorViewModel(ExternalInterfaceType model, IAmlProvider provider) : base(model, provider)
		{ }
	}
}