using Aml.Contracts;

namespace Aml.ViewModel
{
    public class DoublePropertyViewModel : PropertyViewModel<double>
    {
        private double _value;

        public DoublePropertyViewModel(string name, double typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = typedValue;
            Update();
        }

        public override double Value
        {
            get => _value;
            set
            {
                _value = value;
                Update();
            }
        }
    }
}