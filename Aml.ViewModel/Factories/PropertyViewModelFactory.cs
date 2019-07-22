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

			if (attribute.Name == FrameProperty.PropertyName) return typeof(FrameProperty);

			if (attribute.RefAttributeType == KinematicJointValue.AttributeRefTypeName)
				return typeof(KinematicJointValue);

			//if (!attribute.RefAttributeType.Equals("PropertyAttribute")) return null;
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