using System.Collections.Generic;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	/// <summary>
	/// A joint connects to links
	/// </summary>
	public class KinematicJoint : CaexObjectViewModel
	{
		private const string AxisValuePropertyName = "AxisValue";

		private readonly InternalElementType _internalElement;
		private ViewModelCollection<BasePropertyViewModel> _properties;
		private FrameProperty _frame;

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

		public KinematicLink Base { get; set; }

		public KinematicLink Axis { get; set; }

		public string AxisValue
		{
			get
			{
				var property = _properties.OfType<StringPropertyViewModel>()
					.FirstOrDefault(x => x.Name == AxisValuePropertyName);
				return property?.Value;
			}
			set
			{
				var property = _properties.OfType<StringPropertyViewModel>()
					.FirstOrDefault(x => x.Name == AxisValuePropertyName);
				if (property == null)
				{
					property = new StringPropertyViewModel(Provider) { Name = AxisValuePropertyName };
					_properties.Add(property);
				}
				property.Value = value;
			}
		}

		public FrameProperty Frame
		{
			get
			{
				if (_frame != null) return _frame;
				_frame = _properties.OfType<FrameProperty>().SingleOrDefault();
				if (_frame == null)
				{
					_frame = new FrameProperty(Provider);
					_properties.Add(_frame);
				}
				return _frame;
			}
		}

		public KinematicAxisType JointType { get; set; }

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