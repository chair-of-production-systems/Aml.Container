using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public interface ICaexViewModelFactory
	{
		/// <summary>
		/// Checks whether an instance of the specified type can be created or not.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		bool CanCreate(Type type);

		/// <summary>
		/// Creates a view model for the specified model instance.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="resolver">Resolver for external locations.</param>
		/// <returns>A view model for the specified model instance.</returns>
		CaexObjectViewModel Create(CAEXBasicObject model, ILocationResolver resolver);

		/// <summary>
		/// Get the type of the view model related to the specified model. If the factory
		/// cannot create a view model for the given model, <c>null</c> is returned.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Type TypeOfViewModel(CAEXBasicObject model);
	}
}