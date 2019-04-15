using System;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class ExternalDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(ExternalDataConnectorViewModel) });

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
			if (iface.RefBaseClassPath == null) return null;
			if (iface.RefBaseClassPath.Contains("ExternalDataConnector")) return typeof(ExternalDataConnectorViewModel);
			return null;
		}
	}

	public class GeometryDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public virtual Type[] Types => _types ?? (_types = new[] { typeof(GeometryDataConnectorViewModel) });

		public virtual T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (TypeOfViewModel(model) == null) return default(T);
			var connector = new GeometryDataConnectorViewModel((ExternalInterfaceType)model, provider) as T;
			return connector;
		}

		/// <inheritdoc />
		public virtual Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is ExternalInterfaceType iface)) return null;
			if (iface.RefBaseClassPath == null) return null;

			var name = GeometryDataConnectorViewModel.ColladaClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return typeof(GeometryDataConnectorViewModel);

			name = GeometryDataConnectorViewModel.GenericGeometryClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return typeof(GeometryDataConnectorViewModel);

			return null;
		}
	}
}