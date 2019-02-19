using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public abstract class CaexObjectViewModel
	{
		public CAEXBasicObject Model { get; }

		public ILocationResolver Resolver { get; }

		protected CaexObjectViewModel(ILocationResolver resolver)
		{ }

		protected CaexObjectViewModel(CAEXBasicObject model, ILocationResolver resolver)
		{
			Model = model;
			Resolver = resolver;
			ExternalInterfaceType x;
		}

		public abstract IEnumerable<CaexObjectViewModel> GetChildren();
	}
}