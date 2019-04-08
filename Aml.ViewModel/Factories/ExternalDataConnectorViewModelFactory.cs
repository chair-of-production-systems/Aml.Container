using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class ExternalDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(ExternalDataConnectorViewModel) });

		/// <inheritdoc />
		public bool CanCreate(Type type) => typeof(ExternalDataConnectorViewModel).IsSubclassOf(type);

		/// <inheritdoc />
		public CaexObjectViewModel Create(ICAEXWrapper model, IAmlProvider provider)
		{
			if (TypeOfViewModel(model) == null) return null;
			var connector = new ExternalDataConnectorViewModel((ExternalInterfaceType)model, provider);
			return connector;
		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (TypeOfViewModel(model) == null) return default(T);
			var connector = new ExternalDataConnectorViewModel((ExternalInterfaceType)model, provider) as T;
			return connector;
		}

		/// <inheritdoc />
		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is ExternalInterfaceType iface)) return null;
			if (iface.RefBaseClassPath.Contains("ExternalDataConnector")) return typeof(ExternalDataConnectorViewModel);
			return null;
		}
	}

	public class StepDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(StepDataConnectorViewModel) });

		/// <inheritdoc />
		public bool CanCreate(Type type) => typeof(StepDataConnectorViewModel).IsSubclassOf(type);

		/// <inheritdoc />
		public CaexObjectViewModel Create(ICAEXWrapper model, IAmlProvider provider)
		{
			if (TypeOfViewModel(model) == null) return null;
			var connector = new StepDataConnectorViewModel((ExternalInterfaceType)model, provider);
			return connector;
		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (TypeOfViewModel(model) == null) return default(T);
			var connector = new StepDataConnectorViewModel((ExternalInterfaceType)model, provider) as T;
			return connector;
		}

		/// <inheritdoc />
		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is ExternalInterfaceType iface)) return null;
			if (iface.RefBaseClassPath.Contains("StepInterface")) return typeof(ExternalDataConnectorViewModel);
			return null;
		}
	}
}