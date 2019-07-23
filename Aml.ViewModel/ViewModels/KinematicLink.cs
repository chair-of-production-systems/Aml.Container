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
			_internalElement.RefBaseSystemUnitPath = "/Kinematic/Link";
			Flanges = new ViewModelCollection<Flange>(_internalElement.ExternalInterface, this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as KinematicLink);
		}

		public bool Equals(KinematicLink other)
		{
			if (other == null) return false;

			return (_internalElement?.RefBaseSystemUnitPath?.Equals(other._internalElement?.RefBaseSystemUnitPath) ??
			        false)
			       && (Name?.Equals(other.Name) ?? false)
			       && (Id?.Equals(other.Id) ?? false)
			       && (Flanges?.Equals(other.Flanges) ?? false);
		}
	}
}