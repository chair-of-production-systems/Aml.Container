using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class PartViewModel : BaseComponent
	{
		public ViewModelCollection<ExternalDataConnectorViewModel> DataConnectors { get; private set; }

        public ViewModelCollection<BasePropertyViewModel> Properties { get; set; }

		public PartViewModel(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public PartViewModel(InternalElementType model, IAmlProvider provider)
			: base(model, provider)
		{
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Part";
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