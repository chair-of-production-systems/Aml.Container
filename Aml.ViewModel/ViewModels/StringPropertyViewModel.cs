using Aml.Contracts;

namespace Aml.ViewModel
{
    public class StringPropertyViewModel : PropertyViewModel<string>
    {
        private string _value;

        public StringPropertyViewModel(string name, string typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = typedValue;
            Update();
        }

        public override string Value
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