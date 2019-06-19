using System.Collections.Generic;
using System.Linq;
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
		private readonly InternalElementType _internalElement;
		private ViewModelCollection<BasePropertyViewModel> _properties;
		private FrameProperty _frame;

		public KinematicLink Base { get; set; }

		public KinematicLink Axis { get; set; }

		public FrameProperty Frame
		{
			get
			{
				if (_frame != null) return _frame;
				_frame = _properties.OfType<FrameProperty>().SingleOrDefault();
				if (_frame == null)
				{
					_frame = new FrameProperty(Provider);
					_internalElement.Attribute.Insert(_frame.CaexObject as AttributeType);
				}
				return _frame;
			}
		}

		public AxisType JointType { get; set; }

		public KinematicJoint(IAmlProvider provider)
			: base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		public KinematicJoint(IAmlProvider provider, double theta, double d, double a, double alpha)
			: this(provider)
		{
			Frame.SetDhParameters(theta, d, a, alpha);
		}

		protected KinematicJoint(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _internalElement;
			_properties = new ViewModelCollection<BasePropertyViewModel>(_internalElement.Attribute, this);
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}