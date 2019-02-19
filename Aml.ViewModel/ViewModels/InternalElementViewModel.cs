using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class InternalElementViewModel : CaexObjectViewModel
	{
		public ViewModelCollection<InterfaceViewModel> Interfaces { get; }

		public InternalElementViewModel(InternalElementType model, ILocationResolver resolver)
			: base(model, resolver)
		{
			Interfaces = new ViewModelCollection<InterfaceViewModel>(model.ExternalInterface, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetChildren()
		{
			foreach (var iface in Interfaces) yield return iface;
		}
	}
}