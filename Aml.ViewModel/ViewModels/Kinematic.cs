using System.Collections.Generic;
using System.ComponentModel.Design;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class Kinematic : BaseComponent
	{
		public ViewModelCollection<KinematicLink> Joints { get; private set; }

		public ViewModelCollection<KinematicJoint> Links { get; private set; }
		
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
			Joints = new ViewModelCollection<KinematicLink>(_internalElement.InternalElement, this);
			Links = new ViewModelCollection<KinematicJoint>(_internalElement.InternalLink, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}