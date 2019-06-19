using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public enum AxisType
	{
		Prismatic,
		Revolution
	}

	/// <summary>
	/// A joint connects to links
	/// </summary>
	public class KinematicJoint : CaexObjectViewModel
	{
		private readonly InternalLinkType _internalLink;

		public KinematicLink Base { get; set; }

		public KinematicLink Axis { get; set; }

		public FrameProperty Transformation { get; set; }

		public AxisType JointType { get; set; }

		public KinematicJoint(IAmlProvider provider) : base(provider)
		{
			_internalLink = provider.CaexDocument.Create<InternalLinkType>();
			Initialize();
		}

		protected KinematicJoint(InternalLinkType model, IAmlProvider provider)
			: base(provider)
		{
			_internalLink = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _internalLink;
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}