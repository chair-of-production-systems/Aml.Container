using Aml.Contracts;

namespace Aml.ViewModel
{
    public class DoublePropertyViewModel : BasePropertyViewModel<double>
    {
        public DoublePropertyViewModel(string name, double typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = typedValue;
        }

		public override double Value
		{
			get => double.Parse(_attribute.Value);
			set
			{
				_attribute.Value = value.ToString();
			}
		}
	}
}