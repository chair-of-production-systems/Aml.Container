using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class CaexViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[]
		{
			typeof(InstanceHierarchyViewModel),
			typeof(InternalElementViewModel),
			typeof(InterfaceViewModel)
		});

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (model == null) return false;

			return (model.GetType() == typeof(InstanceHierarchyViewModel) &&
						typeof(T).IsAssignableFrom(typeof(InstanceHierarchyViewModel)))
			       || (model.GetType() == typeof(InternalElementViewModel) &&
			           typeof(T).IsAssignableFrom(typeof(InternalElementViewModel)))
				   || (model.GetType() == typeof(InterfaceViewModel) &&
				       typeof(T).IsAssignableFrom(typeof(InterfaceViewModel)));
		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (model.GetType() == typeof(InstanceHierarchyType)) return new InstanceHierarchyViewModel((InstanceHierarchyType)model, provider) as T;
			if (model.GetType() == typeof(InternalElementType)) return new InternalElementViewModel((InternalElementType)model, provider) as T;
			if (model.GetType() == typeof(ExternalInterfaceType)) return new InterfaceViewModel((ExternalInterfaceType)model, provider) as T;
			return null;
		}

		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (model.GetType() == typeof(InstanceHierarchyType)) return typeof(InstanceHierarchyViewModel);
			if (model.GetType() == typeof(InternalElementType)) return typeof(InternalElementViewModel);
			if (model.GetType() == typeof(ExternalInterfaceType)) return typeof(InterfaceViewModel);
			return null;
		}
	}
}