using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;
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
			var found = false;
			foreach (var model in _caexSequence)
			{
				if (Equals(model, item.CaexObject)) found = true;
			}
			if (!found) _caexSequence.Insert(item.CaexObject);
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

		/// <summary>
		/// Create a view model for the specified model and add the view model to this collection.
		/// </summary>
		/// <param name="model"></param>
		private void AddModel(ICAEXWrapper model)
		{
			// skip if a view model for the model already exists
			foreach (var vm in this)
			{
				if (Equals(vm.CaexObject, model)) return;
			}

			var factory = CaexViewModelFactoryManager.Instance.GetFactory<T>(model);
			if (factory == null) return;

			var viewModel = factory.Create<T>(model, _parent.Provider);

			// if the needed viewmodel could not be created, just dont add it the collection
			if (viewModel == null) return;
			Add(viewModel);
		}

		public override bool Equals(object other)
		{
			return Equals(other as ViewModelCollection<T>);
		}

		public bool Equals(ViewModelCollection<T> other)
		{
			if (other == null) return false;
			if (Count != other.Count) return false;
			var otherIndices = new int[Count].Select(x => x = -1).ToArray();

			for (int thisIndex = 0; thisIndex < Count; thisIndex++)
			{
				for (int otherIndex = 0; otherIndex < Count; otherIndex++)
				{
					if (otherIndices.Contains(otherIndex)) continue;
					if (this.ElementAt(thisIndex).Equals(other.ElementAt(otherIndex)))
					{
						otherIndices[thisIndex] = otherIndex;
						break;
					}
				}

				if (otherIndices[thisIndex] == -1)
				{
					return false;
				}
			}
			return true;
		}
	}
}