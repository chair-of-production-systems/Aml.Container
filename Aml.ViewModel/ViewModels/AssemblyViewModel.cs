using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class AssemblyViewModel : CaexObjectViewModel
	{
		private readonly InternalElementType _internalElement;

		public ViewModelCollection<PartViewModel> Parts { get; private set; }

		public AssemblyViewModel(IAmlProvider provider)
			: base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		public AssemblyViewModel(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Assembly";
			CaexObject = _internalElement;
			Parts = new ViewModelCollection<PartViewModel>(_internalElement.InternalElement, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}