﻿using System;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public enum FlangeType
	{
		Undefined,
		Base,
		Tcp,
		Support
	}

	public class Flange : InterfaceViewModel
	{
		private const string FlangeTypePropertyName = "FlangeType";

		private readonly ExternalInterfaceType _interface;
		private FrameProperty _frame;
		private ViewModelCollection<BasePropertyViewModel> _properties;

		public string Name
		{
			get => _interface.Name;
			set => _interface.Name = value;
		}

		public string Id
		{
			get => _interface.ID;
			set => _interface.ID = value;
		}

		public FlangeType Type
		{
			get
			{
				var property = _properties.OfType<StringPropertyViewModel>().FirstOrDefault(x => x.Name == FlangeTypePropertyName);
				if (property == null) return FlangeType.Undefined;
				if (!Enum.TryParse(property.Value, true, out FlangeType value)) return FlangeType.Undefined;
				return value;
			}
			set
			{
				var property = _properties.OfType<StringPropertyViewModel>().FirstOrDefault(x => x.Name == FlangeTypePropertyName);
				if (property == null)
				{
					property = new StringPropertyViewModel(Provider) { Name = FlangeTypePropertyName };
					_properties.Add(property);
				}
				property.Value = value.ToString();
			}
		}

		public FrameProperty Frame
		{
			get
			{
				if (_frame != null) return _frame;
				_frame = _properties.OfType<FrameProperty>().SingleOrDefault();
				if (_frame == null)
				{
					_frame = new FrameProperty(Provider);
					_interface.Attribute.Insert(_frame.CaexObject as AttributeType);
				}
				return _frame;
			}
		}
		public Flange(IAmlProvider provider)
			: base(provider)
		{
			_interface = provider.CaexDocument.Create<ExternalInterfaceType>();
			Initialize();
		}

		public Flange(ExternalInterfaceType model, IAmlProvider provider)
			: base(model, provider)
		{
			_interface = model;
			Initialize();
		}

		private void Initialize()
		{
			CaexObject = _interface;
			_properties = new ViewModelCollection<BasePropertyViewModel>(_interface.Attribute, this);
		}
	}
}