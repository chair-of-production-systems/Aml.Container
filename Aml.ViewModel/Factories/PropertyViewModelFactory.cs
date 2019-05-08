using System;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class PropertyViewModelFactory : ICaexViewModelFactory
	{
		private static Type[] _types;

		public Type[] Types => _types ?? (_types = new[]
		{
			typeof(BasePropertyViewModel),
		});

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			if (!(model is AttributeType attribute)) return null;
			switch (attribute.AttributeDataType)
			{
				case XMLDataTypeMapper.StringTypeName:
					return new StringPropertyViewModel(attribute.Name, attribute.Value, attribute.Unit, provider) as T;
				case XMLDataTypeMapper.BooleanTypeName:
					return new BooleanPropertyViewModel(attribute.Name, bool.Parse(attribute.Value), attribute.Unit, provider) as T;
				case XMLDataTypeMapper.IntTypeName:
					return new IntegerPropertyViewModel(attribute.Name, int.Parse(attribute.Value), attribute.Unit, provider) as T;
				case XMLDataTypeMapper.DoubleTypeName:
					return new DoublePropertyViewModel(attribute.Name, double.Parse(attribute.Value), attribute.Unit, provider) as T;
				default:
					return null;
			}
		}

		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is AttributeType attribute)) return null;
			if (!attribute.RefAttributeType.Equals("PropertyAttribute")) return null;
			switch (attribute.AttributeDataType)
			{
				case XMLDataTypeMapper.StringTypeName:
					return typeof(StringPropertyViewModel);
				case XMLDataTypeMapper.BooleanTypeName:
					return typeof(BooleanPropertyViewModel);
				case XMLDataTypeMapper.IntTypeName:
					return typeof(IntegerPropertyViewModel);
				case XMLDataTypeMapper.DoubleTypeName:
					return typeof(DoublePropertyViewModel);
				default:
					return null;
			}
		}
	}
}