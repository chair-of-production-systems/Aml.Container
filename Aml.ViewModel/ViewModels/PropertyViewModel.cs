using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Aml.ViewModel
{
	public abstract class BasePropertyViewModel<T> : BasePropertyViewModel
	{
		protected BasePropertyViewModel(string name, string unit, IAmlProvider provider) 
			: base(name, provider)
		{
			Unit = unit;
			AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(T));
		}

		public string AttributeDataType
		{
			get { return _attribute.AttributeDataType; }
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
			get { return _attribute.Unit; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				_attribute.Unit = value;
			}
		}
	}
}