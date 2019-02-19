using System;
using System.Collections.Generic;
using System.Linq;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class CaexViewModelFactoryManager
	{
		#region Singleton Implementation

		/// <summary>
		/// Static holder for instance, need to use lambda to construct since constructor private
		/// </summary>
		private static readonly Lazy<CaexViewModelFactoryManager> LazyInstance = new Lazy<CaexViewModelFactoryManager>(() => new CaexViewModelFactoryManager());

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static CaexViewModelFactoryManager Instance => LazyInstance.Value;

		#endregion // Singleton Implementation

		private readonly List<ICaexViewModelFactory> _factories;

		private CaexViewModelFactoryManager()
		{
			_factories = new List<ICaexViewModelFactory>
			{
				new CaexViewModelFactory(),
				new ExternalDataConnectorViewModelFactory()
			};
		}

		public IEnumerable<ICaexViewModelFactory> GetFactoriesForViewModel(Type type)
		{
			return _factories.Where(x => x.CanCreate(type));
		}

		private IEnumerable<ICaexViewModelFactory> GetFactories(CAEXBasicObject model)
		{
			return _factories.Where(x => x.TypeOfViewModel(model) != null);
		}

		public ICaexViewModelFactory GetFactory(CAEXBasicObject model)
		{
			var factories = GetFactories(model).ToList();
			if (factories.Count == 0) throw new Exception("Factory missing for type " + model.GetType());
			if (factories.Count == 1) return factories[0];

			ICaexViewModelFactory match = null;
			foreach (var factory in factories)
			{
				var hasSubClass = false;
				foreach (var f in factories)
				{
					var thisType = factory.TypeOfViewModel(model);
					var otherType = f.TypeOfViewModel(model);
					if (otherType.IsSubclassOf(thisType)) hasSubClass = true;
				}

				if (!hasSubClass)
				{
					if (match != null) throw new Exception("Multiple factories found");
					match = factory;
				}
			}

			if (match == null) throw new Exception("Weird: factory missing for type " + model.GetType());
			return match;
		}

		public void Register(ICaexViewModelFactory factory)
		{
			_factories.Add(factory);
		}
	}
}