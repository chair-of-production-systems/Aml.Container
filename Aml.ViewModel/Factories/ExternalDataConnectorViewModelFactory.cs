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

	public class AssemblyViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(AssemblyViewModel) });

		/// <inheritdoc />
		public bool CanCreate(Type type) => typeof(AssemblyViewModel).IsSubclassOf(type);

		/// <inheritdoc />
		public CaexObjectViewModel Create(ICAEXWrapper model, IAmlProvider provider)
		{
			if (TypeOfViewModel(model) == null) return null;
			var assembly = new AssemblyViewModel((InternalElementType)model, provider);
			return assembly;

		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (TypeOfViewModel(model) == null) return null;
			var assembly = new AssemblyViewModel((InternalElementType)model, provider) as T;
			return assembly;
		}

		/// <inheritdoc />
		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is InternalElementType ie)) return null;
			if (ie.RefBaseSystemUnitPath.Contains("Assembly")) return typeof(AssemblyViewModel);
			return null;
		}
	}

	public class PartViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(PartViewModel) });

		/// <inheritdoc />
		public bool CanCreate(Type type) => typeof(PartViewModel).IsSubclassOf(type);

		/// <inheritdoc />
		public CaexObjectViewModel Create(ICAEXWrapper model, IAmlProvider provider)
		{
			if (TypeOfViewModel(model) == null) return null;
			var part = new PartViewModel((InternalElementType)model, provider);
			return part;

		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (TypeOfViewModel(model) == null) return null;
			var part = new PartViewModel((InternalElementType)model, provider) as T;
			return part;
		}

		/// <inheritdoc />
		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is InternalElementType ie)) return null;
			if (ie.RefBaseSystemUnitPath.Contains("Part")) return typeof(PartViewModel);
			return null;
		}
	}
}