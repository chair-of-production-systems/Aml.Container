using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class Kinematic : BaseComponent
	{
		public ViewModelCollection<KinematicLink> Links { get; private set; }

		public ViewModelCollection<KinematicJoint> Joints { get; private set; }
		
		public Kinematic(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public Kinematic(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Assembly/Kinematic";
			Links = new ViewModelCollection<KinematicLink>(_internalElement.InternalElement, this);
			Joints = new ViewModelCollection<KinematicJoint>(_internalElement.InternalLink, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}