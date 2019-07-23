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

		public double Minimum
		{
			get
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				if (requirement == null) return 0d;
				return Convert.ToDouble(requirement.OrdinalScaledType.RequiredMinValue, CultureInfo.InvariantCulture);
			}
			set
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				requirement.OrdinalScaledType.RequiredMinValue = Convert.ToString(value, CultureInfo.InvariantCulture);
			}
		}

		public double Maximum
		{
			get
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				if (requirement == null) return 0d;
				return Convert.ToDouble(requirement.OrdinalScaledType.RequiredMaxValue, CultureInfo.InvariantCulture);
			}
			set
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				requirement.OrdinalScaledType.RequiredMaxValue = Convert.ToString(value, CultureInfo.InvariantCulture);
			}
		}

		public KinematicJointValue(IAmlProvider provider) : base(provider)
		{
			_attribute.RefAttributeType = AttributeRefTypeName;
			SetDefaultLimits();
		}

		public KinematicJointValue(AttributeType model, IAmlProvider provider) : base(model, provider)
		{
			_attribute.RefAttributeType = AttributeRefTypeName;
			SetDefaultLimits();
		}

		private void SetDefaultLimits()
		{
			var requirement = Provider.CaexDocument.Create<AttributeValueRequirementType>();
			requirement.New_OrdinalType();
			requirement.Name = LimitConstraintName;
			requirement.OrdinalScaledType.RequiredMaxValue = (DefaultValue ?? 0d).ToString();
			requirement.OrdinalScaledType.RequiredMinValue = (DefaultValue ?? 0d).ToString();
			_attribute.Constraint.Insert(requirement);
		}

		public override bool Equals(object other)
		{
			return Equals(other as KinematicJointValue);
		}

		public bool Equals(KinematicJointValue other)
		{
			if (other == null) return false;

			return (_attribute?.RefAttributeType?.Equals(other._attribute?.RefAttributeType) ?? false)
			       && (Name?.Equals(other.Name) ?? false)
			       && (Id?.Equals(other.Id) ?? false)
				   && Math.Abs(Value - other.Value) < Epsilon
			       && Math.Abs(DefaultValue ?? 0d - other.DefaultValue ?? 0d) < Epsilon
			       && Math.Abs(Minimum - other.Minimum) < Epsilon
			       && Math.Abs(Maximum - other.Maximum) < Epsilon;
		}
	}
}