using System.Collections.Generic;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.Services.Interfaces;

namespace Aml.ViewModel
{
    public class IntegerPropertyViewModel : PropertyViewModel<int>
    {
        private int _typedValue;

        public IntegerPropertyViewModel(string name, int typedValue, string unit, IAmlProvider provider) 
            : base(name, unit, provider)
        {
            TypedValue = typedValue;
            Update();
        }

        public override int TypedValue
        {
            get => _typedValue;
            set
            {
                _typedValue = value;
                Update();
            }
        }

        public override IEnumerable<CaexObjectViewModel> GetDescendants()
        {
            yield break;
        }

        private void Update()
        {
            var typedValueAttribute = _internalElement.Attribute.FirstOrDefault(x => x.Name.Equals(nameof(TypedValue)));
            if (typedValueAttribute == null)
            {
                _internalElement.Attribute.Append(new NameObjectTuple(nameof(TypedValue), TypedValue));
            }
            else
            {
                typedValueAttribute.Value = TypedValue.ToString();
            }
        }
    }
}