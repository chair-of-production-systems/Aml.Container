using System;
using System.Globalization;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
    public class DoublePropertyViewModel : BasePropertyViewModel<double>
    {
	    public sealed override double Value
	    {
		    get
		    {
			    var value = _attribute.Value;
			    return value == null ? default(double) : double.Parse(_attribute.Value, CultureInfo.InvariantCulture);
		    }
		    set => _attribute.Value = Convert.ToString(value, CultureInfo.InvariantCulture);
	    }

	    public DoublePropertyViewModel(IAmlProvider provider)
			: base(provider)
	    { }

	    public DoublePropertyViewModel(AttributeType model, IAmlProvider provider)
			: base(model, provider)
	    { }

		[Obsolete]
        public DoublePropertyViewModel(string name, double typedValue, string unit, IAmlProvider provider)
            : base(name, unit, provider)
        {
            Value = typedValue;
        }
	}
}