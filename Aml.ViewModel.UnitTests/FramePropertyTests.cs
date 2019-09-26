using Aml.Container;
using Xunit;

namespace Aml.ViewModel.UnitTests
{
	public class FramePropertyTests
	{
		[Fact]
		public void FrameNamingTest()
		{
			var provider = new AmlDocument();
			var frame = new FrameProperty(provider);
			Assert.Equal("Frame", frame.Name);
		}
	}
}