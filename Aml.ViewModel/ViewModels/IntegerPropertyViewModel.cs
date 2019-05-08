using System.Collections.Generic;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.Services.Interfaces;

namespace Aml.ViewModel
{
    public class IntegerPropertyViewModel : BasePropertyViewModel<int>
    {
        public IntegerPropertyViewModel(string name, int typedValue, string unit, IAmlProvider provider) 
            : base(name, unit, provider)
        {
            Value = typedValue;
        }

		public override int Value
		{
			get => int.Parse(_attribute.Value);
			set
			{
				_attribute.Value = value.ToString();
			}
		}
	}
}