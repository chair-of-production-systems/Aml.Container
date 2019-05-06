using Aml.Contracts;

namespace Aml.ViewModel
{
    public class BooleanPropertyViewModel : PropertyViewModel<bool>
    {
        private bool _value;

        public BooleanPropertyViewModel(string name, bool typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = typedValue;
            Update();
        }

        public override bool Value
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