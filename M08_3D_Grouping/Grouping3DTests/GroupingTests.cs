using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cube3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube3D.Tests
{
    [TestClass()]
    public class GroupingTests
    {
        [TestMethod()]
        public void GetGroupTest_01()
        {
            int distanceThreshold = 100;
            int num_of_points = 2;
            Grouping g = null;
            g = new Grouping(distanceThreshold, num_of_points);  // size of master list generated
            g.MasterList[0] = new Dot(0, 0, 0);  // Replace random points to these
            g.MasterList[1] = new Dot(0, 0, 101);

            // each sublist is a grouping
            List<List<Dot>> sublists = g.applyThreseholdsMakeGroups();

            // Num of groups
            int Actual_Group_Count = sublists.Count;
            int Expected_Group_Count = 2; // expected 2 points not grouped
            Assert.AreEqual(Expected_Group_Count, Actual_Group_Count);

            // 1st group?
            Dot Actual_Group_Point = sublists[0][0];  // 1st and only Point of group 01
            Dot Expected_Group_Point = new Dot(0, 0, 0); // expected Point of group 01
            Boolean same = (Actual_Group_Point.X == Expected_Group_Point.X)
                & (Actual_Group_Point.Y == Expected_Group_Point.Y)
                & (Actual_Group_Point.Z == Expected_Group_Point.Z);
            Assert.IsTrue(same);  // Are points same?

            // 2nd group?
            Actual_Group_Point = sublists[1][0];  // 1st and only Point of group 01
            Expected_Group_Point = new Dot(0, 0, 101); // expected Point of group 01
            same = (Actual_Group_Point.X == Expected_Group_Point.X)
                & (Actual_Group_Point.Y == Expected_Group_Point.Y)
                & (Actual_Group_Point.Z == Expected_Group_Point.Z);
            Assert.IsTrue(same);  // Are points same?
        }
        
        
        
        
        
        
        
        
        
        [TestMethod()]
        public void GetGroupTest_WithPointsWithinThreshold()
        {
            int distanceThreshold = 10;
            Grouping g = new Grouping(distanceThreshold, 0); // Initialize with 0 points
            g.MasterList.Clear(); // Ensure the list is empty, though it should already be empty in this case

            // Add dots directly since the list is empty and we cannot use index assignment
            g.MasterList.Add(new Dot(0, 0, 0));
            g.MasterList.Add(new Dot(3, 4, 0)); // Distance is 5 units from (0,0,0) - should be in the same group
            g.MasterList.Add(new Dot(10, 10, 10)); // Far away

            List<List<Dot>> sublists = g.applyThreseholdsMakeGroups();

            Assert.AreEqual(2, sublists.Count); // Expecting 2 groups
            Assert.AreEqual(2, sublists[0].Count); // First group should have 2 dots
            Assert.AreEqual(1, sublists[1].Count); // Second group should have 1 dot
        }

        [TestMethod()]
        public void GetGroupTest_WithLargeNumberOfPoints()
        {
            int distanceThreshold = 50;
            int num_of_points = 100; // Large number of points
            Grouping g = new Grouping(distanceThreshold, num_of_points);
            // Assuming the Grouping constructor randomly distributes points in a reasonable space

            List<List<Dot>> sublists = g.applyThreseholdsMakeGroups();

            // Not asserting on exact outcomes due to random nature, but we can check if it completes within a reasonable time
            Assert.IsTrue(sublists.Count > 0); //  check to ensure grouping was performed
        }
        
        
        [TestMethod()]
        public void GetGroupTest_PointsExactlyAtThreshold()
        {
            int distanceThreshold = 10;
            Grouping g = new Grouping(distanceThreshold, 0); // Init with no points
            g.MasterList.Add(new Dot(0, 0, 0));
            g.MasterList.Add(new Dot(10, 0, 0)); // Exactly at threshold from (0,0,0)

            List<List<Dot>> sublists = g.applyThreseholdsMakeGroups();

            int expectedGroups = 1; // Passes when we make the expected groups 2 meaning it only counts dots in a group if they are under the threshold
            Assert.AreEqual(expectedGroups, sublists.Count, "Points exactly at the threshold are not handled as expected.");
        }

        [TestMethod()]
        public void GetGroupTest_WithNonIntegerValues()
        {
            int distanceThreshold = 5;
            Grouping g = new Grouping(distanceThreshold, 0); // Init with no points
            g.MasterList.Add(new Dot(0.5, 0.5, 0.5));
            g.MasterList.Add(new Dot(3.5, 4.5, 0)); // Within threshold from (0.5, 0.5, 0.5)

            List<List<Dot>> sublists = g.applyThreseholdsMakeGroups();

            Assert.AreEqual(1, sublists.Count, "Non-integer coordinates are not grouped correctly.");
            Assert.AreEqual(2, sublists[0].Count, "Expected two points in the same group.");
        }
        
        [TestMethod()]
        public void GetGroupTest_PointsInLineAboveThreshold()
        {
            int distanceThreshold = 5; // Threshold distance
            Grouping g = new Grouping(distanceThreshold, 0); // Init with no points

            // Adding points in a line, each 6 units apart which is above the threshold
            g.MasterList.Add(new Dot(0, 0, 0));
            g.MasterList.Add(new Dot(6, 0, 0)); // 6 units from the first point
            g.MasterList.Add(new Dot(12, 0, 0)); // 6 units from the second point, and so on

            List<List<Dot>> sublists = g.applyThreseholdsMakeGroups();

            // Expecting each point to be in its own group since they are all spaced more than the threshold distance apart
            int expectedGroups = 3;
            Assert.AreEqual(expectedGroups, sublists.Count, "Each point should form its own group as they are all above the threshold distance from each other.");

            // also, check that each group has exactly one point
            foreach (var sublist in sublists)
            {
                Assert.AreEqual(1, sublist.Count, "Each group should contain exactly one point.");
            }
        }




    }
}