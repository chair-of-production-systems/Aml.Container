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
    }
}