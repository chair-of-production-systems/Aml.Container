using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public abstract class BaseComponent : CaexObjectViewModel
	{
		// ReSharper disable once InconsistentNaming
		protected readonly InternalElementType _internalElement;
		private FrameProperty _frame;

		public string Name
		{
			get => _internalElement.Name;
			set => _internalElement.Name = value;
		}

		public string Id
		{
			get => _internalElement.ID;
			set => _internalElement.ID = value;
		}

		public ViewModelCollection<BasePropertyViewModel> Properties { get; set; }

		public FrameProperty Frame
		{
			get
			{
				if (_frame != null) return _frame;
				_frame = Properties.OfType<FrameProperty>().SingleOrDefault();
				if (_frame == null)
				{
					_frame = new FrameProperty(Provider);
					_internalElement.Attribute.Insert(_frame.CaexObject as AttributeType);
				}
				return _frame;
			}
			set
			{
				var property = Properties.OfType<FrameProperty>().SingleOrDefault();
				if (property == null)
				{
					_frame = value;
					_internalElement.Attribute.Insert(_frame.CaexObject as AttributeType);
				}
				else
				{
					_internalElement.Attribute.RemoveElement(_frame.CaexObject as AttributeType);
					_frame = value;
					_internalElement.Attribute.Insert(_frame.CaexObject as AttributeType);
				}
			}
		}

		protected BaseComponent(IAmlProvider provider) : base(provider)
		{
			_internalElement = provider.CaexDocument.Create<InternalElementType>();
			Initialize();
		}

		protected BaseComponent(InternalElementType model, IAmlProvider provider)
			: base(provider)
		{
			_internalElement = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _internalElement;
			Properties = new ViewModelCollection<BasePropertyViewModel>(_internalElement.Attribute, this);
		}
	}
}