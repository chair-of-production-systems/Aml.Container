using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class CaexViewModelFactory : ICaexViewModelFactory
	{
		/// <inheritdoc />
		public bool CanCreate(Type type) => type.IsSubclassOf(typeof(CaexObjectViewModel));

		/// <inheritdoc />
		public CaexObjectViewModel Create(CAEXBasicObject model, ILocationResolver resolver)
		{
			//if (model.GetType() == typeof(CAEXDocument)) return new CaexDocumentViewModel((CAEXDocument)model, resolver);
			if (model.GetType() == typeof(InstanceHierarchyType)) return new InstanceHierarchyViewModel((InstanceHierarchyType)model, resolver);
			if (model.GetType() == typeof(InternalElementType)) return new InternalElementViewModel((InternalElementType)model, resolver);
			if (model.GetType() == typeof(ExternalInterfaceType)) return new InterfaceViewModel((ExternalInterfaceType)model, resolver);
			return null;
		}

		public Type TypeOfViewModel(CAEXBasicObject model)
		{
			//if (model.GetType() == typeof(CAEXDocument)) return typeof(CaexDocumentViewModel);
			if (model.GetType() == typeof(InstanceHierarchyType)) return typeof(InstanceHierarchyViewModel);
			if (model.GetType() == typeof(InternalElementType)) return typeof(InternalElementViewModel);
			if (model.GetType() == typeof(ExternalInterfaceType)) return typeof(InterfaceViewModel);
			return null;
		}
	}
}