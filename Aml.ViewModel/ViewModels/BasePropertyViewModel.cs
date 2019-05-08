using System;
using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public abstract class BasePropertyViewModel : CaexObjectViewModel
	{
		protected AttributeType _attribute;

		protected BasePropertyViewModel(string name, IAmlProvider provider) : base(provider)
		{
			_attribute = provider.CaexDocument.Create<AttributeType>();
			Name = name;
			Id = Guid.NewGuid().ToString();
			_attribute.RefAttributeType = "PropertyAttribute";
			CaexObject = _attribute;
		}

		public string Name
		{
			get { return _attribute.Name; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				_attribute.Name = value;
			}
		}

		public string Id
		{
			get { return _attribute.ID; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				_attribute.ID = value;
			}
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}