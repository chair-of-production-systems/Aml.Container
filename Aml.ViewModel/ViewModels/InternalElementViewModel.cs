using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class InternalElementViewModel : CaexObjectViewModel
	{
		private readonly InternalElementType _internalElement;

		public ViewModelCollection<InterfaceViewModel> Interfaces { get; private set; }

		public InternalElementViewModel(IAmlProvider provider)
			: base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		public InternalElementViewModel(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _internalElement;
			Interfaces = new ViewModelCollection<InterfaceViewModel>(_internalElement.ExternalInterface, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			foreach (var iface in Interfaces) yield return iface;
		}
	}
}