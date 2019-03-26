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
			typeof(InstanceHierarchyType),
			typeof(InternalElementType),
			typeof(ExternalInterfaceType)
		});

		/// <inheritdoc />
		public bool CanCreate(Type type) => type.IsSubclassOf(typeof(CaexObjectViewModel));

		/// <inheritdoc />
		public CaexObjectViewModel Create(ICAEXWrapper model, IAmlProvider provider)
		{
			//if (model.GetType() == typeof(CAEXDocument)) return new CaexDocumentViewModel((CAEXDocument)model, resolver);
			if (model.GetType() == typeof(InstanceHierarchyType)) return new InstanceHierarchyViewModel((InstanceHierarchyType)model, provider);
			if (model.GetType() == typeof(InternalElementType)) return new InternalElementViewModel((InternalElementType)model, provider);
			if (model.GetType() == typeof(ExternalInterfaceType)) return new InterfaceViewModel((ExternalInterfaceType)model, provider);
			return null;
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
			//if (model.GetType() == typeof(CAEXDocument)) return typeof(CaexDocumentViewModel);
			if (model.GetType() == typeof(InstanceHierarchyType)) return typeof(InstanceHierarchyViewModel);
			if (model.GetType() == typeof(InternalElementType)) return typeof(InternalElementViewModel);
			if (model.GetType() == typeof(ExternalInterfaceType)) return typeof(InterfaceViewModel);
			return null;
		}
	}
}