using Aml.Contracts;

namespace Aml.ViewModel
{
    public class BooleanPropertyViewModel : PropertyViewModel<bool>
    {
        private bool _typedValue;

        public BooleanPropertyViewModel(string name, bool typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            TypedValue = typedValue;
            Update();
        }

        public override bool TypedValue
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