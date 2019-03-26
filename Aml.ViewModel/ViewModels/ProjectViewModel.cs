using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class ProjectViewModel : CaexObjectViewModel
	{
		private readonly InstanceHierarchyType _instanceHierarchy;

		public ViewModelCollection<AssemblyViewModel> Assemblies { get; private set; }

		public ViewModelCollection<PartViewModel> Parts { get; private set; }

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
			Assemblies = new ViewModelCollection<AssemblyViewModel>(_instanceHierarchy.InternalElement, this);
			Parts = new ViewModelCollection<PartViewModel>(_instanceHierarchy.InternalElement, this);
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}

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

	public class PartViewModel : CaexObjectViewModel
	{
		private readonly InternalElementType _internalElement;

		public string Id
		{
			get => _internalElement.ID;
			set => _internalElement.ID = value;
		}

		public string Name
		{
			get => _internalElement.Name;
			set => _internalElement.Name = value;
		}

		public PartViewModel(IAmlProvider provider)
			: base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		public PartViewModel(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Part";
			CaexObject = _internalElement;
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}