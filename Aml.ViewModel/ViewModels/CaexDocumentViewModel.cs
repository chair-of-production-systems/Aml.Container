using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class CaexDocumentViewModel : CaexObjectViewModel
	{
		public ViewModelCollection<InstanceHierarchyViewModel> InstanceHierarchies { get; }

		public CaexDocumentViewModel(CAEXDocument model, ILocationResolver resolver)
			: base(model.CAEXFile, resolver)
		{
			InstanceHierarchies = new ViewModelCollection<InstanceHierarchyViewModel>(model.CAEXFile.InstanceHierarchy, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetChildren()
		{
			foreach (var ih in InstanceHierarchies) yield return ih;
		}
	}
}