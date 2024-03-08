using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace M01_First_WPF_Proj
{
    public class Grouping
    {
        Random random = new Random();
        private int num_of_points;
        private int threshold_distance;
        private List<List<Point>> sublists = new List<List<Point>>();

        // Encapsulated field (Property) ... getters and setters
        // MasterList is the original set of points
        public List<Point> MasterList { get; set; }


        public Grouping(int threshold_distance, int num_of_points)
        {
            this.num_of_points = num_of_points;
            this.threshold_distance = threshold_distance;


            // Create the new list (empty at first)
            MasterList = new List<Point>();

            for (int i = 0; i < num_of_points; i++)
            {
                int rx = random.Next(10, 700);
                int ry = random.Next(10, 700);
                Point p = new Point(rx, ry); // make random point
                MasterList.Add(p); // add point to list
            }

        }

        public List<List<Point>> applyThreseholdsMakeGroups()
        {
            // workingList is a copy of the MasterList
            List<Point> workingList = new List<Point>();
            foreach (Point p in MasterList) {
                workingList.Add(p);
            }

            // the result build into list (a list of group lists)
            List<List<Point>> lists = new List<List<Point>>();

            /////////////////////////////////////////////
            while (workingList.Any())
            {
                List<Point> newlist = GetGroup(workingList, threshold_distance);
                lists.Add(newlist);  // this is a group
                                    // the workingList is shrunk by the points taken to make this group
            }

            return lists; // the final list of groups
        }


        public List<Point> GetGroup(List<Point> workingList, int threshold_distance)
        {

            List<Point> currentList = new List<Point>();  // will be the new group
            List<Point> straggleList = new List<Point>(); // helps fill the group (no stragglers at end of the list)

            straggleList.Add(workingList[0]);   // first entry at least, might be alone in the list

            while (straggleList.Any())
            {
                // algo pulls from straggleList and adds to currentList
                Point p = straggleList[0];
                straggleList.Remove(p);
                currentList.Add(p);

                foreach (Point q in workingList)
                {
                    if (p != q)
                    {
                        double distance = Point.Subtract(p, q).Length;
                        if (distance < threshold_distance)
                        {
                            if (!currentList.Contains(p))
                            {
                                currentList.Add(p);  // add if not already in current
                            }
                            if (!currentList.Contains(q))
                            {
                                currentList.Add(q);  // add if not already in current
                                straggleList.Add(q); // 
                            }

                        }
                        else
                        {
                            Console.WriteLine("TOO FAR p=" + p.ToString() + " to q=" + q.ToString());
                        }
                    }
                }
            }

            // remove these points from workingList
            // eventually all points are removed from workingList
            foreach (Point q in currentList) {
                if (currentList.Contains(q)) { 
                    workingList.Remove(q);
                }
            }

            return currentList;
        }
    }
}