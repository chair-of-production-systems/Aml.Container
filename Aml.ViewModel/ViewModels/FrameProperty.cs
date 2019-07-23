using System;
using System.Globalization;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class FrameProperty : BasePropertyViewModel
	{
		#region Consts

		internal const string PropertyName = "Frame";
		private const double Epsilon = 1e-6;

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
			get => GetProperty(ref _xProperty, nameof(X));
			set => SetProperty(ref _xProperty, nameof(X), value);
		}

		public double Y
		{
			get => GetProperty(ref _yProperty, nameof(Y));
			set => SetProperty(ref _yProperty, nameof(Y), value);
		}

		public double Z
		{
			get => GetProperty(ref _zProperty, nameof(Z));
			set => SetProperty(ref _zProperty, nameof(Z), value);
		}

		public double RX
		{
			get => GetProperty(ref _rxProperty, nameof(RX));
			set => SetProperty(ref _rxProperty, nameof(RX), value);
		}

		public double RY
		{
			get => GetProperty(ref _ryProperty, nameof(RY));
			set => SetProperty(ref _ryProperty, nameof(RY), value);
		}

		public double RZ
		{
			get => GetProperty(ref _rzProperty, nameof(RZ));
			set => SetProperty(ref _rzProperty, nameof(RZ), value);
		}

		#endregion // Properties

		#region Ctor & Dtor

		public FrameProperty(IAmlProvider provider)
			: base(provider)
		{
			Initialize();
		}

		public FrameProperty(AttributeType model, IAmlProvider provider) 
			: base(model, provider)
		{
			Initialize();
		}

		private void Initialize()
		{
			Name = PropertyName;

			foreach (var attribute in _attribute.Attribute)
			{
				switch (attribute.Name)
				{
					case nameof(X):
						X = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
					case nameof(Y):
						Y = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
					case nameof(Z):
						Z = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
					case nameof(RX):
						RX = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
					case nameof(RY):
						RY = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
					case nameof(RZ):
						RZ = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
						break;
				}
			}
		}

		#endregion // Ctor & Dtor

		#region Public API

		/// <summary>
		/// 
		/// </summary>
		/// <param name="matrix"></param>
		public void GetRowMajorMatrix(double[] matrix)
		{
			const double deg2Rag = Math.PI / 180d;
			var cr = Math.Cos(RX * deg2Rag);
			var cp = Math.Cos(RY * deg2Rag);
			var cy = Math.Cos(RZ * deg2Rag);
			var sr = Math.Sin(RX * deg2Rag);
			var sp = Math.Sin(RY * deg2Rag);
			var sy = Math.Sin(RZ * deg2Rag);

			matrix[0] = cy * cp;
			matrix[1] = cy * sp * sr - sy * cr;
			matrix[2] = cy * sp * cr + sy * sr;
			matrix[3] = X;

			matrix[4] = sy * cp;
			matrix[5] = sy * sp * sr + cy * cr;
			matrix[6] = sy * sp * cr - cy * sr;
			matrix[7] = Y;

			matrix[8] = -sp;
			matrix[9] = cp * sr;
			matrix[10] = cp * cr;
			matrix[11] = Z;

			matrix[12] = 0d;
			matrix[13] = 0d;
			matrix[14] = 0d;
			matrix[15] = 1d;
		}

		public void GetColumnMajorMatrix(double[] matrix)
		{
			const double deg2Rag = Math.PI / 180d;
			var cr = Math.Cos(RX * deg2Rag);
			var cp = Math.Cos(RY * deg2Rag);
			var cy = Math.Cos(RZ * deg2Rag);
			var sr = Math.Sin(RX * deg2Rag);
			var sp = Math.Sin(RY * deg2Rag);
			var sy = Math.Sin(RZ * deg2Rag);

			matrix[0] = cy * cp;
			matrix[1] = sy * cp;
			matrix[2] = -sp;
			matrix[3] = 0d;

			matrix[4] = cy * sp * sr - sy * cr;
			matrix[5] = sy * sp * sr + cy * cr;
			matrix[6] = cp * sr;
			matrix[7] = 0d;

			matrix[8] = cy * sp * cr + sy * sr;
			matrix[9] = sy * sp * cr - cy * sr;
			matrix[10] = cp * cr;
			matrix[11] = 0d;

			matrix[12] = X;
			matrix[13] = Y;
			matrix[14] = Z;
			matrix[15] = 1d;
		}

		public double[] ToRowMajorMatrix()
		{
			var matrix = new double[16];
			GetRowMajorMatrix(matrix);
			return matrix;
		}

		public double[] ToColumnMajorMatrix()
		{
			var matrix = new double[16];
			GetColumnMajorMatrix(matrix);
			return matrix;
		}

		public void SetFromRowMajorMatrix(double[] matrix, double scale = 1.0)
		{
			// | matrix[ 0]  matrix[ 1]  matrix[ 2]  matrix[ 3] |
			// | matrix[ 4]  matrix[ 5]  matrix[ 6]  matrix[ 7] |
			// | matrix[ 8]  matrix[ 9]  matrix[10]  matrix[11] |
			// | matrix[12]  matrix[13]  matrix[14]  matrix[15] |

			var r11 = matrix[0];
			var r12 = matrix[1];
			var r21 = matrix[4];
			var r22 = matrix[5];
			var r31 = matrix[8];
			var r32 = matrix[9];
			var r33 = matrix[10];

			double rotX, rotZ;
			var rotY = Math.Atan2(-r31, Math.Sqrt(r11 * r11 + r21 * r21));
			if (rotY + Epsilon > Math.PI / 2)
			{
				// WARNING. Not a unique solution.
				//rotY = Math.PI / 2;
				//rotZ = 0;
				//rotX = Math.Atan2(matrix[1], matrix[5]);
				rotX = Math.Atan2(r12, r22);
				rotY = Math.PI / 2;
				rotZ = 0;
			}
			else if (rotY - Epsilon < -Math.PI / 2)
			{
				// WARNING. Not a unique solution.
				//rotY = -Math.PI / 2;
				//rotZ = 0;
				//rotX = -Math.Atan2(matrix[1], matrix[5]);
				rotX = -Math.Atan2(r12, r22);
				rotY = -Math.PI / 2;
				rotZ = 0;
			}
			else
			{
				//var cosY = Math.Cos(rotY);
				//rotZ = Math.Atan2(matrix[1] / cosY, matrix[0] / cosY);
				//rotX = Math.Atan2(matrix[9] / cosY, matrix[10] / cosY);
				rotX = Math.Atan2(r32, r33);
				rotZ = Math.Atan2(r21, r11);
			}

			var x = scale * matrix[3];
			var y = scale * matrix[7];
			var z = scale * matrix[11];
			var rx = rotX / Math.PI * 180d;
			var ry = rotY / Math.PI * 180d;
			var rz = rotZ / Math.PI * 180d;

			if (Math.Abs(x) < Epsilon) { x = 0; }
			if (Math.Abs(y) < Epsilon) { y = 0; }
			if (Math.Abs(z) < Epsilon) { z = 0; }
			if (Math.Abs(rx) < Epsilon) { rx = 0; }
			if (Math.Abs(ry) < Epsilon) { ry = 0; }
			if (Math.Abs(rz) < Epsilon) { rz = 0; }

			X = x;
			Y = y;
			Z = z;
			RX = rx;
			RY = ry;
			RZ = rz;
		}

		public void SetFromColumnMajorMatrix(double[] matrix, double scale = 1.0)
		{
			var m = new[]
			{
				matrix[0], matrix[4], matrix[8], matrix[12],
				matrix[1], matrix[5], matrix[9], matrix[13],
				matrix[2], matrix[6], matrix[10], matrix[14],
				matrix[3], matrix[7], matrix[11], matrix[15]
			};
			SetFromRowMajorMatrix(m, scale);
		}

		public bool IsIdentity()
		{
			if (Math.Abs(X) > Epsilon) return false;
			if (Math.Abs(Y) > Epsilon) return false;
			if (Math.Abs(Z) > Epsilon) return false;
			if (Math.Abs(RX) > Epsilon) return false;
			if (Math.Abs(RY) > Epsilon) return false;
			if (Math.Abs(RZ) > Epsilon) return false;
			return true;
		}

		public void ComputeDhParameter(out double theta, out double d, out double a, out double alpha)
		{
			const double rad2Deg = 180d / Math.PI;
			var matrix = ToRowMajorMatrix();

			// from https://en.wikipedia.org/wiki/Denavit%E2%80%93Hartenberg_parameters#Denavit%E2%80%93Hartenberg_matrix
			var cosTheta = matrix[0];
			var sinTheta = matrix[4];
			var cosAlpha = matrix[10];
			var sinAlpha = matrix[9];
			d = matrix[11];
			a = (Math.Abs(cosTheta) > 0.5) ? matrix[3] / cosTheta : matrix[7] / sinTheta;
			theta = Math.Acos(cosTheta) * rad2Deg;
			alpha = Math.Acos(cosAlpha) * rad2Deg;
			if (sinTheta < 0) theta = -theta;
			if (sinAlpha < 0) alpha = -alpha;
		}

		public void SetDhParameters(double theta, double d, double a, double alpha)
		{
			const double deg2Rag = Math.PI / 180d;

			var ct = Math.Cos(theta * deg2Rag);
			var st = Math.Sin(theta * deg2Rag);
			var ca = Math.Cos(alpha * deg2Rag);
			var sa = Math.Sin(alpha * deg2Rag);

			var matrix = new double[]
			{
				ct, -st*ca, st*sa, a*ct,
				st, ct*ca, -ct*sa, a*st,
				0, sa, ca, d,
				0, 0, 0, 1
			};

			SetFromRowMajorMatrix(matrix);
		}

		#endregion // Public API

		private double GetProperty(ref DoublePropertyViewModel property, string name)
		{
			FindPropertyInstance(ref property, name);
			return property?.Value ?? default(double);
		}

		private void SetProperty(ref DoublePropertyViewModel property, string name, double value)
		{
			FindPropertyInstance(ref property, name);
			if (property == null)
			{
				property = new DoublePropertyViewModel(Provider) { Name = name, Id = null };
				_attribute.Attribute.Insert(property.CaexObject as AttributeType);
			}
			property.Value = value;
		}

		private void FindPropertyInstance(ref DoublePropertyViewModel property, string name)
		{
			if (property != null) return;

			var attribute = _attribute.Attribute.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if (attribute != null) property = new DoublePropertyViewModel(attribute, Provider);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Frame);
		}

		public bool Equals(FrameProperty other)
		{
			if (other == null) return false;

			return Math.Abs(X - other.X) < Epsilon
			    && Math.Abs(Y - other.Y) < Epsilon
			    && Math.Abs(Z - other.Z) < Epsilon
			    && Math.Abs(RX - other.RX) < Epsilon
			    && Math.Abs(RY - other.RY) < Epsilon
			    && Math.Abs(RZ - other.RZ) < Epsilon;
		}
	}
}