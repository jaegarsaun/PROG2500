using JetBrains.Annotations;
using M01_First_WPF_Proj;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace M08_Grouping_ProjTests
{
    [TestClass]
    [TestSubject(typeof(Grouping))]
    public class GroupingTest
    {

        [TestMethod]
        public void applyThreseholdsMakeGroupsTest()
        {
            int distThreshold = 100;
            int numPoints = 2;
            Grouping g = null;
            g = new Grouping(distThreshold, numPoints); // size of master list
            // replace random points to these vv
            g.MasterList[0] = new Point(0, 0);
            g.MasterList[1] = new Point(0, 101);

            List<List<Point>> sublists = g.applyThreseholdsMakeGroups();
            
            // Num of groups
            int realGroupCount = sublists.Count;
            int expectedGroupCount = 2;
            // Check if the real group count is the same as the one we are expecting to get
            Assert.AreEqual(expectedGroupCount, realGroupCount);
            
            // 1st group check if the point is where it should be
            Point realGroup01Point = sublists[0][0]; // only point in group 1
            Point expectedGroup01Point = new Point(0, 0);
            // Check if the real point in group 1 is actually at 0, 0
            Assert.AreEqual(realGroup01Point, expectedGroup01Point);
            
            // 2nd group check if the point is where it should be
            Point realGroup02Point = sublists[1][0]; // only point in group 2
            Point expectedGroup02Point = new Point(0, 101);
            // Check if the real point in group 1 is actually at 0, 101
            Assert.AreEqual(realGroup02Point, expectedGroup02Point);

        }
        
        [TestMethod]
        public void GetGroup_WithPointsWithinThreshold_ReturnsCorrectGrouping()
        {
            // Arrange
            var grouping = new Grouping(100, 0); // Threshold distance set to 100, no initial points
            var workingList = new List<Point>
            {
                new Point(10, 10),
                new Point(50, 50), // This point is within 100 units of the first point
                new Point(200, 200) // This point is outside the threshold distance
            };
            int thresholdDistance = 100;

            // Act
            var result = grouping.GetGroup(workingList, thresholdDistance);

            // Assert
            Assert.IsTrue(result.Contains(new Point(10, 10)) && result.Contains(new Point(50, 50)));
            Assert.IsFalse(result.Contains(new Point(200, 200))); // This point should not be included
        }
        
        [TestMethod]
        public void GetGroup_WithPointsExactlyAtThreshold_IncludesBothPointsInSameGroup()
        {
            // Arrange
            var grouping = new Grouping(100, 0); // Assuming the threshold distance includes points exactly at it
            var workingList = new List<Point>
            {
                new Point(0, 0),
                new Point(100, 0) // This point is exactly 100 units away from the first point
            };
            int thresholdDistance = 100;

            // Act
            var result = grouping.GetGroup(workingList, thresholdDistance);

            // Assert
            // Check if both points are considered within the same group
            // This assertion is based on the assumption that points at the threshold are included
            Assert.AreEqual(2, result.Count); // Expecting both points to be in the same group
            Assert.IsTrue(result.Contains(new Point(0, 0)) && result.Contains(new Point(100, 0)));
        }
        
        [TestMethod]
        public void ApplyThresholdsMakeGroups_WithEmptyList_ReturnsEmptyGroupList()
        {
            // Arrange
            var grouping = new Grouping(100, 0);
            grouping.MasterList.Clear(); // Ensure the master list is empty

            // Act
            var result = grouping.applyThreseholdsMakeGroups();

            // Assert
            Assert.IsFalse(result.Any()); // Expecting no groups to be returned
        }
        
        [TestMethod]
        public void ApplyThresholdsMakeGroups_WithAllPointsOutsideThreshold_ReturnsIndividualGroups()
        {
            // Arrange
            var grouping = new Grouping(100, 3);
            grouping.MasterList = new List<Point>
            {
                new Point(0, 0),
                // Make two points that are way past the threshold to test that they will be in their own groups
                new Point(200, 200), 
                new Point(400, 400)  
            };

            // Act
            var result = grouping.applyThreseholdsMakeGroups();

            // Assert
            Assert.AreEqual(3, result.Count); // Expecting each point to be in its own group
            foreach (var group in result)
            {
                Assert.AreEqual(1, group.Count); // Each group should contain exactly one point
            }
        }


        

    }
}