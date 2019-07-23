using System;
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
		private const string AxisPropertyName = "Axis";
		private const string AxisValuePropertyName = "AxisValue";
		private const string BasePropertyName = "Base";
		private const string FrameAttributeName = "Frame";
		private const string JointTypeAttributeName = "JointType";

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

		public string Base
		{
			get
			{
				var property = _properties.OfType<StringPropertyViewModel>()
					.FirstOrDefault(x => x.Name == BasePropertyName);
				return property?.Value;
			}
			set
			{
				var property = _properties.OfType<StringPropertyViewModel>()
					.FirstOrDefault(x => x.Name == BasePropertyName);
				if (property == null)
				{
					property = new StringPropertyViewModel(Provider) { Name = BasePropertyName };
					_properties.Add(property);
				}
				property.Value = value;
			}
		}

		public string Axis
		{
			get
			{
				var property = _properties.OfType<StringPropertyViewModel>()
					.FirstOrDefault(x => x.Name == AxisPropertyName);
				return property?.Value;
			}
			set
			{
				var property = _properties.OfType<StringPropertyViewModel>()
					.FirstOrDefault(x => x.Name == AxisPropertyName);
				if (property == null)
				{
					property = new StringPropertyViewModel(Provider) { Name = AxisPropertyName };
					_properties.Add(property);
				}
				property.Value = value;
			}
		}

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

		public KinematicAxisType JointType
		{
			get
			{
				var property = _properties.OfType<StringPropertyViewModel>().FirstOrDefault(x => x.Name == JointTypeAttributeName);
				if (property == null) return KinematicAxisType.Revolution;
				if (!Enum.TryParse(property.Value, true, out KinematicAxisType value)) return KinematicAxisType.Revolution;
				return value;
			}
			set
			{
				var property = _properties.OfType<StringPropertyViewModel>().FirstOrDefault(x => x.Name == JointTypeAttributeName);
				if (property == null)
				{
					property = new StringPropertyViewModel(Provider) { Name = JointTypeAttributeName };
					_properties.Add(property);
				}
				property.Value = value.ToString();
			}
		}

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

		public KinematicJoint(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
			// AddElements(model);
		}

		private void Initialize()
		{
			_internalElement.RefBaseSystemUnitPath = "/Kinematic/Joint";
			CaexObject = _internalElement;
			_properties = new ViewModelCollection<BasePropertyViewModel>(_internalElement.Attribute, this);
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}

		public override bool Equals(object other)
		{
			return Equals(other as KinematicJoint);
		}

		public bool Equals(KinematicJoint other)
		{
			if (other == null) return false;

			return (_internalElement?.RefBaseSystemUnitPath?.Equals(other._internalElement?.RefBaseSystemUnitPath) ??
			        false)
			       && (Name?.Equals(other.Name) ?? false)
			       && (Id?.Equals(other.Id) ?? false)
			       && (Base?.Equals(other.Base) ?? false)
			       && (Axis?.Equals(other.Axis) ?? false)
			       && (AxisValue?.Equals(other.AxisValue) ?? false)
			       && (Frame?.Equals(other.Frame) ?? false)
			       && JointType == other.JointType;
		}
	}
}