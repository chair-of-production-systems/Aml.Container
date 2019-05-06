﻿using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Aml.ViewModel
{
    public abstract class PropertyViewModel<T> : BasePropertyViewModel
    {
        protected readonly InternalElementType _internalElement;

        protected PropertyViewModel(string name, string unit, IAmlProvider provider) : base(provider)
        {
            Name = name;
            Unit = unit;
            _internalElement = provider.CaexDocument.Create<InternalElementType>();
            _internalElement.Name = name;
            CaexObject = _internalElement;
            if (!string.IsNullOrEmpty(Unit))
            {
                var unitAttribute = GetAttribute("Unit", true);
                unitAttribute.Value = Unit;
                unitAttribute.AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(string));
            }
        }

        public override string Name { get; set; }

        public abstract T Value { get; set; }

        public override string Unit { get; set; }

        public override IEnumerable<CaexObjectViewModel> GetDescendants()
        {
            yield break;
        }

        protected AttributeType GetAttribute(string name, bool create = false)
        {
            var attribute = _internalElement.Attribute.GetCAEXAttribute(name);
            if (attribute == null && create)
            {
                attribute = _internalElement.New_Attribute(name);
            }
            return attribute;
        }

        protected void Update()
        {

            var typedValueAttribute = GetAttribute("Value", true);
            typedValueAttribute.Value = Value.ToString();
            typedValueAttribute.AttributeDataType = XMLDataTypeMapper.GetXmlDataType(typeof(T));
        }
    }
}