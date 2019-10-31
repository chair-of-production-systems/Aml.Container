using Aml.Contracts;
using Aml.Engine.CAEX;
using System;
using System.Globalization;
using System.Linq;

namespace Aml.ViewModel
{
	public class CouplingPropertyViewModel : BasePropertyViewModel
	{
		internal const string AttributeName = "Coupling";
		private const double Epsilon = 1e-6;

		private BooleanPropertyViewModel _activeProperty;
		private StringPropertyViewModel _parentProperty;
		private DoublePropertyViewModel _rationProperty;

		public bool Active
		{
			get => GetProperty(ref _activeProperty, nameof(Active));
			set => SetProperty(ref _activeProperty, nameof(Active), value);
		}

		public string Parent
		{
			get => GetProperty(ref _parentProperty, nameof(Parent));
			set => SetProperty(ref _parentProperty, nameof(Parent), value);
		}

		public double Ratio
		{
			get => GetProperty(ref _rationProperty, nameof(Ratio));
			set => SetProperty(ref _rationProperty, nameof(Ratio), value);
		}

		public CouplingPropertyViewModel(IAmlProvider provider) 
			: base(provider)
		{
			Active = false;
			Parent = string.Empty;
			Ratio = 0d;

			Initialize();
		}

		public CouplingPropertyViewModel(AttributeType model, IAmlProvider provider)
			: base(model, provider)
		{
			Initialize();
		}

		internal void CopyValues(CouplingPropertyViewModel other)
		{
			Active = other.Active;
			Parent = other.Parent;
			Ratio = other.Ratio;
		}

		private void Initialize()
		{
			Name = AttributeName;

			foreach (var attribute in _attribute.Attribute)
			{
				switch (attribute.Name)
				{
					case nameof(Active):
						Active = bool.Parse(attribute.Value);
						break;
					case nameof(Parent):
						Parent = attribute.Value;
						break;
					case nameof(Ratio):
						Ratio = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
				}
			}
		}

		private bool GetProperty(ref BooleanPropertyViewModel property, string name)
		{
			FindPropertyInstance(ref property, name);
			return property?.Value ?? default(bool);
		}

		private double GetProperty(ref DoublePropertyViewModel property, string name)
		{
			FindPropertyInstance(ref property, name);
			return property?.Value ?? default(double);
		}

		private string GetProperty(ref StringPropertyViewModel property, string name)
		{
			FindPropertyInstance(ref property, name);
			return property?.Value ?? string.Empty;
		}

		private void FindPropertyInstance(ref BooleanPropertyViewModel property, string name)
		{
			if (property != null) return;

			var attribute = _attribute.Attribute.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if (attribute != null) property = new BooleanPropertyViewModel(attribute, Provider);
		}

		private void FindPropertyInstance(ref DoublePropertyViewModel property, string name)
		{
			if (property != null) return;

			var attribute = _attribute.Attribute.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if (attribute != null) property = new DoublePropertyViewModel(attribute, Provider);
		}

		private void FindPropertyInstance(ref StringPropertyViewModel property, string name)
		{
			if (property != null) return;

			var attribute = _attribute.Attribute.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if (attribute != null) property = new StringPropertyViewModel(attribute, Provider);
		}

		private void SetProperty(ref BooleanPropertyViewModel property, string name, bool value)
		{
			FindPropertyInstance(ref property, name);
			if (property == null)
			{
				property = new BooleanPropertyViewModel(Provider) { Name = name, Id = null };
				_attribute.Attribute.Insert(property.CaexObject as AttributeType);
			}
			property.Value = value;
		}

		private void SetProperty(ref DoublePropertyViewModel property, string name, double value)
		{
			FindPropertyInstance(ref property, name);
			if (property == null)
			{
				property = new DoublePropertyViewModel(Provider) { Name = name, Id = null };
				_attribute.Attribute.Insert(property.CaexObject as AttributeType);
			}
			property.Value = value;
		}

		private void SetProperty(ref StringPropertyViewModel property, string name, string value)
		{
			FindPropertyInstance(ref property, name);
			if (property == null)
			{
				property = new StringPropertyViewModel(Provider) { Name = name, Id = null };
				_attribute.Attribute.Insert(property.CaexObject as AttributeType);
			}
			property.Value = value;
		}

		public override bool Equals(object other)
		{
			return Equals(other as CouplingPropertyViewModel);
		}

		public bool Equals(CouplingPropertyViewModel other)
		{
			if (other == null) return false;

			return Math.Abs(Ratio - other.Ratio) < Epsilon
			       && Active == other.Active
			       && Parent.Equals(other.Parent);
		}
	}
}