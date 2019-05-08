using Aml.Contracts;

namespace Aml.ViewModel
{
	public class StringPropertyViewModel : BasePropertyViewModel<string>
	{
		public StringPropertyViewModel(string name, string typedValue, string unit, IAmlProvider provider)
			: base(name, unit, provider)
		{
			Value = typedValue;
		}

		public override string Value
		{
			get => _attribute.Value;
			set
			{
				_attribute.Value = value;
			}
		}
	}
}