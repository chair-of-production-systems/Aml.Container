using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class InterfaceViewModel : CaexObjectViewModel
	{
		public InterfaceViewModel(ExternalInterfaceType model, ILocationResolver resolver)
			: base(model, resolver)
		{ }

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetChildren()
		{
			yield break;
		}
	}
}