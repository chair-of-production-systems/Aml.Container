using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class ProjectViewModel : CaexObjectViewModel
	{
		private readonly InstanceHierarchyType _instanceHierarchy;

		public ViewModelCollection<BaseComponent> Parts { get; private set; }

		public ProjectViewModel(IAmlProvider provider)
			: base(provider)
		{
			_instanceHierarchy = provider.CaexDocument.Create<InstanceHierarchyType>();
			Initialize();
		}

		public ProjectViewModel(InstanceHierarchyType model, IAmlProvider provider)
			: base(provider)
		{
			_instanceHierarchy = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _instanceHierarchy;
			Parts = new ViewModelCollection<BaseComponent>(_instanceHierarchy.InternalElement, this);
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}