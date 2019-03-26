using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class InterfaceViewModel : CaexObjectViewModel
	{
		private readonly ExternalInterfaceType _interface;

		public InterfaceViewModel(IAmlProvider provider)
			: base(provider)
		{
			_interface = provider.CaexDocument.Create<ExternalInterfaceType>();
		}

		public InterfaceViewModel(ExternalInterfaceType model, IAmlProvider provider)
			: base(provider)
		{
			_interface = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _interface;
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}