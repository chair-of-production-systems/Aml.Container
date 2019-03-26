using System.Collections.Generic;
using Aml.Contracts;

namespace Aml.ViewModel
{
	public class CaexDocumentViewModel : CaexObjectViewModel
	{
		public ViewModelCollection<InstanceHierarchyViewModel> InstanceHierarchies { get; private set; }

		#region Ctor & Dtor

		public CaexDocumentViewModel(IAmlProvider provider)
			: base(provider)
		{
			CaexObject = provider.CaexDocument.CAEXFile;
			InstanceHierarchies = new ViewModelCollection<InstanceHierarchyViewModel>(provider.CaexDocument.CAEXFile.InstanceHierarchy, this);
		}

		#endregion // Ctor & Dtor

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			foreach (var ih in InstanceHierarchies) yield return ih;
		}
	}
}