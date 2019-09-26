using System.IO;
using System.Linq;
using Aml.Container;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using Xunit;

namespace Aml.ViewModel.UnitTests
{
	public class KinematicJointValueTests
	{
		[Fact]
		public void SerDesTest()
		{
			var provider = new AmlDocument();

			var expected = new KinematicJointValue(provider)
			{
				Name = "foobar",
				Value = 1d,
				DefaultValue = 3d,
				Minimum = -10d,
				Maximum = 20d
			};

			var value = new KinematicJointValue((AttributeType)expected.CaexObject, provider);

			Assert.Equal(expected.Name, value.Name);
			Assert.Equal(expected.Value, value.Value);
			Assert.Equal(expected.Minimum, value.Minimum);
			Assert.Equal(expected.Maximum, value.Maximum);


			var ie = provider.CaexDocument.Create<InternalElementType>();
			var ih = provider.CaexDocument.Create<InstanceHierarchyType>();
			ie.Attribute.Insert((AttributeType)expected.CaexObject);
			ih.InternalElement.Insert(ie);
			provider.CaexDocument.CAEXFile.InstanceHierarchy.Insert(ih);

			var fullPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			provider.CaexDocument.SaveToFile(fullPath, true);

			var d = CAEXDocument.LoadFromFile(fullPath);
			var p = new AmlDocument(d);
			File.Delete(fullPath);

			value = new KinematicJointValue(d.CAEXFile.InstanceHierarchy.First().InternalElement.First().Attribute.First(), provider);

			Assert.Equal(expected.Name, value.Name);
			Assert.Equal(expected.Value, value.Value);
			Assert.Equal(expected.Minimum, value.Minimum);
			Assert.Equal(expected.Maximum, value.Maximum);
		}

		[Fact]
		public void FactoryTest()
		{
			var provider = new AmlDocument();

			var expected = new KinematicJointValue(provider)
			{
				Name = "foobar",
				Value = 1d,
				DefaultValue = 3d,
				Minimum = -10d,
				Maximum = 20d
			};

			var ie = provider.CaexDocument.Create<InternalElementType>();
			ie.Attribute.Insert((AttributeType)expected.CaexObject);
			Assert.Equal(1, ie.Attribute.Count);

			var viewModel = new InternalElementViewModel(ie, provider);
			var values = new ViewModelCollection<KinematicJointValue>(ie.Attribute, viewModel);
			Assert.Single(values);
			Assert.Equal(typeof(KinematicJointValue), values.First().GetType());
		}

		[Fact]
		public void LoadTest()
		{
			var document = CAEXDocument.LoadFromFile(@"D:\test.aml");
			var provider = new AmlDocument(document);
			var ie = document.CAEXFile.InstanceHierarchy.First().InternalElement.First();
			var factory = new KinematicFactory();
			var kinematic = factory.Create<Kinematic>(ie, provider);
		}
	}
}