using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aml.ViewModel;

namespace Aml.View
{
	public abstract class BaseView
	{
		protected CaexObjectViewModel _viewModel;

		protected BaseView(CaexObjectViewModel viewModel)
		{
			_viewModel = viewModel;
		}
	}
}
