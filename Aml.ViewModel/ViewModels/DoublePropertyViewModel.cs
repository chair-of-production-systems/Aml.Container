using Aml.Contracts;

namespace Aml.ViewModel
{
    public class DoublePropertyViewModel : PropertyViewModel<double>
    {
        private double _typedValue;

        public DoublePropertyViewModel(string name, double typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            TypedValue = typedValue;
            Update();
        }

        public override double TypedValue
        {
            get => _typedValue;
            set
            {
                _typedValue = value;
                Update();
            }
        }
    }
}