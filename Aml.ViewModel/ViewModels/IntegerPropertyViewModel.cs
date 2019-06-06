using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
    public class IntegerPropertyViewModel : BasePropertyViewModel<int>
    {
	    public sealed override int Value
	    {
		    get => int.Parse(_attribute.Value);
		    set => _attribute.Value = value.ToString();
	    }

	    public IntegerPropertyViewModel(IAmlProvider provider)
		    : base(provider)
	    { }

	    public IntegerPropertyViewModel(AttributeType model, IAmlProvider provider)
		    : base(model, provider)
	    { }

		[Obsolete]
	    public IntegerPropertyViewModel(string name, int typedValue, string unit, IAmlProvider provider) 
            : base(name, unit, provider)
        {
            Value = typedValue;
        }
	}
}