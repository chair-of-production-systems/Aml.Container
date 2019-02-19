using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class ViewModelCollection<T> : ObservableCollection<T> where T : class
	{
		private readonly CaexObjectViewModel _parent;

		public ViewModelCollection(IEnumerable collection, CaexObjectViewModel parent)
		{
			_parent = parent;

			foreach (var item in collection.OfType<CAEXBasicObject>())
			{
				AddModel(item);
			}

			if (collection is INotifyCollectionChanged observable)
			{
				observable.CollectionChanged += OnCollectionChanged;
			}
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems.OfType<CAEXBasicObject>())
				{
					AddModel(item);
				}
			}
		}

		private void AddModel(CAEXBasicObject model)
		{
			var factory = CaexViewModelFactoryManager.Instance.GetFactory(model);
			var viewModel = factory.Create(model, _parent.Resolver) as T;
			if (viewModel == null) throw new Exception("Cannot create view model");
			Add(viewModel);
		}
	}
}