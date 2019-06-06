using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class FrameProperty : BasePropertyViewModel
	{
		#region Consts

		internal const string PropertyName = "Frame";

		#endregion // Consts

		#region Fields

		private DoublePropertyViewModel _xProperty;
		private DoublePropertyViewModel _yProperty;
		private DoublePropertyViewModel _zProperty;
		private DoublePropertyViewModel _rxProperty;
		private DoublePropertyViewModel _ryProperty;
		private DoublePropertyViewModel _rzProperty;

		#endregion // Fields

		#region Properties

		public double X
		{
			get => GetProperty(_xProperty);
			set => SetProperty(ref _xProperty, value);
		}

		public double Y
		{
			get => GetProperty(_yProperty);
			set => SetProperty(ref _yProperty, value);
		}

		public double Z
		{
			get => GetProperty(_zProperty);
			set => SetProperty(ref _zProperty, value);
		}

		public double RX
		{
			get => GetProperty(_rxProperty);
			set => SetProperty(ref _rxProperty, value);
		}

		public double RY
		{
			get => GetProperty(_ryProperty);
			set => SetProperty(ref _ryProperty, value);
		}

		public double RZ
		{
			get => GetProperty(_rzProperty);
			set => SetProperty(ref _rzProperty, value);
		}

		#endregion // Properties

		#region Ctor & Dtor

		public FrameProperty(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public FrameProperty(AttributeType model, IAmlProvider provider) : base(model, provider)
		{
			Initialize();
		}

		private void Initialize()
		{
			Name = PropertyName;
		}

		#endregion // Ctor & Dtor

		private double GetProperty(DoublePropertyViewModel property)
		{
			return property?.Value ?? default(double);
		}

		private void SetProperty(ref DoublePropertyViewModel property, double value)
		{
			if (property == null) property = new DoublePropertyViewModel(Provider);
			property.Value = value;
		}
	}
}