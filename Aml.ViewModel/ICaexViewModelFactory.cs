using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public interface ICaexViewModelFactory
	{
		/// <summary>
		/// Gets a list of types that the factory can create.
		/// </summary>
		Type[] Types { get; }

		//bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel;

		T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T: CaexObjectViewModel;

		/// <summary>
		/// Get the type of the view model related to the specified model. If the factory
		/// cannot create a view model for the given model, <c>null</c> is returned.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Type TypeOfViewModel(ICAEXWrapper model);
	}
}