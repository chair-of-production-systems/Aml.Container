using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class PartViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[] { typeof(PartViewModel) });

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (!(model is InternalElementType ie)) return false;
			if (ie.RefBaseSystemUnitPath.Contains("Part")) return true;
			return false;
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