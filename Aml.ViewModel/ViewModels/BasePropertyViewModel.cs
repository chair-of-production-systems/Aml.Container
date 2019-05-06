using Aml.Contracts;

namespace Aml.ViewModel
{
    public abstract class BasePropertyViewModel : CaexObjectViewModel
    {
        protected BasePropertyViewModel(IAmlProvider provider) : base(provider)
        {
        }

        public abstract string Name { get; set; }

        public abstract string Unit { get; set; }
    }
}