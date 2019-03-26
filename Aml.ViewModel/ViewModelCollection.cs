using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class ViewModelCollection<T> : ObservableCollection<T> where T : CaexObjectViewModel
	{
		private readonly ICAEXSequence _caexSequence;
		private readonly CaexObjectViewModel _parent;

		public ViewModelCollection(ICAEXSequence caexSequence, CaexObjectViewModel parent)
		{
			_caexSequence = caexSequence;
			_parent = parent;

			foreach (var item in caexSequence)
			{
				AddModel(item);
			}
		}

		//protected ViewModelCollection(IEnumerable collection, CaexObjectViewModel parent)
		//{
		//	_parent = parent;

		//	foreach (var item in collection.OfType<CAEXBasicObject>())
		//	{
		//		AddModel(item);
		//	}

		//	if (collection is INotifyCollectionChanged observable)
		//	{
		//		observable.CollectionChanged += OnCollectionChanged;
		//	}
		//}

		protected override void InsertItem(int index, T item)
		{
			_caexSequence.Insert(item.CaexObject);
			base.InsertItem(index, item);
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems.OfType<CAEXBasicObject>())
				{
					// TODO: skip items where a view model already exists
					AddModel(item);
				}
			}
		}

		private void AddModel(ICAEXWrapper model)
		{
			var factory = CaexViewModelFactoryManager.Instance.GetFactory<T>(model);
			if (factory == null) return;

			var viewModel = factory.Create<T>(model, _parent.Provider);
			if (viewModel == null) throw new Exception("Cannot create view model");
			Add(viewModel);
		}
	}
}