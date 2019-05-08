using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class PartViewModel : CaexObjectViewModel
	{
		private readonly InternalElementType _internalElement;

		public string Id
		{
			get => _internalElement.ID;
			set => _internalElement.ID = value;
		}

		public string Name
		{
			get => _internalElement.Name;
			set => _internalElement.Name = value;
		}

		public ViewModelCollection<ExternalDataConnectorViewModel> DataConnectors { get; private set; }

        public ViewModelCollection<BasePropertyViewModel> Properties { get; set; }

		public PartViewModel(IAmlProvider provider)
			: base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		public PartViewModel(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Part";
			CaexObject = _internalElement;
			DataConnectors = new ViewModelCollection<ExternalDataConnectorViewModel>(_internalElement.ExternalInterface, this);
		    Properties = new ViewModelCollection<BasePropertyViewModel>(_internalElement.Attribute, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}