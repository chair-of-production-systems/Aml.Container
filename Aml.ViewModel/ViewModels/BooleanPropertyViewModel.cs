using Aml.Contracts;
using Aml.Engine.Services.Interfaces;

namespace Aml.ViewModel
{
    public class BooleanPropertyViewModel : BasePropertyViewModel<bool>
    {
        public BooleanPropertyViewModel(string name, bool value, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = value;
        }

        public override bool Value
        {
            get => bool.Parse(_attribute.Value);
            set
            {
                _attribute.Value = value.ToString();
            }
        }
    }
}