using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public abstract class CaexObjectViewModel
	{
		public IAmlProvider Provider { get; }

		public ICAEXWrapper CaexObject { get; protected set; }

		protected CaexObjectViewModel(IAmlProvider provider)
		{
			Provider = provider;
		}

		public abstract IEnumerable<CaexObjectViewModel> GetDescendants();
	}
}