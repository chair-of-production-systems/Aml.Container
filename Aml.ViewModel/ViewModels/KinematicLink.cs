using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class KinematicLink : AssemblyViewModel
	{
		public ViewModelCollection<Flange> Flanges { get; private set; }

		public KinematicLink(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public KinematicLink(InternalElementType model, IAmlProvider provider)
			: base(model, provider)
		{
			Initialize();
		}

		private void Initialize()
		{
			Flanges = new ViewModelCollection<Flange>(_internalElement.ExternalInterface, this);
		}
	}
}