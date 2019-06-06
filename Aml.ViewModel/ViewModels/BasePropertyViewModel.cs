using System;
using System.Collections.Generic;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public abstract class BasePropertyViewModel : CaexObjectViewModel
	{
		#region Fields

		protected AttributeType _attribute;

		#endregion // Fields

		#region Properties

		/// <summary>
		/// Name of the property.
		/// </summary>
		public string Name
		{
			get => _attribute.Name;
			set => _attribute.Name = value;
		}

		/// <summary>
		/// Unique identifier of the attribute.
		/// </summary>
		/// <remarks>Some kind of attributes do not require an identifier.</remarks>
		public string Id
		{
			get => _attribute.ID;
			set => _attribute.ID = value;
		}

		#endregion // Properties

		[Obsolete("Use new BasePropertyViewModel(provider) { Name = name } instead.")]
		protected BasePropertyViewModel(string name, IAmlProvider provider)
			: base(provider)
		{
			_attribute = provider.CaexDocument.Create<AttributeType>();
			Name = name;
			Id = Guid.NewGuid().ToString("D");
			//_attribute.RefAttributeType = "PropertyAttribute";
			CaexObject = _attribute;
		}

		protected BasePropertyViewModel(IAmlProvider provider)
			: base(provider)
		{
			_attribute = provider.CaexDocument.Create<AttributeType>();
			Id = Guid.NewGuid().ToString("D");
			// TODO: why do we need this? _attribute.RefAttributeType = "PropertyAttribute";
			CaexObject = _attribute;
		}

		protected BasePropertyViewModel(AttributeType model, IAmlProvider provider)
			: base(provider)
		{
			_attribute = model;
			CaexObject = _attribute;
		}

		public override IEnumerable<CaexObjectViewModel> GetDescendants()
		{
			yield break;
		}
	}
}