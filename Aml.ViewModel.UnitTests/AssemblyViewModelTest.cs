using System;
using Aml.Container;
using Aml.Engine.CAEX;
using Xunit;

namespace Aml.ViewModel.UnitTests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			const string asm1Name = "Slider";
			const string asm2Name = "Bed Assembly";
			const string part1Name = "Base";
			const string part2Name = "Bed";
			const string part3Name = "Flange";

			var doc = new AmlDocument();
			var project = new ProjectViewModel(doc);
			doc.CaexDocument.CAEXFile.InstanceHierarchy.Insert(project.CaexObject as InstanceHierarchyType);

			var asm1 = new AssemblyViewModel(doc) { Name = asm1Name };
			var asm2 = new AssemblyViewModel(doc) { Name = asm2Name };
			var part1 = new PartViewModel(doc) { Name = part1Name };
			var part2 = new PartViewModel(doc) { Name = part2Name };
			var part3 = new PartViewModel(doc) { Name = part3Name };

			project.Parts.Add(asm1);

			asm1.Parts.Add(asm2);
			asm1.Parts.Add(part1);

			asm2.Parts.Add(part2);
			asm2.Parts.Add(part3);

			var stream = doc.CaexDocument.SaveToStream(true);

			var caex = CAEXDocument.LoadFromStream(stream);
			var doc2 = new AmlDocument(caex);
			var ih = doc2.CaexDocument.CAEXFile.InstanceHierarchy.First;
			var vm = new ProjectViewModel(ih, doc2);
		} 
	}
}
