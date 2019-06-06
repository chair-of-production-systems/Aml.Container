using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public abstract class BasePropertyViewModel<T> : BasePropertyViewModel
	{
		protected BasePropertyViewModel(IAmlProvider provider)
			: base(provider)
		{
			AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(T));
		}

		protected BasePropertyViewModel(AttributeType model, IAmlProvider provider)
			: base(model, provider)
		{
			AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(T));
		}

		[Obsolete]
		protected BasePropertyViewModel(string name, string unit, IAmlProvider provider) 
			: base(name, provider)
		{
			Unit = unit;
			AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(T));
		}

		// TODO: allow setter?
		public string AttributeDataType
		{
			get => _attribute.AttributeDataType;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				_attribute.AttributeDataType = value;
			}
		}

		public abstract T Value { get; set; }

		public string Unit
		{
			get => _attribute.Unit;
			set
			{
				// TODO: remove?
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				_attribute.Unit = value;
			}
		}
	}
}