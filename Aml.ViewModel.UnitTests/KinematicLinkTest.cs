using Aml.Container;
using Aml.Engine.CAEX;
using Xunit;

namespace Aml.ViewModel.UnitTests
{
	public class KinematicLinkTest
	{
		[Fact]
		public void SerDesTest()
		{
			// create view model
			var provider = new AmlDocument();
			var link = new KinematicLink(provider);
			
			var flange = new Flange(provider);
			flange.Type = FlangeType.Base;
			flange.Frame.SetDhParameters(0, 10, 0, 90);

			link.Flanges.Add(flange);

			// get model
			var model = link.CaexObject as InternalElementType;

			// create new view model based on model
			var link2 = new KinematicLink(model, provider);

			// compare
			Assert.Equal(link.Flanges.Count, link2.Flanges.Count);
		}
	}
}