using System;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.ViewModel.ViewModels;

namespace Aml.ViewModel
{
	public class ExternalDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(ExternalDataConnectorViewModel) });

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (!(model is ExternalInterfaceType iface)) return false;
			if (iface.RefBaseClassPath == null) return false;
			if (iface.RefBaseClassPath.Contains("ExternalDataConnector")) return true;
			return false;
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
			if (iface.RefBaseClassPath == null) return null;
			if (iface.RefBaseClassPath.Contains("ExternalDataConnector")) return typeof(ExternalDataConnectorViewModel);
			return null;
		}
	}

	public class GeometryDataConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public virtual Type[] Types => _types ?? (_types = new[] { typeof(GeometryDataConnectorViewModel) });

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (!(model is ExternalInterfaceType iface)) return false;
			if (iface.RefBaseClassPath == null) return false;

			var name = GeometryDataConnectorViewModel.ColladaClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return true;

			name = GeometryDataConnectorViewModel.GenericGeometryClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return true;

			return false;
		}

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

	public class ExternalGeometryConnectorViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public virtual Type[] Types => _types ?? (_types = new[] { typeof(ExternalGeometryConnectorViewModel) });

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (!(model is ExternalInterfaceType iface)) return false;
			if (iface.RefBaseClassPath == null) return false;

			var name = ExternalGeometryConnectorViewModel.ColladaClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return true;

			name = ExternalGeometryConnectorViewModel.GenericGeometryClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return true;

			return false;
		}

		public virtual T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (TypeOfViewModel(model) == null) return default(T);
			var connector = new ExternalGeometryConnectorViewModel((ExternalInterfaceType)model, provider) as T;
			return connector;
		}

		/// <inheritdoc />
		public virtual Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is ExternalInterfaceType iface)) return null;
			if (iface.RefBaseClassPath == null) return null;

			var name = ExternalGeometryConnectorViewModel.ColladaClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return typeof(ExternalGeometryConnectorViewModel);

			name = ExternalGeometryConnectorViewModel.GenericGeometryClassPath.Split('/').Last();
			if (iface.RefBaseClassPath.Contains(name)) return typeof(ExternalGeometryConnectorViewModel);

			return null;
		}
	}
}