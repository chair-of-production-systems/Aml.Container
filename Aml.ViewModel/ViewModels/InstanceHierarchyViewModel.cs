using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class InstanceHierarchyViewModel : CaexObjectViewModel
	{
		public ViewModelCollection<InternalElementViewModel> InternalElements { get; }

		public InstanceHierarchyViewModel(InstanceHierarchyType model, ILocationResolver resolver)
			: base(model, resolver)
		{
			InternalElements = new ViewModelCollection<InternalElementViewModel>(model.InternalElement, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetChildren()
		{
			foreach (var ie in InternalElements) yield return ie;
		}
	}
}