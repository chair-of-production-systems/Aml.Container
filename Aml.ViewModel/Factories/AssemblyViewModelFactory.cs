using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
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
}