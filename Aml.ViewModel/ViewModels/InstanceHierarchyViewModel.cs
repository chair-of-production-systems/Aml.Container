using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class InstanceHierarchyViewModel : CaexObjectViewModel
	{
		private readonly InstanceHierarchyType _instanceHierarchy;

		public ViewModelCollection<InternalElementViewModel> InternalElements { get; private set; }

		public InstanceHierarchyViewModel(IAmlProvider provider)
			: base(provider)
		{
			_instanceHierarchy = provider.CaexDocument.Create<InstanceHierarchyType>();
			Initialize();
		}

		public InstanceHierarchyViewModel(InstanceHierarchyType model, IAmlProvider provider)
			: base(provider)
		{
			_instanceHierarchy = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _instanceHierarchy;
			InternalElements = new ViewModelCollection<InternalElementViewModel>(_instanceHierarchy.InternalElement, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			foreach (var ie in InternalElements) yield return ie;
		}
	}
}