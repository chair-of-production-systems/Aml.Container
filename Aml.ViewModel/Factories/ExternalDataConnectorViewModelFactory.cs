using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class ExternalDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		/// <inheritdoc />
		public bool CanCreate(Type type) => typeof(ExternalDataConnectorViewModel).IsSubclassOf(type);

		/// <inheritdoc />
		public CaexObjectViewModel Create(CAEXBasicObject model, ILocationResolver resolver)
		{
			if (TypeOfViewModel(model) == null) return null;
			var connector = new ExternalDataConnectorViewModel((ExternalInterfaceType)model, resolver);
			return connector;
		}

		/// <inheritdoc />
		public Type TypeOfViewModel(CAEXBasicObject model)
		{
			if (!(model is ExternalInterfaceType iface)) return null;
			if (iface.RefBaseClassPath.Contains("ExternalDataConnector")) return typeof(ExternalDataConnectorViewModel);
			return null;
		}
	}
}