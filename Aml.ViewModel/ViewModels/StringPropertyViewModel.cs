using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class StringPropertyViewModel : BasePropertyViewModel<string>
	{
		public sealed override string Value
		{
			get => _attribute.Value;
			set => _attribute.Value = value;
		}

		public StringPropertyViewModel(IAmlProvider provider)
			: base(provider)
		{ }

		public StringPropertyViewModel(AttributeType model, IAmlProvider provider)
			: base(model, provider)
		{ }

		[Obsolete]
		public StringPropertyViewModel(string name, string typedValue, string unit, IAmlProvider provider)
			: base(name, unit, provider)
		{
			Value = typedValue;
		}
	}
}