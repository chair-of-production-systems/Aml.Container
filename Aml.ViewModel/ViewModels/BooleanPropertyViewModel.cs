using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
    public class BooleanPropertyViewModel : BasePropertyViewModel<bool>
    {
	    public sealed override bool Value
	    {
		    get => bool.Parse(_attribute.Value);
		    set => _attribute.Value = value.ToString();
	    }

		public BooleanPropertyViewModel(IAmlProvider provider)
		    : base(provider)
	    { }

	    public BooleanPropertyViewModel(AttributeType model, IAmlProvider provider)
		    : base(model, provider)
	    { }

		[Obsolete]
		public BooleanPropertyViewModel(string name, bool value, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = value;
        }
    }
}