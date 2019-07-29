using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class KinematicFactory : ICaexViewModelFactory
	{
		public Type[] Types => new[]
		{
			typeof(Kinematic),
			typeof(KinematicJoint),
			typeof(KinematicLink),
			typeof(Flange)
		};

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (model is InternalElementType internalElement)
			{
				switch (internalElement.RefBaseSystemUnitPath)
				{
					case "/Kinematic":
						return typeof(T).IsAssignableFrom(typeof(Kinematic));
					case "/Kinematic/Joint":
						return typeof(T).IsAssignableFrom(typeof(KinematicJoint));
					case "/Kinematic/Link":
						return typeof(T).IsAssignableFrom(typeof(KinematicLink));
				}
			}

			if (model is ExternalInterfaceType externalInterface)
			{
				switch (externalInterface.RefBaseClassPath)
				{
					case "/Kinematic/Link/Flange":
						return typeof(T).IsAssignableFrom(typeof(Flange));
				}
			}
			return false;
		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			var t = TypeOfViewModel(model);
			if (t == null) return null;

			if (t == typeof(Kinematic)) return new Kinematic(model as InternalElementType, provider) as T;
			if (t == typeof(KinematicJoint)) return new KinematicJoint(model as InternalElementType, provider) as T;
			if (t == typeof(KinematicLink)) return new KinematicLink(model as InternalElementType, provider) as T;
			if (t == typeof(Flange)) return new Flange(model as ExternalInterfaceType, provider) as T;
			return null;
		}

		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (model is InternalElementType internalElement)
			{
				switch (internalElement.RefBaseSystemUnitPath)
				{
					case "/Kinematic":
						return typeof(Kinematic);
					case "/Kinematic/Joint":
						return typeof(KinematicJoint);
					case "/Kinematic/Link":
						return typeof(KinematicLink);
				}
			}

			if (model is ExternalInterfaceType externalInterface)
			{
				switch (externalInterface.RefBaseClassPath)
				{
					case "/Kinematic/Link/Flange":
						return typeof(Flange);
				}
			}
			return null;
		}
	}
}