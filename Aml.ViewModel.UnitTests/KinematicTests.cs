using Aml.Container;
using Aml.Engine.CAEX;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Aml.ViewModel.UnitTests
{
	public class KinematicTests
	{
		[Fact]
		public void FactoryTest()
		{
			// Create and safe file name and file path, to be usabel after the using clause.
			var amlxFileName = "IRB_120.amlx";
			var amlxFilePath = Path.Combine(Environment.CurrentDirectory, amlxFileName);
			using (var doc = AmlxDocument.Create())
			{
				var project = new ProjectViewModel(doc);

				var j1 = new KinematicJointValue(doc)
				{
					Name = "J1",
					Minimum = -165,
					Maximum = 165,
					DefaultValue = 0
				};
				var j2 = new KinematicJointValue(doc)
				{
					Name = "J2",
					Minimum = -110,
					Maximum = 110,
					DefaultValue = 0
				};
				var j3 = new KinematicJointValue(doc)
				{
					Name = "J3",
					Minimum = -110,
					Maximum = 70,
					DefaultValue = 0
				};

				var link0 = new KinematicLink(doc) {Name = "Base Link"};
				link0.Flanges.Add(new Flange(doc)
				{
					Name = "Base",
					Type = FlangeType.Base
				});

				var link1 = new KinematicLink(doc) {Name = "Link 1"};

				var link2 = new KinematicLink(doc) {Name = "Link 2"};

				var joint1 = new KinematicJoint(doc, 0, 0, 290, -90)
				{
					Name = "Joint1",
					Base = link0.Id,
					Axis = link1.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j1.Id}"
				};
				var joint2 = new KinematicJoint(doc, -90, 0, 270, 0)
				{
					Name = "Joint2",
					Base = link1.Id,
					Axis = link2.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j2.Id}"
				};

				var robot = new Kinematic(doc) {Name = "IRB 120"};
				robot.Links.Add(link0);
				robot.Links.Add(link1);
				robot.Links.Add(link2);
				robot.Joints.Add(joint1);
				robot.Joints.Add(joint2);
				robot.JointValues.Add(j1);
				robot.JointValues.Add(j2);

				project.Parts.Add(robot);

				var instanceHierachy = project.CaexObject as InstanceHierarchyType;

				var project2 = new ProjectViewModel(instanceHierachy, doc);
				var robot2 = project2.Parts.OfType<Kinematic>().Single();

				Assert.Equal(robot, robot2);
			}
		}
	}
}