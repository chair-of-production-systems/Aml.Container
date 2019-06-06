using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public abstract class BaseComponent : CaexObjectViewModel
	{
		protected readonly InternalElementType _internalElement;

		public string Name
		{
			get => _internalElement.Name;
			set => _internalElement.Name = value;
		}

		public string Id
		{
			get => _internalElement.ID;
			set => _internalElement.ID = value;
		}

		protected BaseComponent(IAmlProvider provider) : base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		protected BaseComponent(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _internalElement;
		}
	}

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

	public class KinematicViewModel : CaexObjectViewModel
	{
		private readonly InternalElementType _internalElement;

		public ViewModelCollection<PartViewModel> Parts { get; private set; }

		public KinematicViewModel(IAmlProvider provider)
			: base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		public KinematicViewModel(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Assembly/Kinematic";
			CaexObject = _internalElement;
			Parts = new ViewModelCollection<PartViewModel>(_internalElement.InternalElement, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}