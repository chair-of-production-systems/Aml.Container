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
			typeof(BasePropertyViewModel)
		});

		public bool CanCreate<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			if (!(model is AttributeType attribute)) return false;

			if (attribute.RefAttributeType == FrameProperty.RefAttributeType) return true;

			if (attribute.RefAttributeType == KinematicJointValue.AttributeRefTypeName)
				return true;

			switch (attribute.AttributeDataType)
			{
				case XMLDataTypeMapper.StringTypeName:
				case XMLDataTypeMapper.BooleanTypeName:
				case XMLDataTypeMapper.IntTypeName:
				case XMLDataTypeMapper.DoubleTypeName:
					return true;
				default:
					return false;
			}
		}

		public T Create<T>(ICAEXWrapper model, IAmlProvider provider) where T : CaexObjectViewModel
		{
			var t = TypeOfViewModel(model);
			if (t == null) return null;

			var inst = Activator.CreateInstance(t, model, provider) as T;
			return inst;
		}

		public Type TypeOfViewModel(ICAEXWrapper model)
		{
			if (!(model is AttributeType attribute)) return null;

			if (attribute.RefAttributeType == FrameProperty.RefAttributeType) return typeof(FrameProperty);

			if (attribute.RefAttributeType == KinematicJointValue.AttributeRefTypeName)
				return typeof(KinematicJointValue);

			switch (attribute.AttributeDataType)
			{
				case XMLDataTypeMapper.StringTypeName: return typeof(StringPropertyViewModel);
				case XMLDataTypeMapper.BooleanTypeName: return typeof(BooleanPropertyViewModel);
				case XMLDataTypeMapper.IntTypeName: return typeof(IntegerPropertyViewModel);
				case XMLDataTypeMapper.DoubleTypeName: return typeof(DoublePropertyViewModel);
				default:
					// TODO: how to handle missing attribute data type
					return null;
			}
		}
	}
}