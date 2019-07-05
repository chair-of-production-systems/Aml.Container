using System.Runtime.InteropServices;
using Aml.Container;
using Xunit;

namespace Aml.ViewModel.UnitTests
{
	public class FramePropertyTest
	{
		[Fact]
		public void SetDhParameterMatrixTest()
		{
			var document = new AmlDocument();

			const double expectedTheta = 0;
			const double expectedD = 0;
			const double expectedA = 290;
			const double expectedAlpha = -90;

			var frame = new FrameProperty(document);
			frame.SetDhParameters(expectedTheta, expectedD, expectedA, expectedAlpha);

			frame.ComputeDhParameter(out var theta, out var d, out var a, out var alpha);

			Assert.Equal(expectedTheta, theta, 6);
			Assert.Equal(expectedD, d, 6);
			Assert.Equal(expectedA, a, 6);
			Assert.Equal(expectedAlpha, alpha, 6);
		}
	}
}