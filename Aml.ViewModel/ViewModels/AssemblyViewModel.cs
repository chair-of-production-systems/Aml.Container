using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class AssemblyViewModel : BaseComponent
	{
		public ViewModelCollection<BaseComponent> Parts { get; private set; }
		
		public AssemblyViewModel(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public AssemblyViewModel(InternalElementType model, IAmlProvider provider)
			: base(model, provider)
		{
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Assembly";
			Parts = new ViewModelCollection<BaseComponent>(_internalElement.InternalElement, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}