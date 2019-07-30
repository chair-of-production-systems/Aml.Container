using Aml.Container;
using Aml.Contracts;
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
			using (
				var doc = AmlxDocument.Create())
			{
				var project = new ProjectViewModel(doc);

				// DH ABB IRB 120
				// theta, d, a, alpha
				//   0	165	  0	  0
				//   0	125	  0	-90
				// -90	  0	270	  0
				//   0	  0	 70	-90
				//   0	302	  0	 90
				//   0	  0	  0	-90

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
				var j4 = new KinematicJointValue(doc)
				{
					Name = "J4",
					Minimum = -160,
					Maximum = 160,
					DefaultValue = 0
				};
				var j5 = new KinematicJointValue(doc)
				{
					Name = "J5",
					Minimum = -120,
					Maximum = 120,
					DefaultValue = 30
				};
				var j6 = new KinematicJointValue(doc)
				{
					Name = "J6",
					Minimum = -400,
					Maximum = 400,
					DefaultValue = 0
				};

				var cadDir = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Resources\ABB_IRB_120");

				var link0 = new KinematicLink(doc) { Name = "Base Link" };
				var baseFlange = new Flange(doc)
				{
					Name = "Base",
					Type = FlangeType.Base
				};
				baseFlange.Frame = new FrameProperty(doc)
				{
					Name = "BaseFrame",
					X = 0,
					Y = 0,
					Z = 0,
					RX = 0,
					RY = 0,
					RZ = 0
				};
				link0.Flanges.Add(baseFlange);
				var part0 = new PartViewModel(doc);
				var file0 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Base.STEP"));
				var geometry0 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file0.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file0.Location)
				};
				part0.DataConnectors.Add(geometry0);
				link0.Parts.Add(part0);

				var link1 = new KinematicLink(doc) { Name = "Link 1" };
				var part1 = new PartViewModel(doc);
				var file1 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Link1.STEP"));
				var geometry1 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file1.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file1.Location)
				};
				part1.DataConnectors.Add(geometry1);
				link1.Parts.Add(part1);

				var link2 = new KinematicLink(doc) { Name = "Link 2" };
				var part2 = new PartViewModel(doc);
				var file2 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Link2.STEP"));
				var geometry2 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file2.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file2.Location)
				};
				part2.DataConnectors.Add(geometry2);
				link2.Parts.Add(part2);

				var link3 = new KinematicLink(doc) { Name = "Link 3" };
				var part3 = new PartViewModel(doc);
				var file3 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Link3.STEP"));
				var geometry3 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file3.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file3.Location)
				};
				part3.DataConnectors.Add(geometry3);
				link3.Parts.Add(part3);

				var link4 = new KinematicLink(doc) { Name = "Link 4" };
				var part4 = new PartViewModel(doc);
				part4.Frame = new FrameProperty(doc)
				{
					Name = "Link4_Frame",
					X = 0,
					Y = 0,
					Z = 134,
					RX = 0,
					RY = 0,
					RZ = 0
				};
				var file4 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Link4.STEP"));
				var geometry4 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file4.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file4.Location)
				};
				part4.DataConnectors.Add(geometry4);
				link4.Parts.Add(part4);

				var link5 = new KinematicLink(doc) { Name = "Link 5" };
				var part5 = new PartViewModel(doc);
				var file5 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Link5.STEP"));
				var geometry5 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file5.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file5.Location)
				};
				part5.DataConnectors.Add(geometry5);
				link5.Parts.Add(part5);

				var link6 = new KinematicLink(doc) { Name = "Link 6" };
				var tcpFlange = new Flange(doc)
				{
					Name = "TcpFlange",
					Type = FlangeType.Tcp
				};
				tcpFlange.Frame = new FrameProperty(doc)
				{
					Name = "TcpFrame",
					X = 0,
					Y = 0,
					Z = 72,
					RX = 0,
					RY = 0,
					RZ = 0
				};
				link6.Flanges.Add(tcpFlange);
				var part6 = new PartViewModel(doc);
				part6.Frame = new FrameProperty(doc)
				{
					Name = "Link6_Frame",
					X = 0,
					Y = 0,
					Z = 72,
					RX = 0,
					RY = 0,
					RZ = 0
				};
				var file6 = doc.Files.Add(Path.Combine(cadDir, "ABB_IRB_120_Link6.STEP"));
				var geometry6 = new GeometryDataConnectorViewModel(doc)
				{
					Location = file6.Location,
					MimeType = XMLMimeTypeMapper.GetMimeType(file6.Location)
				};
				part6.DataConnectors.Add(geometry6);
				link6.Parts.Add(part6);

				var joint1 = new KinematicJoint(doc, 0, 165, 0, 0)
				{
					Name = "Joint1",
					Base = link0.Id,
					Axis = link1.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j1.Id}"
				};
				var joint2 = new KinematicJoint(doc, 0, 125, 0, -90)
				{
					Name = "Joint2",
					Base = link1.Id,
					Axis = link2.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j2.Id}"
				};
				var joint3 = new KinematicJoint(doc, -90, 0, 270, 0)
				{
					Name = "Joint3",
					Base = link2.Id,
					Axis = link3.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j3.Id}"
				};
				var joint4 = new KinematicJoint(doc, 0, 0, 70, -90)
				{
					Name = "Joint4",
					Base = link3.Id,
					Axis = link4.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j4.Id}"
				};
				var joint5 = new KinematicJoint(doc, 0, 302, 0, 90)
				{
					Name = "Joint5",
					Base = link4.Id,
					Axis = link5.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j5.Id}"
				};
				var joint6 = new KinematicJoint(doc, 0, 0, 0, -90)
				{
					Name = "Joint6",
					Base = link5.Id,
					Axis = link6.Id,
					JointType = KinematicAxisType.Revolution,
					AxisValue = $"{j6.Id}"
				};

				var robot = new Kinematic(doc) { Name = "IRB 120" };
				robot.Links.Add(link0);
				robot.Links.Add(link1);
				robot.Links.Add(link2);
				robot.Links.Add(link3);
				robot.Links.Add(link4);
				robot.Links.Add(link5);
				robot.Links.Add(link6);
				robot.Joints.Add(joint1);
				robot.Joints.Add(joint2);
				robot.Joints.Add(joint3);
				robot.Joints.Add(joint4);
				robot.Joints.Add(joint5);
				robot.Joints.Add(joint6);
				robot.JointValues.Add(j1);
				robot.JointValues.Add(j2);
				robot.JointValues.Add(j3);
				robot.JointValues.Add(j4);
				robot.JointValues.Add(j5);
				robot.JointValues.Add(j6);

				project.Parts.Add(robot);

				var instanceHierachy = project.CaexObject as InstanceHierarchyType;

				var project2 = new ProjectViewModel(instanceHierachy, doc);
				var robot2 = project2.Parts.OfType<Kinematic>().Single();

				Assert.Equal(robot, robot2);
			}
		}
	}
}