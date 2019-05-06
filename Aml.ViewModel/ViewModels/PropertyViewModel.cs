using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
    public abstract class PropertyViewModel<T> : BasePropertyViewModel
    {
        protected readonly InternalElementType _internalElement;

        protected PropertyViewModel(string name, string unit, IAmlProvider provider) : base(provider)
        {
            Name = name;
            Unit = unit;
            _internalElement = provider.CaexDocument.Create<InternalElementType>();
            _internalElement.Attribute.Append(new NameObjectTuple("Name", Name));
            _internalElement.Attribute.Append(new NameObjectTuple("Unit", Unit));
        }

        public override string Name { get; set; }

        public abstract T TypedValue { get; set; }

        public override object Value => TypedValue;

        public override string Unit { get; set; }
    }
}