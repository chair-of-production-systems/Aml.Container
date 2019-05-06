using Aml.Contracts;

namespace Aml.ViewModel
{
    public class StringPropertyViewModel : PropertyViewModel<string>
    {
        private string _typedValue;

        public StringPropertyViewModel(string name, string typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            TypedValue = typedValue;
            Update();
        }

        public override string TypedValue
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