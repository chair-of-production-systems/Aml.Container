using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class KinematicJoint : AssemblyViewModel
	{
		public ViewModelCollection<Flange> Flanges { get; private set; }

		public KinematicJoint(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public KinematicJoint(InternalElementType model, IAmlProvider provider)
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