using System;
using System.Globalization;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class KinematicJointValue : DoublePropertyViewModel
	{
		#region Consts

		internal const string AttributeRefTypeName = "/Kinematik/JointValue";
		private const string LimitConstraintName = "JointLimits";
		private const double Epsilon = 1e-6;

		#endregion // Consts

		private AttributeValueRequirementType _requirement;

		public double Minimum
		{
			get
			{
				if (_requirement == null) return 0d;
				return Convert.ToDouble(_requirement.OrdinalScaledType.RequiredMinValue, CultureInfo.InvariantCulture);
			}
			set
			{
				EnsureElements();
				_requirement.OrdinalScaledType.RequiredMinValue = Convert.ToString(value, CultureInfo.InvariantCulture);
			}
		}

		public double Maximum
		{
			get
			{
				if (_requirement == null) return 0d;
				return Convert.ToDouble(_requirement.OrdinalScaledType.RequiredMaxValue, CultureInfo.InvariantCulture);
			}
			set
			{
				EnsureElements();
				_requirement.OrdinalScaledType.RequiredMaxValue = Convert.ToString(value, CultureInfo.InvariantCulture);
			}
		}

		public KinematicJointValue(IAmlProvider provider) : base(provider)
		{
			_attribute.RefAttributeType = AttributeRefTypeName;
		}

		public KinematicJointValue(AttributeType model, IAmlProvider provider) : base(model, provider)
		{
			_attribute.RefAttributeType = AttributeRefTypeName;
			EnsureElements();
		}

		private void EnsureElements()
		{
			if (_requirement != null) return;

			_requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
			if (_requirement == null)
			{
				_requirement = Provider.CaexDocument.Create<AttributeValueRequirementType>();
				_requirement.New_OrdinalType();
				_requirement.Name = LimitConstraintName;
				_requirement.OrdinalScaledType.RequiredMaxValue = (DefaultValue ?? 0d).ToString(CultureInfo.InvariantCulture);
				_requirement.OrdinalScaledType.RequiredMinValue = (DefaultValue ?? 0d).ToString(CultureInfo.InvariantCulture);
				_attribute.Constraint.Insert(_requirement);
			}
		}

		public override bool Equals(object other)
		{
			return Equals(other as KinematicJointValue);
		}

		public bool Equals(KinematicJointValue other)
		{
			if (other == null) return false;

			if (!(_attribute?.RefAttributeType?.Equals(other._attribute?.RefAttributeType) ?? false))
				return false;

			if (!(Name?.Equals(other.Name) ?? false))
				return false;

			if (!(Id?.Equals(other.Id) ?? false))
				return false;

			if (!(Math.Abs(Value - other.Value) < Epsilon))
				return false;

			if (!(Math.Abs((DefaultValue ?? 0d) - (other.DefaultValue ?? 0d)) < Epsilon))
				return false;

			if (!(Math.Abs(Minimum - other.Minimum) < Epsilon))
				return false;

			if (!(Math.Abs(Maximum - other.Maximum) < Epsilon))
				return false;

			return true;
		}
	}
}