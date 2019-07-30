using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class Kinematic : BaseComponent
	{
		public ViewModelCollection<KinematicLink> Links { get; private set; }

		public ViewModelCollection<KinematicJoint> Joints { get; private set; }

		public ViewModelCollection<KinematicJointValue> JointValues { get; private set; }
		
		public Kinematic(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public Kinematic(InternalElementType model, IAmlProvider provider)
			: base(model, provider)
		{
			Initialize();
			//AddElements(model);
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Kinematic";
			Links = new ViewModelCollection<KinematicLink>(_internalElement.InternalElement, this);
			Joints = new ViewModelCollection<KinematicJoint>(_internalElement.InternalElement, this);
			JointValues = new ViewModelCollection<KinematicJointValue>(_internalElement.Attribute, this);
		}

		/// <inheritdoc />
		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}

		//private void AddElements(InternalElementType model)
		//{
		//	var factory = new KinematicFactory();
		//	foreach (var attribute in model.Attribute)
		//	{
		//		var jointValue = factory.Create<KinematicJointValue>(attribute, Provider);
		//		if (jointValue != null)
		//		{
		//			JointValues.Add(jointValue);
		//		}
		//	}

		//	foreach (var internalElement in model.InternalElement)
		//	{
		//		var joint = factory.Create<KinematicJoint>(internalElement, Provider);
		//		if (joint != null)
		//		{
		//			Joints.Add(joint);
		//		}

		//		var link = factory.Create<KinematicLink>(internalElement, Provider);
		//		if (link != null)
		//		{
		//			Links.Add(link);
		//		}
		//	}
		//}

		public override bool Equals(object other)
		{
			return Equals(other as Kinematic);
		}

		public bool Equals(Kinematic other)
		{
			if (other == null) return false;

			if (!(_internalElement?.RefBaseSystemUnitPath?.Equals(other._internalElement?.RefBaseSystemUnitPath) ??
			      false))
				return false;

			if (!(Name?.Equals(other.Name) ?? false))
				return false;

			if (!(Id?.Equals(other.Id) ?? false))
				return false;

			if (!(Links?.Equals(other.Links) ?? false))
				return false;

			if (!(JointValues?.Equals(other.JointValues) ?? false))
				return false;

			if (!(Joints?.Equals(other.Joints) ?? false))
				return false;

			return true;
		}
	}
}